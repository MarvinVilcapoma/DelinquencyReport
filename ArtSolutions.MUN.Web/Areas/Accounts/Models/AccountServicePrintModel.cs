using ArtSolutions.MUN.Web.Areas.Collections.Models;
using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtSolutions.MUN.Web.Areas.Accounts.Models
{
    public class AccountServicePrintModel
    {
        #region public properties
        public CompanyModel Company { get; set; }
        public AccountServiceModel AccountService { get; set; }
        public AddressModel AccountAddress { get; set; }
        public PrintTemplateModel PrintTemplate { get; set; }
        public AccountBusinessModel AccountBusiness { get; set; }
        public AccountServiceCollectionPrintModel AccountServiceCollection { get; set; }
        public List<AccountServiceCollectionPrintModel> AccountServiceDebtList { get; set; }
        #endregion

        #region Public Methods
        public AccountServicePrintModel GetPrint(int? accountserviceId, int? printTemplateId)
        {
            AccountServicePrintModel list = new AccountServicePrintModel();
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "accountServiceID", Value = accountserviceId });
            lstParameter.Add(new NameValuePair { Name = "printTemplateId", Value = printTemplateId });
            list = new RestSharpHandler().RestRequest<AccountServicePrintModel>(APISelector.Municipality, true, "api/AccountService/PrintGet", "GET", lstParameter, null);
            return list;
        }

        public AccountServicePrintModel GetPrintModel(int? accountserviceId, int? printTemplateId)
        {
            AccountServicePrintModel model = new AccountServicePrintModel().GetPrint(accountserviceId, printTemplateId);

            if (printTemplateId == 4)
                model.PrepareSuspensionOrderPrintTemplate();
            else
                model.PreparePrintTemplate();

            return model;
        }
        public bool Print(int id, byte[] original_rowversion)
        {
            AccountServiceModel obj = new AccountServiceModel();
            obj.ID = id;
            obj.PrintDate = Common.CurrentDateTime;
            obj.ModifiedUserID = UserSession.Current.Id;
            obj.ModifiedDate = Common.CurrentDateTime;
            obj.RowVersion = original_rowversion;
            List<object> lstObjParameter = new List<object>();
            lstObjParameter.Add(obj);
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/Print", "POST", null, lstObjParameter);
        }
        public bool RePrintLog(int id)
        {
            List<NameValuePair> param = new List<NameValuePair>();
            param.Add(new NameValuePair() { Name = "ID", Value = id });
            return new RestSharpHandler().RestRequest<bool>(APISelector.Municipality, true, "api/AccountService/RePrintLog", "POST", param, null);
        }
        public void PreparePrintTemplate()
        {
            string printData = this.PrintTemplate.TemplateContent;

            if (!string.IsNullOrEmpty(printData))
            {
                //Company
                printData = printData.Replace("#CompanyName#", this.Company.Name);

                if (this.Company.LogoID > 0)
                    printData = printData.Replace("#ImgURL#", ArtSolutions.MUN.Web.Common.FileURL + this.Company.LogoID);
                else
                    printData = printData.Replace("#ImgURL#", Common.GetAbsoluteUrl("/Content/Images/no_image_available.png"));

                //Account Service                
                printData = printData.Replace("#CurrentDate#", DateTime.Now.ToString("d"));
                printData = printData.Replace("#RegisterNumber#", this.AccountService.LicenseNumber);
                printData = printData.Replace("#DisplayName#", this.AccountService.AccountName);
                printData = printData.Replace("#BusinessDisplayName#", this.AccountService.AccountName);
                printData = printData.Replace("#SSN#", this.AccountService.SSEIN);
                printData = printData.Replace("#FiscalYear#", this.AccountService.Year.ToString());
                printData = printData.Replace("#FromDate#", this.AccountService.StartDate != null && this.AccountService.StartDate != DateTime.MinValue ? this.AccountService.StartDate.Value.ToString("dd/MMM/yyyy").Replace("-", "/") : "-");
                printData = printData.Replace("#ToDate#", this.AccountService.EndDate != null && this.AccountService.EndDate != DateTime.MinValue ? this.AccountService.EndDate.Value.ToString("dd/MMM/yyyy").Replace("-", "/") : "-");
                printData = printData.Replace("#IssueNumber#", this.AccountService.IssueNumber.ToString().PadLeft(7, '0') ?? "0000001");

                printData = printData.Replace("#Name#", this.AccountService.DisplayName);
                printData = printData.Replace("#CustomField3#", this.AccountService.CustomField3);
                printData = printData.Replace("#CustomDateField#", this.AccountService.CustomDateField.HasValue ? this.AccountService.CustomDateField.Value.ToString("D") : null);
                printData = printData.Replace("#LicenseCustomDateField#", this.AccountService.CustomDateField.HasValue ? this.AccountService.CustomDateField.Value.ToString("d") : DateTime.Now.ToString("d"));
                printData = printData.Replace("#ServiceName#", this.AccountService.ServiceName);

                printData = printData.Replace("#AuthorizedActivity#", this.AccountService.CustomField4);
                printData = printData.Replace("#PropertyName#", this.AccountService.CustomField5);

                if (this.AccountService.AccountTypeID == 5)
                {
                    printData = printData.Replace("#DBA#", this.AccountBusiness.DBAName);
                    printData = printData.Replace("#BusinessDescription#", this.AccountBusiness.BusinessDescription);
                }
                else
                {
                    printData = printData.Replace("#DBA#", this.AccountService.DisplayName);
                    printData = printData.Replace("#BusinessDescription#", this.AccountService.BusinessDescription);
                }

                //Account Business
                printData = printData.Replace("#Classification#", this.AccountBusiness.NAICSCode);
                printData = printData.Replace("#Description#", this.AccountBusiness.BusinessDescription);

                //Address
                printData = printData.Replace("#StateName#", this.AccountAddress.State);
                printData = printData.Replace("#CityName#", this.AccountAddress.City);

                AddressModel address = this.AccountAddress;
                string addr = address.AddressLine1;
                if (!string.IsNullOrEmpty(address.AddressLine2))
                    addr = addr + ", " + address.AddressLine2;
                addr = addr + ",";
                if (!string.IsNullOrEmpty(address.City))
                    addr = addr + address.City;
                if (!string.IsNullOrEmpty(address.State))
                    addr = addr + ", " + address.State;
                if (!string.IsNullOrEmpty(address.TwoLetterIsoCode))
                    addr = addr + ", " + address.TwoLetterIsoCode;
                if (!string.IsNullOrEmpty(address.ZipPostalCode))
                    addr = addr + " - " + address.ZipPostalCode;

                printData = printData.Replace("#Address#", addr);

                //Account Service Collection Detail
                printData = printData.Replace("#PaidAmount#", this.AccountServiceCollection.TotalPaidAmount.ToString("C"));
                printData = printData.Replace("#PRINCIPAL#", this.AccountServiceCollection.TotalPrincipal.ToString("C"));
            }

            this.PrintTemplate.TemplateContent = printData;
        }
        public void PrepareSuspensionOrderPrintTemplate()
        {
            string printData = this.PrintTemplate.TemplateContent;

            if (!string.IsNullOrEmpty(printData))
            {
                //Company
                printData = printData.Replace("#CompanyName#", this.Company.Name);

                if (this.Company.LogoID > 0)
                    printData = printData.Replace("#ImgURL#", ArtSolutions.MUN.Web.Common.FileURL + this.Company.LogoID);
                else
                    printData = printData.Replace("#ImgURL#", Common.GetAbsoluteUrl("/Content/Images/no_image_available.png"));

                //Account Service Detail
                printData = printData.Replace("#CurrentDate#", DateTime.Now.ToString("d"));
                printData = printData.Replace("#DisplayName#", this.AccountService.AccountName);
                printData = printData.Replace("#SSN#", this.AccountService.SSEIN);
                printData = printData.Replace("#Property#", this.AccountService.Property);

                //Address               
                AddressModel address = this.AccountAddress;
                string addr = address.AddressLine1;
                if (!string.IsNullOrEmpty(address.AddressLine2))
                    addr = addr + ", " + address.AddressLine2;
                addr = addr + ",";
                if (!string.IsNullOrEmpty(address.City))
                    addr = addr + address.City;
                if (!string.IsNullOrEmpty(address.State))
                    addr = addr + ", " + address.State;
                if (!string.IsNullOrEmpty(address.TwoLetterIsoCode))
                    addr = addr + ", " + address.TwoLetterIsoCode;
                if (!string.IsNullOrEmpty(address.ZipPostalCode))
                    addr = addr + " - " + address.ZipPostalCode;

                printData = printData.Replace("#Address#", addr);

                printData = printData.Replace("#MeterNumber#", this.AccountService.CustomField1);
                printData = printData.Replace("#CurrentDateInWord#", @DateTime.Now.ToString("D"));

                //Account Service Debt Detail
                StringBuilder strPaymentDetail = new StringBuilder();
                foreach (var item in this.AccountServiceDebtList)
                {
                    strPaymentDetail.Append("<tr>");
                    strPaymentDetail.Append("<td class='left table-border-td'>" + @item.ServiceName + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.TotalPrincipal.ToString("C") + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.TotalInterest.ToString("C") + "</td>");
                    strPaymentDetail.Append("<td class='right table-border-td'>" + @item.Total.ToString("C") + "</td>");
                    strPaymentDetail.Append("</tr>");
                }
                printData = printData.Replace("#CollectionDebtDetailItem#", strPaymentDetail.ToString());
                printData = printData.Replace("#PrincipalTotal#", this.AccountServiceDebtList.Sum(x => x.TotalPrincipal).ToString("C"));
                printData = printData.Replace("#InteresesTotal#", this.AccountServiceDebtList.Sum(x => x.TotalInterest).ToString("C"));
                printData = printData.Replace("#GrandTotal#", this.AccountServiceDebtList.Sum(x => x.Total).ToString("C"));

                //Collector Info
                UserProfileViewModel userProfileModel = new UserProfile().GetUserByUserIDs(this.AccountService.CreatedUserID.ToString()).FirstOrDefault();
                printData = printData.Replace("#Collector#", string.IsNullOrEmpty(userProfileModel.FullName) ? userProfileModel.Email : userProfileModel.FullName);
            }
            this.PrintTemplate.TemplateContent = printData;
        }
        #endregion
    }
}