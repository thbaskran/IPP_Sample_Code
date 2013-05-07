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
using System.Configuration;
using System.Globalization;
using System.Web;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    /// <summary>
    /// This flow is invoked when the the user selects a QuickBooks company and then clicks Authorize to 
    /// grant this app access to that company's data. 
    /// Behind the scenes, this app exchanges tokens with the Intuit OAuth service and then 
    /// stores the authorized access token in a session store. (use persistent store such as a database in real time scenarios.)  
    /// A valid access token indicates that the user has connected your app to a specific company.  
    /// (Connections are important because Intuit charges you according to how many active connections 
    /// users have made with your app.)  Later, when your app calls Data Services for QuickBooks, 
    /// it fetches the access token from the persistent store and includes the token in the 
    /// HTTP request header.  
    /// </summary>
    public partial class OauthHandler : System.Web.UI.Page
    {
        /// <summary>
        /// OAuthVerifyer, RealmId, DataSource
        /// </summary>
        private String _oauthVerifyer, _realmid, _dataSource;

        /// <summary>
        /// Action Results for Index, OAuthToken, OAuthVerifyer and RealmID is recieved as part of Response
        /// and are stored inside Session object for future references
        /// NOTE: Session storage is only used for demonstration purpose only.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.HasKeys())
            {
                // This value is used to Get Access Token.
                _oauthVerifyer = Request.QueryString["oauth_verifier"].ToString();

                _realmid = Request.QueryString["realmId"].ToString();
                HttpContext.Current.Session["realm"] = _realmid;

                //If dataSource is QBO call QuickBooks Online Services, else call QuickBooks Desktop Services
                _dataSource = Request.QueryString["dataSource"].ToString();
                HttpContext.Current.Session["dataSource"] = _dataSource;

                getAccessToken();

                //Production applications should securely store the Access Token.
                //In this template, encrypted Oauth access token is persisted in OauthAccessTokenStorage.xml
                OauthAccessTokenStorageHelper.StoreOauthAccessToken(Page);

                // This value is used to redirect to Default.aspx from Cleanup page when user clicks on ConnectToInuit widget.
                Session["RedirectToDefault"] = true;
            }
            else
            {
                Response.Write("No OAuth token was received");
            }
        }

        /// <summary>
        /// Gets the Access Token
        /// </summary>
        private void getAccessToken()
        {
            IOAuthSession clientSession = CreateSession();
            try
            {
                IToken accessToken = clientSession.ExchangeRequestTokenForAccessToken((IToken)Session["requestToken"], _oauthVerifyer);
                Session["accessToken"] = accessToken.Token;

                // Add flag to session which tells that accessToken is in session
                Session["Flag"] = true;

                // Remove the Invalid Access token since we got the new access token
                HttpContext.Current.Session.Remove("InvalidAccessToken");
                Session["accessTokenSecret"] = accessToken.TokenSecret;
            }
            catch (Exception ex)
            {
                //Handle Exception if token is rejected or exchange of Request Token for Access Token failed. 
                throw ex;
            }
        }

        /// <summary>
        /// Creates the OAuth Session using Consumer key
        /// </summary>        
        /// <returns>OAuth Session.</returns>
        private IOAuthSession CreateSession()
        {
            OAuthConsumerContext consumerContext = new OAuthConsumerContext
            {
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(CultureInfo.InvariantCulture),
                ConsumerSecret = ConfigurationManager.AppSettings["consumerSecret"].ToString(CultureInfo.InvariantCulture),
                SignatureMethod = SignatureMethod.HmacSha1
            };

            return new OAuthSession(consumerContext,
                                            Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlRequestToken,
                                            Constants.OauthEndPoints.IdFedOAuthBaseUrl,
                                             Constants.OauthEndPoints.IdFedOAuthBaseUrl + Constants.OauthEndPoints.UrlAccessToken);
        }
    }
}