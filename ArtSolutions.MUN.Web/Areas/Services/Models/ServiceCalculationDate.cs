using ArtSolutions.MUN.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArtSolutions.MUN.Web.Areas.Services.Models
{
    public class ServiceCalculationDate
    {
        #region public methods
        public List<ServiceCalculationDateModel> Get(int? id, int? fiscalYearID)
        {
            List<NameValuePair> lstParameter = new List<NameValuePair>();
            lstParameter.Add(new NameValuePair { Name = "id", Value = id });
            lstParameter.Add(new NameValuePair { Name = "fiscalYearID", Value = fiscalYearID });
            return new RestSharpHandler().RestRequest<List<ServiceCalculationDateModel>>(APISelector.Municipality, true, "api/Service/ServiceCalculationDateGet", "GET", lstParameter);
        }

        public void GenerateCalculationDateTable(ServiceModel service)
        {
            // [1] - Declare Variables
            int seq = 1, i = 1;
            DateTime validationStartDate = DateTime.MinValue;
            DateTime currentPeriodEndDate = DateTime.MinValue;

            // [2] - Get Period for service frequency
            List<FrequencyModel> freqList = new FrequencyModel().Get();
            int period = freqList.Where(x => x.ID == service.FrequencyID).SingleOrDefault().Period.Value;
            int monthPart = 12 / period;
            DateTime start = service.StartDate;
            int currentMonth = monthPart;
            string previousMonthEndDate = "";
            // [3] - Set custom fields required for validation 
            for (int startYear = service.EffectiveFrom.Year; startYear <= service.EffectiveTo.Year; startYear++)
            {
                while (i <= period)
                {
                    ServiceCalculationDateModel obj = new Models.ServiceCalculationDateModel();
                    SetCalculationDateValues(obj, service.StartDate, ref start, ref startYear, ref currentMonth, ref monthPart, ref validationStartDate, ref seq, ref previousMonthEndDate);
                    previousMonthEndDate = obj.CurrentPeriodEndDate;
                    if (currentMonth == 12)
                        currentMonth = monthPart;
                    else
                        currentMonth = currentMonth + monthPart;
                    start = start.AddMonths(monthPart);
                    i++;
                    seq++;
                    service.ServiceCalculationDateList.Add(obj);
                }
                i = 1;
            }
        }

        public void GenerateCalculationDateTable_Append(ServiceModel service)
        {
            if (service.EffectiveTo_Original != null && service.EffectiveTo != service.EffectiveTo_Original)
            {
                // [1] - Declare Variables
                int seq = service.ServiceCalculationDateList.Count() + 1, i = 1;
                DateTime validationStartDate = DateTime.MinValue;
                DateTime currentPeriodEndDate = DateTime.MinValue;

                // [2] - Get Period for service frequency
                List<FrequencyModel> freqList = new FrequencyModel().Get();
                int period = freqList.Where(x => x.ID == service.FrequencyID).SingleOrDefault().Period.Value;
                int monthPart = 12 / period;
                int currentMonth = monthPart;
                DateTime start = service.StartDate;
                string previousMonthEndDate = "";
                // [3] - Prepare New CalculationDateList to append with existing.
                for (int startYear = service.EffectiveTo_Original.Value.AddYears(1).Year; startYear <= service.EffectiveTo.Year; startYear++)
                {
                    if (service.ServiceCalculationDateList.Where(x => x.FiscalYearID == startYear).Count() == 0)
                    {
                        while (i <= period)
                        {
                            ServiceCalculationDateModel obj = new Models.ServiceCalculationDateModel();

                            SetCalculationDateValues(obj, service.StartDate, ref start, ref startYear, ref currentMonth, ref monthPart, ref validationStartDate, ref seq, ref previousMonthEndDate);
                            previousMonthEndDate = obj.CurrentPeriodEndDate;

                            SetDueDates(service, obj);
                            if (currentMonth == 12)
                                currentMonth = monthPart;
                            else
                                currentMonth = currentMonth + monthPart;
                            start = start.AddMonths(monthPart);
                            i++;
                            seq++;
                            service.ServiceCalculationDateList_Append.Add(obj);
                            service.ServiceCalculationDateList.Add(obj);
                        }
                        i = 1;
                    }
                }
            }
        }

        public void GenerateCalculationDateTable_ExistingDates(ServiceModel service)
        {
            // [1] - Declare Variables
            int seq = 1;
            DateTime validationStartDate = DateTime.MinValue;
            DateTime currentPeriodEndDate = DateTime.MinValue;

            // [2] - Get Period for service frequency
            List<FrequencyModel> freqList = new FrequencyModel().Get();
            int period = freqList.Where(x => x.ID == service.FrequencyID).SingleOrDefault().Period.Value;
            int monthPart = 12 / period;
            int currentMonth = monthPart;

            // [3] - Set custom fields required for validation 
            string previousMonthEndDate = "";
            foreach (var obj in service.ServiceCalculationDateList)
            {
                if (validationStartDate == DateTime.MinValue)
                    validationStartDate = new DateTime(obj.FiscalYearID, obj.EndPeriodID, 1).AddMonths(1);
                else
                    validationStartDate = validationStartDate.AddMonths(monthPart);
                obj.ValStartDate = validationStartDate.ToShortDateString();

                obj.SequenceID = seq++;
                obj.CurrentPeriodStartDate = previousMonthEndDate == "" ? new DateTime(obj.FiscalYearID - 1, service.StartDate.Month, service.StartDate.Day).ToShortDateString() : DateTime.Parse(previousMonthEndDate).AddDays(1).ToShortDateString();
                obj.CurrentPeriodEndDate = new DateTime(obj.FiscalYearID - 1, service.StartDate.Month, service.StartDate.Day).AddMonths(currentMonth).AddDays(-1).ToShortDateString();
                previousMonthEndDate = obj.CurrentPeriodEndDate;

                if (currentMonth == 12)
                    currentMonth = monthPart;
                else
                    currentMonth = currentMonth + monthPart;

                if ((DateTime.Parse(obj.FillingDueDate) > Common.CurrentDateTime) || service.IsTestMode == true)
                    obj.IsEditable = true;
                else
                    obj.IsEditable = false;
            }
        }

        public void UpdateCalculationDateList(ServiceModel service)
        {
            foreach (ServiceCalculationDateModel obj in service.ServiceCalculationDateList)
            {
                SetDueDates(service, obj);
                // Remove culture before calling API
                obj.FillingDueDate = !string.IsNullOrEmpty(obj.FillingDueDate) ? DateTime.Parse(obj.FillingDueDate).ToString("d", CultureInfo.InvariantCulture) : obj.FillingDueDate;
                obj.PaymentDueDate = !string.IsNullOrEmpty(obj.PaymentDueDate) ? DateTime.Parse(obj.PaymentDueDate).ToString("d", CultureInfo.InvariantCulture) : obj.PaymentDueDate;
                obj.DiscountDate = !string.IsNullOrEmpty(obj.DiscountDate) ? DateTime.Parse(obj.DiscountDate).ToString("d", CultureInfo.InvariantCulture) : obj.DiscountDate;
                obj.PaymentGraceDate = !string.IsNullOrEmpty(obj.PaymentGraceDate) ? DateTime.Parse(obj.PaymentGraceDate).ToString("d", CultureInfo.InvariantCulture) : obj.PaymentGraceDate;

                //19-Feb-2020
                if (service.DiscountDueType == 3)
                    obj.DiscountPercentage = service.DiscountPercentage;
                else if (service.DiscountDueType <= 2)
                    obj.DiscountPercentage = service.ServiceCalculationDateList.Max(x => x.DiscountPercentage);
            }

        }

        public void RemoveCalculationDates_OutOfEffectiveDateRange(ServiceModel service)
        {
            service.ServiceCalculationDateList = service.ServiceCalculationDateList.Where(x => x.FiscalYearID <= service.EffectiveTo.Year).ToList();
        }
        #endregion

        #region private methods
        private void SetDueDates(ServiceModel service, ServiceCalculationDateModel obj)
        {
            if (service.UseFixedFillingDueDate != null && !service.UseFixedFillingDueDate.Value && obj.FillingDueDate == null)
            {
                obj.FillingDueDate = DateTime.Parse(obj.CurrentPeriodEndDate).AddDays(service.FillingDueDays.Value).ToString("d");
            }
            if (service.UseFixedPaymentDueDate != null && !service.UseFixedPaymentDueDate.Value && obj.PaymentDueDate == null)
            {
                obj.PaymentDueDate = DateTime.Parse(obj.CurrentPeriodEndDate).AddDays(service.PaymentDueDays.Value).ToString("d");
            }
            if (service.UseFixedDiscountDate != null && !service.UseFixedDiscountDate.Value && obj.DiscountDate == null)
            {
                obj.DiscountDate = DateTime.Parse(obj.CurrentPeriodEndDate).AddDays(service.DiscountDueDays.Value).ToString("d");
                obj.DiscountPercentage = service.DiscountPercentage;
            }
            if (service.UseFixedPaymentGracePeriod != null && !service.UseFixedPaymentGracePeriod.Value && obj.PaymentGraceDate == null)
            {
                obj.PaymentGraceDate = DateTime.Parse(obj.CurrentPeriodEndDate).AddDays(service.PaymentGracePeriod.Value).ToString("d");
            }
            //if (service.FilingTypeID == 4)
            if (service.FilingTypeID == 4 || service.FilingTypeID == 5)
            {
                obj.FillingDueDate = DateTime.Parse(obj.CurrentPeriodStartDate).ToString("d");
                obj.PaymentDueDate = DateTime.Parse(obj.CurrentPeriodEndDate).ToString("d");

                //14-Feb-2020 - CO-1349
                obj.DiscountDate = null;
                obj.DiscountPercentage = null;
                obj.PaymentGraceDate = null;
            }
        }

        private void SetCalculationDateValues(ServiceCalculationDateModel obj, DateTime serviceStartDate, ref DateTime currentPeriodStartDate, ref int currentYear, ref int currentMonth, ref int monthPart, ref DateTime valStartDate, ref int seq, ref string previousMonthEndDate)

        {
            obj.FiscalYearID = currentYear;
            obj.StartPeriodID = currentPeriodStartDate.Month;
            obj.SequenceID = seq;
            if (valStartDate == DateTime.MinValue)
                valStartDate = new DateTime(obj.FiscalYearID, currentPeriodStartDate.Month, 1).AddMonths(monthPart);
            else
                valStartDate = valStartDate.AddMonths(monthPart);
            obj.ValStartDate = valStartDate.ToShortDateString();
            obj.CurrentPeriodStartDate = previousMonthEndDate == "" ? new DateTime(obj.FiscalYearID - 1, serviceStartDate.Month, serviceStartDate.Day).ToShortDateString() : DateTime.Parse(previousMonthEndDate).AddDays(1).ToShortDateString();
            obj.CurrentPeriodEndDate = new DateTime(obj.FiscalYearID - 1, serviceStartDate.Month, serviceStartDate.Day).AddMonths(currentMonth).AddDays(-1).ToShortDateString();
            obj.EndPeriodID = DateTime.Parse(obj.CurrentPeriodEndDate).Month;
            obj.IsEditable = true;
        }
        #endregion
    }
}