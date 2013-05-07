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
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    /// <summary>
    /// Direct Connect to Intuit Page.
    /// </summary>
    public partial class DirectConnectToIntuit : System.Web.UI.Page
    {
        /// <summary>
        /// OpenId Relying Party.
        /// </summary>
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        protected void Page_Load(object sender, EventArgs e)
        {
            var openid_identifier = ConfigurationManager.AppSettings["openid_identifier"];
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openid_identifier, out id))
                {
                    try
                    {
                        IAuthenticationRequest request = openid.CreateRequest(openid_identifier);
                        FetchRequest fetch = new FetchRequest();
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Contact.Email));
                        fetch.Attributes.Add(new AttributeRequest(WellKnownAttributes.Name.FullName));
                        request.AddExtension(fetch);
                        request.RedirectToProvider();
                    }
                    catch (ProtocolException ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                if (response.FriendlyIdentifierForDisplay == null)
                {
                    Response.Redirect("/DirectConnectToIntuit.aspx");
                }

                // Stage 3: OpenID Provider sending assertion response, storing the response in Session object is only for demonstration purpose
                Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;
                FetchResponse fetch = response.GetExtension<FetchResponse>();
                if (fetch != null)
                {
                    Session["OpenIdResponse"] = "True";
                    Session["FriendlyEmail"] = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);// emailAddresses.Count > 0 ? emailAddresses[0] : null;
                    Session["FriendlyName"] = fetch.GetAttributeValue(WellKnownAttributes.Name.FullName);//fullNames.Count > 0 ? fullNames[0] : null;
                }
            }
        }
    }
}