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
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using Intuit.Ipp.Core;
using Intuit.Ipp.Security;
using Intuit.Ipp.Services;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    /// <summary>
    /// Controller which connects to QuickBooks and pulls customer Info.
    /// This flow will make use of Data Service SDK V2 to create OAuthRequest and connect to 
    /// Customer Data under the service context and display data inside the grid.
    /// </summary>
    public partial class QuickBooksCustomers : System.Web.UI.Page
    {
        /// <summary>
        /// RealmId, AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret, DataSourceType
        /// </summary>
        private String realmId, accessToken, accessTokenSecret, consumerKey, consumerSecret;
        private IntuitServicesType dataSourcetype;

        /// <summary>
        /// Page Load Event, pulls Customer data from QuickBooks using SDK and Binds it to Grid
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">Event Args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session.Keys.Count > 0)
            {
                realmId = HttpContext.Current.Session["realm"].ToString();
                accessToken = HttpContext.Current.Session["accessToken"].ToString();
                accessTokenSecret = HttpContext.Current.Session["accessTokenSecret"].ToString();
                consumerKey = ConfigurationManager.AppSettings["consumerKey"].ToString(CultureInfo.InvariantCulture);
                consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
                dataSourcetype = HttpContext.Current.Session["dataSource"].ToString().ToLower() == "qbd" ? IntuitServicesType.QBD : IntuitServicesType.QBO;

                OAuthRequestValidator oauthValidator =  new OAuthRequestValidator(accessToken, accessTokenSecret, consumerKey, consumerSecret);
                ServiceContext context = new ServiceContext(oauthValidator, realmId, dataSourcetype);
                DataServices commonService = new DataServices(context);

                try
                {
                    switch(dataSourcetype)
                    {
                        case IntuitServicesType.QBD:
                            var qbdCustomerQuery = new Intuit.Ipp.Data.Qbd.CustomerQuery();
                            qbdCustomerQuery.ItemElementName = Intuit.Ipp.Data.Qbd.ItemChoiceType4.StartPage;
                            qbdCustomerQuery.Item = "1";
                            qbdCustomerQuery.ChunkSize = "10";
                            var qbdCustomers = qbdCustomerQuery.ExecuteQuery<Intuit.Ipp.Data.Qbd.Customer>(context).ToList();
                            grdQuickBooksCustomers.DataSource = qbdCustomers;
                            break;
                        case IntuitServicesType.QBO:
                            var qboCustomer = new Intuit.Ipp.Data.Qbo.Customer();
                            var qboCustomers = commonService.FindAll(qboCustomer, 1, 10).ToList();
                            grdQuickBooksCustomers.DataSource = qboCustomers;
                            break;
                    }

                    grdQuickBooksCustomers.DataBind();

                    if (grdQuickBooksCustomers.Rows.Count > 0)
                    {
                        GridLocation.Visible = true;
                        MessageLocation.Visible = false;
                    }
                    else
                    {
                        GridLocation.Visible = false;
                        MessageLocation.Visible = true;
                    }
                }
                catch (Intuit.Ipp.Exception.InvalidTokenException)
                {
                    //Remove the Oauth access token from the OauthAccessTokenStorage.xml
                    OauthAccessTokenStorageHelper.RemoveInvalidOauthAccessToken(Session["FriendlyEmail"].ToString(), Page);

                    Session["show"] = true;
                    Response.Redirect("~/Default.aspx");
                }
            }
        }
    }
}