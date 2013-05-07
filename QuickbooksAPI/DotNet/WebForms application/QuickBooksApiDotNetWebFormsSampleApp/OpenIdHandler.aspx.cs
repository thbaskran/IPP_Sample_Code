/* 
//  Copyright Intuit, Inc 2013
//  DotNet AggCat DevKit Sample Application
//  This sample is for reference purposes only.

// Copyright (c) 2013 Intuit Inc. All rights reserved.

// Redistribution and use in source and binary forms, with or without modification, are permitted in conjunction 
// with Intuit Partner Platform. 
 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
// BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY, NON-INFRINGEMENT AND FITNESS FOR A 
// PARTICULAR PURPOSE ARE DISCLAIMED.  

// IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
// WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
// OF THE USE OF THIS SOFTWARE, WHETHER OR NOT SUCH DAMAGES WERE FORESEEABLE AND EVEN IF THE AUTHOR IS ADVISED 
// OF THE POSSIBILITY OF SUCH DAMAGES. 
*/

using System;
using System.Xml;
using System.Configuration;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;


namespace QuickBooksApiDotNetWebFormsSampleApp
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
        /// OpenId Relying Party
        /// </summary>
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        /// <summary>
        /// Action Results for Index, uses DotNetOpenAuth for creating OpenId Request with Intuit
        /// and handling response recieved. 
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var openid_identifier = ConfigurationManager.AppSettings["openid_identifier"];
            var returnUrl = "OpenIdHandler.aspx";
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openid_identifier, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = openid.CreateRequest(openid_identifier);
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

                    //get the OAuth Access token for the user from OauthAccessTokenStorage.xml
                    OauthAccessTokenStorageHelper.GetOauthAccessTokenForUser(Session["FriendlyEmail"].ToString(), Page);
                }

                string query = Request.Url.Query;
                if (!string.IsNullOrWhiteSpace(query) && query.ToLower().Contains("disconnect=true"))
                {
                    Session["accessToken"] = "dummyAccessToken";
                    Session["accessTokenSecret"] = "dummyAccessTokenSecret";
                    Session["Flag"] = true;
                    Response.Redirect("CleanupOnDisconnect.aspx");
                }

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    Response.Redirect("Default.aspx");
                }
            }
        }
    }
}