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
using System.IO;
using System.Net;
using System.Web;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    /// <summary>
    /// Controller for Blue Dot Menu, Returns the HTML for the Intuit "blue dot" menu, 
    /// which shows the QuickBooks API apps available to the user.   
    /// </summary>
    public partial class MenuProxy : System.Web.UI.Page
    {
        /// <summary>
        /// Service Response.
        /// </summary>
        private String txtServiceResponse = "";

        /// <summary>
        /// On Page Event, The request header must include the OAuth parameters defined by OAuth Core 1.0 Revision A.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            GetBlueDotMenu();
        }

        /// <summary>
        /// Core Logic to get HTML content for BLue Dot Menu
        /// Error Handling: If the OAuth access token has expired or is invalid for some other reason, 
        /// then the HTTP status code is 200, and the HTML returned shows the Connect to QuickBooks button within the Intuit "blue dot" menu.  
        /// If an internal error is detected, then the HTTP status code returned is not 2xx, and the HTML returned will display the following text in the menu: "We are sorry, but we cannot load the menu right now."
        /// </summary>
        protected void GetBlueDotMenu()
        {
            HttpContext.Current.Session["serviceEndPoint"] = Constants.PlatformApiEndpoints.BlueDotAppMenuUrl;
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

            oSession.AccessToken = new TokenBase
            {
                Token = Session["accessToken"].ToString(),
                ConsumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(),
                TokenSecret = Session["accessTokenSecret"].ToString()
            };

            IConsumerRequest conReq = oSession.Request();
            conReq = conReq.Get();
            conReq = conReq.ForUrl(HttpContext.Current.Session["serviceEndPoint"].ToString());
            try
            {
                conReq = conReq.SignWithToken();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            string header = conReq.Context.GenerateOAuthParametersForHeader();
            try
            {
                txtServiceResponse = conReq.ReadBody();
                Response.Write(txtServiceResponse);
            }
            catch (WebException we)
            {
                HttpWebResponse rsp = (HttpWebResponse)we.Response;
                if (rsp != null)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(rsp.GetResponseStream()))
                        {
                            txtServiceResponse = txtServiceResponse + rsp.StatusCode + " | " + reader.ReadToEnd();
                        }
                    }
                    catch (Exception)
                    {
                        txtServiceResponse = txtServiceResponse + "Status code: " + rsp.StatusCode;
                    }
                }
                else
                {
                    txtServiceResponse = txtServiceResponse + "Error Communicating with App Menu Platform API" + we.Message;
                }
            }
        }
    }
}