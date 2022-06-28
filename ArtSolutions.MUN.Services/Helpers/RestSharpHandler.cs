using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Services.Helpers
{
    public class RestSharpHandler
    {
        public T RestRequest<T>(APISelector apiSelector, string url, string methodType, List<NameValuePair> lstParameters, List<object> lstObjParameter = null, bool withAuth = false, Guid? token = null, string language = null, int companyId = 0) where T : new()
        {
            var restUrl = url;

            switch (apiSelector)
            {
                case APISelector.Security:
                    restUrl = Common.URLSecurityAPIPath + url;
                    break;
                case APISelector.Finance:
                    restUrl = Common.URLFinanceAPIPath + url;
                    break;
            }

            var client = new RestClient(restUrl);

            RestRequest request = null;
            switch (methodType.ToUpper())
            {
                case "GET":
                    request = new RestRequest("", Method.GET);
                    break;
                case "POST":
                    request = new RestRequest("", Method.POST);
                    break;
                default:
                    request = new RestRequest("", Method.GET);
                    break;
            }

            if (withAuth)
            {
                request.AddParameter("Authorization", token, ParameterType.HttpHeader);
                request.AddParameter("Company", companyId, ParameterType.HttpHeader);
                request.AddParameter("Language", language, ParameterType.HttpHeader);
            }

            if (lstParameters != null)
            {
                foreach (NameValuePair param in lstParameters)
                {
                    request.AddParameter(param.Name, param.Value);
                }
            }

            if (lstObjParameter != null)
            {
                foreach (object obj in lstObjParameter)
                {
                    request.AddParameter("application/json; charset=utf-8", JsonConvert.SerializeObject(obj), ParameterType.RequestBody);
                    
                }
            }

            request.Timeout = 1200000;
            client.Timeout = 1200000;
            var response = client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var context = new HttpContextWrapper(HttpContext.Current);
                if (context.Request.IsAjaxRequest())
                {
                    context.Response.Clear();
                    context.Response.Write("UnAuthorized");
                    context.Response.End();
                }
                else
                {
                    HttpContext.Current.Response.Redirect("~/Account/Login");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var badRequestMessage = JsonConvert.DeserializeObject<APIMessage>(response.Content);
                if (badRequestMessage != null)
                    throw new Exception(badRequestMessage.Message);
                else
                    throw new Exception(response.Content);
            }
            else if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (response.Content.Contains("ArtSolutions.ExceptionHandler.SecurityException"))
                    throw new ArtSolutions.ExceptionHandler.SecurityException();
                else
                    throw new Exception(response.ErrorMessage);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
    public enum APISelector
    {
        Security,
        Finance
    }
}