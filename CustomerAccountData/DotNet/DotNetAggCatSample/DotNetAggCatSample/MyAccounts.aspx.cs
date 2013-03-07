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
using Intuit.Ipp.DataAggregation.AggregationCategorizationServices;
using Intuit.Ipp.DataAggregation.Core;
using Intuit.Ipp.DataAggregation.Data;
using Intuit.Ipp.DataAggregation.Exception;
using Intuit.Ipp.DataAggregation.Security;

namespace DotNetAggCatSample
{
    public partial class MyAccounts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    FillAccountsList();
                }
                catch (Exception ex)
                {
                    Master.ErrorMessage = "Error occurred while calling GetCustomerAccounts: " + ex.Message;
                }
            }

        }

        protected void FillAccountsList()
        {
            AggregationCategorizationService svc = Services.AggCatService.GetService(Cache, HttpContext.Current.User.Identity.Name);
            AccountList accounts = svc.GetCustomerAccounts();
            AccountsGridView.DataSource = accounts.AnyIntuitObjects;
            AccountsGridView.DataBind();
        }

        protected void AccountsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int row = Convert.ToInt32(e.CommandArgument);
                long accountId = long.Parse(AccountsGridView.DataKeys[row].Value.ToString());
                if (e.CommandName == "ViewTransactions")
                {
                    Response.Redirect("ViewTransactions.aspx?Account=" + accountId);
                }
                if (e.CommandName == "DeleteAccount")
                {
                    AggregationCategorizationService svc = Services.AggCatService.GetService(Cache, HttpContext.Current.User.Identity.Name);
                    svc.DeleteAccount(accountId);
                    FillAccountsList();
                }
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while calling " + e.CommandName + ": " + ex.Message;
            }

        }

        protected void AddAccounts_Click(object sender, EventArgs e)
        {
            Response.Redirect("SelectInstitution.aspx");
        }
    }
}