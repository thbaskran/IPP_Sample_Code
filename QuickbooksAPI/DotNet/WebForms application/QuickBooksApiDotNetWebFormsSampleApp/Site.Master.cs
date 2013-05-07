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
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get tokens from the AppSettings in config files
            string applicationToken = ConfigurationManager.AppSettings["applicationToken"];
            string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];
            // Check whether the keys are null or empty.
            if (string.IsNullOrWhiteSpace(applicationToken) || string.IsNullOrWhiteSpace(consumerKey) || string.IsNullOrWhiteSpace(consumerSecret))
            {
                // Show Error message
                this.errorDiv.Visible = true;
                this.mainContetntDiv.Visible = false;
            }
            else
            {
                // Show main content
                this.errorDiv.Visible = false;
                this.mainContetntDiv.Visible = true;
            }

            if (Session["FriendlyName"] != null)
            {
                this.friendlyName.Text = Session["FriendlyName"].ToString();
            }

            // Read value from session and check flag which decides the display of blue dot menu
            object flag = Session["Flag"];
            if (flag != null)
            {
                bool flagValue = Convert.ToBoolean(flag.ToString());
                if (flagValue)
                {
                    // Show BlueDot widget
                    this.blueDotDiv.Visible = true;
                    this.logoutview.Style.Add(HtmlTextWriterStyle.MarginRight, "125px");
                }
                else
                {
                    // Disable BlueDot widget
                    this.blueDotDiv.Visible = false;
                    this.logoutview.Style.Add(HtmlTextWriterStyle.MarginRight, "0px");
                }
            }
            else
            {
                // Disable BlueDot widget
                this.blueDotDiv.Visible = false;
                this.logoutview.Style.Add(HtmlTextWriterStyle.MarginRight, "0px");
            }
        }
    }
}
