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
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Intuit.Ipp.DataAggregation.AggregationCategorizationServices;
using Intuit.Ipp.DataAggregation.Core;
using Intuit.Ipp.DataAggregation.Data;

namespace AggCatDotNetWebFormsSampleApp
{
    public partial class Institution_Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            try
            {
                if (Request.QueryString["InstitutionID"] == null) { Response.Redirect("SelectInstitution.aspx"); }
                AggregationCategorizationService svc = Services.AggCatService.GetService(Cache, HttpContext.Current.User.Identity.Name);
                InstitutionDetail institutionDetail = svc.GetInstitutionDetails(long.Parse(Request.QueryString["InstitutionID"]));
                Control[][] institutionLoginControls = new Control[institutionDetail.keys.Length][];
                foreach (InstitutionDetailKey institutionKey in institutionDetail.keys)
                {
                    TextBox value = new TextBox();
                    if (institutionKey.mask) { value.TextMode = TextBoxMode.Password; }
                    value.Attributes.Add("KeyName", institutionKey.name);
                    value.Width = 200;

                    Label name = new Label { Text = institutionKey.name + ":   ", AssociatedControlID = value.ID };

                    Literal lineBreak = new Literal { Text = "<br/><br/>" };

                    institutionLoginControls[institutionKey.displayOrder - 1] = new Control[] { name, value, lineBreak };
                }
                for (int i = 0; i < institutionLoginControls.Length; i++)
                {
                    LoginControls.Controls.Add(institutionLoginControls[i][0]);
                    LoginControls.Controls.Add(institutionLoginControls[i][1]);
                    LoginControls.Controls.Add(institutionLoginControls[i][2]);
                }
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while calling GetInsitutionDetails: " + ex.Message;
            }
            //}
        }

        protected string StringToAlphanumeric(string stringToConvert)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            return rgx.Replace(stringToConvert, "");
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["InstitutionID"] == null) { Response.Redirect("SelectInstitution.aspx"); }
                AggregationCategorizationService svc = Services.AggCatService.GetService(Cache, HttpContext.Current.User.Identity.Name);
                InstitutionDetail insutitutionDetail = svc.GetInstitutionDetails(long.Parse(Request.QueryString["InstitutionID"]));
                InstitutionLogin instLogin = new InstitutionLogin();
                Credentials creds = new Credentials();
                List<Credential> credentials = new List<Credential>();
                foreach (Control control in LoginControls.Controls)
                {
                    if (control as TextBox != null)
                    {
                        Credential cred = new Credential();
                        TextBox loginValue = (TextBox)control;
                        cred.name = loginValue.Attributes["KeyName"];
                        cred.value = loginValue.Text;
                        credentials.Add(cred);
                    }
                }
                creds.credential = credentials.ToArray();
                instLogin.AnyIntuitObject = creds;

                Challenges challenges = null;
                ChallengeSession challengeSession = null;

                AccountList accountList = svc.DiscoverAndAddAccounts(long.Parse(Request.QueryString["InstitutionID"].ToString()), instLogin, out challenges, out challengeSession);
                Response.Redirect("MyAccounts.aspx?Success");
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while calling logging into the instituion: " + ex.Message;
            }
        }
    }
}