using System;
using System.Linq;

namespace ArtSolutions.MUN.Core
{
    public class CompanyParameterModel
    {
        #region Public Property
        public int ID { get; set; }
        public int Precision { get; set; }
        public bool UseLeapYear { get; set; }
        public bool UseAccountReceivableJournal { get; set; }
        #endregion

        public MUNCompanyParameterGet_Result Get(int ID)
        {
            using (CompanyDataModelContainer context = new CompanyDataModelContainer())
            {
                return context.MUNCompanyParameterGet(ID).FirstOrDefault();
            }
        }
    }
}
