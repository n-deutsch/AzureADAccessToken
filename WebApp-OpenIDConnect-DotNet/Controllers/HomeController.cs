using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.IO;
using System.Text;

namespace WebApp_OpenIDConnect_DotNet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string AccessToken = getToken();
            ViewBag.Message = AccessToken;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Gains an access token using credentials stored in Web.config
        /// and a password retrieved from Azure AD portal
        /// </summary>
        public static string getToken()
        {
            //Resource ID is the application we want a token for. In this case, it's the same as client
            string ResourceId = ConfigurationManager.AppSettings["ida:ClientId"];
            string AuthInstance = "https://login.microsoftonline.com/{0}/";

            //tenant is the domain the app is registered in. "natedeutschgmail.onmicrosoft.com"
            string TenantId = ConfigurationManager.AppSettings["ida:Tenant"]; // Tenant or directory ID

            //ClientID is the application ID in the azure resource portal
            string ClientID = ConfigurationManager.AppSettings["ida:ClientId"];
            string RedirectURI = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

            //key generated from Azure portal
            string AccessKey = "[access key goes here]";

            // Construct the authority string from the Azure AD OAuth endpoint and the tenant ID. 
            string authority = string.Format(CultureInfo.InvariantCulture, AuthInstance, TenantId);
            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext authContext =
                new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authority);

            ClientCredential Client = new ClientCredential(ClientID, AccessKey);

            // Acquire an access token from Azure AD. 
            //AuthenticationResult result = authContext.AcquireTokenAsync(ResourceId,ClientID, new Uri(RedirectURI), new PlatformParameters(PromptBehavior.Auto)).Result;
            AuthenticationResult result = authContext.AcquireTokenAsync(ResourceId, Client).Result;

            return result.AccessToken;
        }
    }
}