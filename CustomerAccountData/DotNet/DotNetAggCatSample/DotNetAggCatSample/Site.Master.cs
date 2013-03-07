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

namespace DotNetAggCatSample
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public string _errorMessage = string.Empty;
        public String ErrorMessage
        {
            set { _errorMessage = value; ErrorContainer.Visible = !String.IsNullOrEmpty(_errorMessage); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AggCatService.AggCatInstitutions.InstitutionsRetrieved && HttpContext.Current.User.Identity.Name.IsNotNullOrEmpty()) { AggCatService.AggCatInstitutions.GetInstitutionsAsync(Server.MapPath("~/App_Data/FinancialInsitutions.encrypted"), Cache, HttpContext.Current.User.Identity.Name); }
            NavigationMenu.FindItem("Your Accounts").Enabled = HttpContext.Current.User.Identity.Name.IsNotNullOrEmpty();
            ErrorMessage = String.Empty;
        }
    }
}
