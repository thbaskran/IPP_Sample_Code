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
using System.Web;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    /// <summary>
    /// Invalidates the OAuth access token in the request, 
    /// thereby disconnecting the user from QuickBooks for this app. 
    /// Because accessing QuickBooks data requires a valid access token, 
    /// when the user is disconnected, your app cannot access the user's QuickBooks company data. 
    /// After disconnecting the user, your app should display the Connect to QuickBooks button.
    /// </summary>
    public partial class Disconnect : System.Web.UI.Page
    {
        /// <summary>
        /// Service Response
        /// </summary>
        private String txtServiceResponse = "";

        /// <summary>
        /// Disconnect Flag
        /// </summary>
        protected String DisconnectFlg = "";

        /// <summary>        
        /// Creates a HttpRequest with oAuthSession (OAuth Token) and gets the response with invalidating user
        /// from QuickBooks for this app
        /// For Authorization: The request header must include the OAuth parameters defined by OAuth Core 1.0 Revision A.
        /// 
        /// If the disconnect is successful, then the HTTP status code is 200 and 
        /// the XML response includes the <ErrorCode> element with a 0 value.  
        /// If an HTTP error is detected, then the HTTP status code is not 200.  
        /// If an HTTP error is not detected but the disconnect is unsuccessful, 
        /// then the HTTP status code is 200 and the response XML includes the <ErrorCode> element with a non-zero value.   
        /// For example,  if the OAuth access token expires or is invalid for some other reason, then the value of <ErrorCode> is 270.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            OAuthConsumerContext consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(),
                SignatureMethod = SignatureMethod.HmacSha1,
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString()
            };

            OAuthSession oSession = new OAuthSession(consumerContext, Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
                                  Constants.OauthEndPoints.AuthorizeUrl,
                                  Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken);

            oSession.ConsumerContext.UseHeaderForOAuthParameters = true;
            if ((Session["accessToken"] + "").Length > 0)
            {
                oSession.AccessToken = new TokenBase
                {
                    Token = HttpContext.Current.Session["accessToken"].ToString(),
                    ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(),
                    TokenSecret = HttpContext.Current.Session["accessTokenSecret"].ToString()
                };

                IConsumerRequest conReq = oSession.Request();
                conReq = conReq.Get();
                conReq = conReq.ForUrl(Constants.PlatformApiEndpoints.DisconnectUrl);
                try
                {
                    conReq = conReq.SignWithToken();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                //Used just see the what header contains
                string header = conReq.Context.GenerateOAuthParametersForHeader();

                //This method will clean up the OAuth Token
                txtServiceResponse = conReq.ReadBody();

                //Reset All the Session Variables
                HttpContext.Current.Session.Remove("oauthToken");

                // Add the invalid access token into session for the display of the Disconnect btn
                HttpContext.Current.Session["InvalidAccessToken"] = HttpContext.Current.Session["accessToken"];

                // Dont remove the access token since this is required for Reconnect btn in the Blue dot menu
                // HttpContext.Current.Session.Remove("accessToken");

                // Dont Remove flag since we need to display the blue dot menu for Reconnect btn in the Blue dot menu
                // HttpContext.Current.Session.Remove("Flag");
                DisconnectFlg = "User is Disconnected from QuickBooks!";
                //Remove the Oauth access token from the OauthAccessTokenStorage.xml
                OauthAccessTokenStorageHelper.RemoveInvalidOauthAccessToken(Session["FriendlyEmail"].ToString(), Page);
            }
        }
    }
}
