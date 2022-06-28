using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtSolutions.MUN.Web.Helpers
{
    public class RestSharpHandler
    {
        public T RestRequest<T>(APISelector apiSelector,bool withAuth, string url, string methodType, List<NameValuePair> lstParameters, List<object> lstObjParameter = null) where T : new()
        {
            var restUrl = "";
            
            switch (apiSelector)
            {
                case APISelector.Municipality:
                    restUrl = Common.URLServicePath + url;
                    break;
                case APISelector.Security:
                    restUrl = Common.URLSecurityServicePath + url;
                    break;
                case APISelector.CustomerService:
                    restUrl = Common.URLCustomerServicePath + url;
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
                case "PUT":
                    request = new RestRequest("", Method.PUT);
                    break;
                default:
                    request = new RestRequest("", Method.GET);
                    break;
            }

            if (withAuth)
            {
                request.AddParameter("Authorization", Helpers.UserSession.Current.Token, ParameterType.HttpHeader);
                request.AddParameter("Company", Helpers.UserSession.Current.CompanyID, ParameterType.HttpHeader);
                request.AddParameter("Language", Helpers.UserSession.Current.Language, ParameterType.HttpHeader);
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

            request.Timeout = int.MaxValue;
            client.Timeout = int.MaxValue;
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
                throw new Exception(response.ErrorMessage);
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
    public enum APISelector
    {
        Security,
        CustomerService,
        Municipality
    }
}