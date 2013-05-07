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
using System.Web;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["show"] != null)
            {
                bool value = Convert.ToBoolean(Session["show"]);
                if (value)
                {
                    Session.Remove("show");

                    //show a message to the user that token is invalid
                    string message = "<SCRIPT LANGUAGE='JavaScript'>alert('This application is no longer authorized to accesss your QuickBooks data. Please reauthorize to enable this functionality.')</SCRIPT>";

                    // show user the Connect to QuickBooks page again
                    Response.Write(message);
                }
            }

            #region OpenId

            // Hide Connect to QuickBooks widget and show Sign in widget
            IntuitInfo.Visible = false;
            IntuitSignin.Visible = true;
            this.Master.FindControl("logoutview").Visible = false;
            // If Session has keys
            if (HttpContext.Current.Session.Keys.Count > 0)
            {
                // If there is a key OpenIdResponse
                if (HttpContext.Current.Session["OpenIdResponse"] != null)
                {
                    // Show the Sign in widget and disable the Connect to QuickBooks widget
                    IntuitSignin.Visible = false;
                    IntuitInfo.Visible = true;
                    this.Master.FindControl("logoutview").Visible = true;
                }

                // Sow information of the user if the keys are in the session
                if (Session["FriendlyIdentifier"] != null)
                {
                    friendlyIdentifier.Text = Session["friendlyIdentifier"].ToString();
                }
                if (Session["FriendlyName"] != null)
                {
                    friendlyName.Text = Session["FriendlyName"].ToString();
                }
                else
                {
                    friendlyName.Text = "User Didnt Login Via OpenID, look them up in your system";
                }

                if (Session["FriendlyEmail"] != null)
                {
                    friendlyEmail.Text = Session["FriendlyEmail"].ToString();
                }
                else
                {
                    friendlyEmail.Text = "User Didnt Login Via OpenID, look them up in your system";
                }
            }
            #endregion

            #region oAuth

            // If session has accesstoken and InvalidAccessToken is null
            if (HttpContext.Current.Session["accessToken"] != null && HttpContext.Current.Session["InvalidAccessToken"] == null)
            {
                // Show oAuthinfo(contains Get Customers QuickBooks List) and disable Connect to QuickBooks widget
                oAuthinfo.Visible = true;
                connectToIntuitDiv.Visible = false;
            }

            #endregion
        }
    }

}
