using ArtSolutions.MUN.Web.Areas.Companies.Models;
using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Areas.Reports.Models
{
    public class MonthlyIncomeStatementByServices
    {
        #region public methods
        public MonthlyIncomeStatementByServicesModel Get()
        {
            MonthlyIncomeStatementByServicesModel model = new MonthlyIncomeStatementByServicesModel();
            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.ReportCompanyModel = GetReportCompany();
            return model;
        }
        public MonthlyIncomeStatementByServicesModel Get(DateTime closingDate, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            MonthlyIncomeStatementByServicesModel model = new MonthlyIncomeStatementByServicesModel();
            model = GetByPaging(closingDate, commaSeperatedServiceIDs, pageIndex, pageSize);
            model.MonthlyIncomeStatementByServicesList = model.MonthlyIncomeStatementByServicesList ?? new List<MonthlyIncomeStatementByServicesList>();
            model.ReportCompanyModel = GetReportCompany(closingDate);
            return model;
        }
        public MonthlyIncomeStatementByServicesModel GetList(DateTime closingDate, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            return Get(closingDate, commaSeperatedServiceIDs, pageIndex, pageSize);
        }
        #endregion

        #region private methods
        private ReportCompanyModel GetReportCompany(DateTime? closingDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.MonthlyIncomeStatementByServices, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 8, null, null, null, null, true, closingDate, false);
        }
        private MonthlyIncomeStatementByServicesModel GetByPaging(DateTime closingDate, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "closingDate", Value = closingDate.ToString("d", CultureInfo.InvariantCulture) });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceIDs", Value = commaSeperatedServiceIDs });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<MonthlyIncomeStatementByServicesModel>(APISelector.Municipality, true, "api/Report/MonthlyIncomeStatementByServicesGet", "GET", lstParameter);
        }
        #endregion
    }
    public class QuarterlyIncomeStatementByServices
    {
        public QuarterlyIncomeStatementByServicesModel Get()
        {

            QuarterlyIncomeStatementByServicesModel model = new QuarterlyIncomeStatementByServicesModel();
            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.ReportCompanyModel = GetReportCompany();
            model.YearList = new SelectList(Enumerable.Range(1990, DateTime.Now.Year - 1990 + 1).Reverse()).ToList();
            return model;
        }
        public QuarterlyIncomeStatementByServicesModel Get(int Quarter, int Year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            QuarterlyIncomeStatementByServicesModel model = new QuarterlyIncomeStatementByServicesModel();
            model = GetByPaging(Quarter, Year, commaSeperatedServiceIDs, pageIndex, pageSize);
            model.MonthlyIncomeStatementByServicesList = model.MonthlyIncomeStatementByServicesList ?? new List<MonthlyIncomeStatementByServicesList>();
            model.ReportCompanyModel = GetReportCompany(new DateTime(Year, (Quarter*3)-2, 1), new DateTime(Year, (Quarter * 3), 1).AddMonths(1).AddDays(-1));
            return model;
        }
        public QuarterlyIncomeStatementByServicesModel GetList(int Quarter, int Year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            return Get(Quarter, Year, commaSeperatedServiceIDs, pageIndex, pageSize);
        }
        
        private ReportCompanyModel GetReportCompany(DateTime? closingStartDate = null, DateTime? closingEndDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.QuarterlyIncomeStatementByServices, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, closingStartDate, closingEndDate, null, null, true, closingStartDate, false);
        }
        private QuarterlyIncomeStatementByServicesModel GetByPaging(int Quarter, int Year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "quarter", Value = Quarter });
            lstParameter.Add(new NameValuePair { Name = "year", Value = Year });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceIDs", Value = commaSeperatedServiceIDs });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<QuarterlyIncomeStatementByServicesModel>(APISelector.Municipality, true, "api/Report/QuarterlyIncomeStatementByServicesGet", "GET", lstParameter);
        }
    }

    public class YearlyIncomeStatementByServices
    {
        public YearlyIncomeStatementByServicesModel Get()
        {

            YearlyIncomeStatementByServicesModel model = new YearlyIncomeStatementByServicesModel();
            model.ServiceList = new SelectList(new Services.Models.Service().Get(), "ID", "Name").OrderBy(x => x.Text);
            model.ReportCompanyModel = GetReportCompany();
            model.YearList = new SelectList(Enumerable.Range(1990, DateTime.Now.Year - 1990 + 1).Reverse()).ToList();
            return model;
        }
        public YearlyIncomeStatementByServicesModel Get(int Year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            YearlyIncomeStatementByServicesModel model = new YearlyIncomeStatementByServicesModel();
            model = GetByPaging(Year, commaSeperatedServiceIDs, pageIndex, pageSize);
            model.YearlyIncomeStatementByServicesList = model.YearlyIncomeStatementByServicesList ?? new List<YearlyIncomeStatementByServicesList>();
            model.ReportCompanyModel = GetReportCompany(new DateTime(Year, 1, 1), new DateTime(Year, 12, 1).AddMonths(1).AddDays(-1));
            return model;
        }
        public YearlyIncomeStatementByServicesModel GetList(int Year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            return Get( Year, commaSeperatedServiceIDs, pageIndex, pageSize);
        }

        private ReportCompanyModel GetReportCompany(DateTime? closingStartDate = null, DateTime? closingEndDate = null)
        {
            return new ReportCompanyModel().Get(ArtSolutions.MUN.Web.Resources.Report.YearlyIncomeStatementByServices, ArtSolutions.MUN.Web.Resources.Report.DepartmentOfFinance, 6, closingStartDate, closingEndDate, null, null, true, closingStartDate, false);
        }
        private YearlyIncomeStatementByServicesModel GetByPaging(int Year, string commaSeperatedServiceIDs, int pageIndex, int pageSize)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();           
            lstParameter.Add(new NameValuePair { Name = "year", Value = Year });
            lstParameter.Add(new NameValuePair { Name = "commaSeperatedServiceIDs", Value = commaSeperatedServiceIDs });
            lstParameter.Add(new NameValuePair { Name = "pageIndex", Value = pageIndex });
            lstParameter.Add(new NameValuePair { Name = "pageSize", Value = pageSize });
            return new RestSharpHandler().RestRequest<YearlyIncomeStatementByServicesModel>(APISelector.Municipality, true, "api/Report/YearlyIncomeStatementByServicesGet", "GET", lstParameter);
        }
    }
}