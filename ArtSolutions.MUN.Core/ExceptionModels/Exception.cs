using ArtSolutions.MUN.Core;
using System;
using System.Linq;

namespace ArtSolutions.ExceptionHandler
{
    public class Exception
    {
        #region Properties
        public int ExceptionID { get; set; }
        public Guid ApplicationID { get; set; }
        public string Application { get; set; }
        public Guid ModuleID { get; set; }
        public string Module { get; set; }
        public Guid FeatureID { get; set; }
        public string Feature { get; set; }
        public Guid UserID { get; set; }
        public string User { get; set; }
        public DateTime ExceptionDateTime { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string ExceptionName { get; set; }
        public string Source { get; set; }
        public string Host { get; set; }
        #endregion
        public static string Log(Guid featureID, Guid token, string methodName, string language, System.Exception exception)
        {
            try
            {
                using (ExceptionHandlerDataModelContainer context = new ExceptionHandlerDataModelContainer())
                {
                    Exception _exption = new Exception();
                    int outError;
                    Int32.TryParse(exception.Message, out outError);
                    if (outError > 0)
                        return _exption.GetErrorMessage(outError, language);
                    else if (exception.InnerException != null)
                    {                      
                        Int32.TryParse(exception.InnerException.Message, out outError);
                        if (outError > 0)
                            return _exption.GetErrorMessage(outError, language);
                        else
                            return exception.InnerException.Message;
                    }

                    if (exception.GetType() == typeof(SecurityException))
                        return _exption.GetErrorMessage(50004, language);

                    EXCLogInsert_Result model = context.EXCLogInsert(featureID, token, System.DateTime.UtcNow, "-1", "ERROR", methodName, exception.Message, (exception.InnerException == null ? exception.StackTrace : exception.InnerException.ToString()), exception.Source, "").FirstOrDefault();
                    return "Error Log: " + model.ID.ToString();
                }
            }
            catch (System.Exception)
            { }
            return string.Empty;
        }

        public string GetErrorMessage(int id, string language)
        {
            using (ExceptionHandlerDataModelContainer context = new ExceptionHandlerDataModelContainer())
            {
                return context.EXCMessageGet(id, language).FirstOrDefault();
            }
        }
    }
}
