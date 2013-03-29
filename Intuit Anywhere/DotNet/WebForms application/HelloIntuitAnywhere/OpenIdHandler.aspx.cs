using System;
using System.Configuration;
using System.Globalization;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;


namespace HelloIntuitAnywhere
{
    /// <summary>
    /// This flow enables single sign on (SSO) between this app and the Intuit App Center.
    /// This feature offers two key benefits:  First, the user only has to sign in once, 
    /// instead of having to sign in to both this app and Intuit App Center.  
    /// Second, this app does not need to implement a customized solution for user authentication 
    /// because it relies on Intuit's OpenID service.
    /// the following occurs during the sign in process:
    /// 1.The user initiates the sign in process by going to your app and clicking the Sign in with Intuit button.
    /// 2.The Intuit sign in window appears, where the user enters the Intuit user ID (email) and password.
    /// 3.this app sends an authentication request to the Intuit OpenID service.
    /// 4.This page verifies the authentication response it receives from the Intuit OpenID service and stores
    /// user information inside the session object.
    /// </summary>
    public partial class OpenIdHandler : System.Web.UI.Page
    {
        /// <summary>
        /// Action Results for Index, uses DotNetOpenAuth for creating OpenId Request with Intuit
        /// and handling response recieved. 
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //OpenId Relying Party
            OpenIdRelyingParty openid = new OpenIdRelyingParty();

            var openIdIdentifier = ConfigurationManager.AppSettings["openid_identifier"];
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openIdIdentifier, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = openid.CreateRequest(openIdIdentifier);
                        FetchRequest fetch = new FetchRequest();
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email));
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.FullName));
                        request.AddExtension(fetch);
                        request.RedirectToProvider();
                    }
                    catch (ProtocolException ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                if (response.FriendlyIdentifierForDisplay == null)
                {
                    Response.Redirect("/OpenIdHandler.aspx");
                }

                // Stage 3: OpenID Provider sending assertion response
                Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;
                FetchResponse fetch = response.GetExtension<FetchResponse>();
                if (fetch != null)
                {
                    Session["OpenIdResponse"] = "True";
                    Session["FriendlyEmail"] = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                    Session["FriendlyName"] = fetch.GetAttributeValue(WellKnownAttributes.Name.FullName);
                }

                //Check if user disconnected from the App Center
                if (Request.QueryString["disconnect"] != null && Request.QueryString["disconnect"].ToString(CultureInfo.InvariantCulture) == "true")
                {
                    Session["Flag"] = true;
                    Response.Redirect("CleanupOnDisconnect.aspx");
                }
                else
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
    }
}