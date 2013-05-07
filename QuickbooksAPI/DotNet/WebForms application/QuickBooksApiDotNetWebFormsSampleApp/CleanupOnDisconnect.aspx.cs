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

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    public partial class CleanupOnDisconnect : System.Web.UI.Page
    {
        /// <summary>
        /// This page is called as a result of disconnect from workplace if this page is specified as landing page Url for disconnect. 
        /// Please perform all the required clean on this page in such scenario. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //perform the clean up here 

            // Redirect to Default.aspx when user clicks on ConenctToIntuit widget.
            object value = Session["RedirectToDefault"];
            if (value != null)
            {
                bool isTrue = (bool)value;
                if (isTrue)
                {
                    Response.Redirect("default.aspx");
                }
            }
        }
    }
}