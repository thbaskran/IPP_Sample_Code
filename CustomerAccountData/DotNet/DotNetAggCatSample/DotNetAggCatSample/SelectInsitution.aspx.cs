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
using DotNetAggCatSample.Services;
using Intuit.Ipp.DataAggregation.AggregationCategorizationServices;
using Intuit.Ipp.DataAggregation.Core;
using Intuit.Ipp.DataAggregation.Data;
using Intuit.Ipp.DataAggregation.Exception;
using Intuit.Ipp.DataAggregation.Security;

namespace DotNetAggCatSample
{
    public partial class SelectInstitution : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    List<Institution> availableInstitutions = ((Institutions)AggCatService.AggCatInstitutions.GetInstitutions(new object[] { Server.MapPath("~/App_Data/FinancialInsitutions.encrypted"), Cache, HttpContext.Current.User.Identity.Name })).institution.ToList<Institution>();
                    foreach (Institution availableInstitution in availableInstitutions)
                    {
                        institutions.Items.Add(new ListItem() { Text = availableInstitution.institutionName, Value = availableInstitution.institutionId.ToString() });
                    }
                    ReorderAlphabetized(institutions);
                    institutions.Items.Insert(0, new ListItem("Please select your institution", "PleaseSelect"));
                }
                catch (Exception ex)
                {
                    Master.ErrorMessage = "Error occurred while calling GetInstitutions: " + ex.Message;
                }
            }
        }

        public static void ReorderAlphabetized(DropDownList ddl)
        {
            List<ListItem> listCopy = new List<ListItem>();
            foreach (ListItem item in ddl.Items)
                listCopy.Add(item);
            ddl.Items.Clear();
            foreach (ListItem item in listCopy.OrderBy(item => item.Text))
                ddl.Items.Add(item);
        }

        protected void institutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (institutions.SelectedValue == "PleaseSelect")
                {
                    InstitutionDetails.Visible = false;
                }
                else
                {
                    InstitutionDetails.Visible = true;
                    AggregationCategorizationService svc = Services.AggCatService.GetService(Cache, HttpContext.Current.User.Identity.Name);
                    InstitutionDetail insutitutionDetail = svc.GetInstitutionDetails(long.Parse(institutions.SelectedItem.Value));
                    InstitutionId.Text = insutitutionDetail.institutionId.ToString();
                    InsitutionName.Text = insutitutionDetail.institutionName;
                    Website.Text = insutitutionDetail.homeUrl;
                    PhoneNumer.Text = insutitutionDetail.phoneNumber;
                    Address.Text = AggCatService.AggCatInstitutions.GetFormattedInstitutionAddress(insutitutionDetail);
                    Email.Text = insutitutionDetail.emailAddress;
                }
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while calling GetInstitutionDetails: " + ex.Message;
            }


        }


        protected void FindAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("InstitutionLogin.aspx?InstitutionID=" + institutions.SelectedItem.Value);
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while redirecting to the institution's login page: " + ex.Message;
            }

        }

        protected void SelectCCBank_Click(object sender, EventArgs e)
        {
            try
            {
                institutions.ClearSelection();
                institutions.Items.FindByText("CC Bank").Selected = true;
                institutions_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while selecting CC Bank: " + ex.Message;
            }
        }

    }
}