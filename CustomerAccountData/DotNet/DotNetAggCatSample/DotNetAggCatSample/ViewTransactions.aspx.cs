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
using Intuit.Ipp.DataAggregation.Data;

namespace DotNetAggCatSample
{
    public partial class ViewTransactions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    StartDate.Text = DateTime.Now.AddDays(-15).ToShortDateString();
                    EndDate.Text = DateTime.Now.ToShortDateString();
                    ShowTransactions(StartDate.Text, EndDate.Text);
                }
                catch (Exception ex)
                {
                    Master.ErrorMessage = "Error occurred while calling GetAccountTransactions: " + ex.Message;
                }
            }
        }

        protected void ShowTransactions(String startDate, String endDate)
        {
            try
            {
                AggregationCategorizationService svc = Services.AggCatService.GetService(Cache, HttpContext.Current.User.Identity.Name);
                TransactionList transactionList = svc.GetAccountTransactions(long.Parse(Request.QueryString["Account"]),
                                                                             Convert.ToDateTime(startDate ), Convert.ToDateTime(endDate));
                AccountsGridView.DataSource = transactionList.AnyIntuitObjects;
                AccountsGridView.DataBind();
            }
            catch (Exception ex)
            {
                Master.ErrorMessage = "Error occurred while calling GetAccountTransactions: " + ex.Message;
            }
        }

        protected void RefreshTransactions_Click(object sender, EventArgs e)
        {
            ShowTransactions(StartDate.Text, EndDate.Text);
        }

        protected void AccountsGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                ShowTransactions(StartDate.Text, EndDate.Text);
                AccountsGridView.PageIndex = e.NewPageIndex;
                AccountsGridView.DataBind();
            }
            catch (Exception ex)
            {

                Master.ErrorMessage = "Error occurred while retrieving transactions: " + ex.Message;
            }
        }

    }
}