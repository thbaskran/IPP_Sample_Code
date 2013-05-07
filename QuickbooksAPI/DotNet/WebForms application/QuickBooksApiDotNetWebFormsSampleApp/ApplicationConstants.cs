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

namespace QuickBooksApiDotNetWebFormsSampleApp
{
    using System.Configuration;

    /// <summary>
    /// Contains Constants.
    /// </summary>
    internal class Constants
    {
        /// <summary>
        /// OAuth EndPoints.
        /// </summary>
        internal class OauthEndPoints
        {
            /// <summary>
            /// Url Request Token
            /// </summary>
            internal static string UrlRequestToken = ConfigurationManager.AppSettings["Url_Request_Token"] != null ?
                ConfigurationManager.AppSettings["Url_Request_Token"].ToString() : "/get_request_token";

            /// <summary>
            /// Url Access Token
            /// </summary>
            internal static string UrlAccessToken = ConfigurationManager.AppSettings["Url_Access_Token"] != null ?
                ConfigurationManager.AppSettings["Url_Access_Token"].ToString() : "/get_access_token";

            /// <summary>
            /// Federation base url.
            /// </summary>
            internal static string IdFedOAuthBaseUrl = ConfigurationManager.AppSettings["Intuit_OAuth_BaseUrl"] != null ?
                ConfigurationManager.AppSettings["Intuit_OAuth_BaseUrl"].ToString() : "https://oauth.intuit.com/oauth/v1";

            /// <summary>
            /// Authorize url.
            /// </summary>
            internal static string AuthorizeUrl = ConfigurationManager.AppSettings["Intuit_Workplace_AuthorizeUrl"] != null ?
                ConfigurationManager.AppSettings["Intuit_Workplace_AuthorizeUrl"].ToString() : "https://workplace.intuit.com/Connect/Begin";
        }

        /// <summary>
        /// PlatformApiEndpoints
        /// </summary>
        internal class PlatformApiEndpoints
        {
            /// <summary>
            /// BlueDot Menu Url.
            /// </summary>
            internal static string BlueDotAppMenuUrl = ConfigurationManager.AppSettings["BlueDot_AppMenuUrl"] != null ?
                ConfigurationManager.AppSettings["BlueDot_AppMenuUrl"].ToString() : "https://workplace.intuit.com/api/v1/Account/AppMenu";

            /// <summary>
            /// Disconnect url.
            /// </summary>
            internal static string DisconnectUrl = ConfigurationManager.AppSettings["DisconnectUrl"] != null ?
                ConfigurationManager.AppSettings["DisconnectUrl"].ToString() : "https://appcenter.intuit.com/api/v1/Connection/Disconnect";
        }
    }
}