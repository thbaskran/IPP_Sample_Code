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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Caching;
using Intuit.Ipp.DataAggregation.AggregationCategorizationServices;
using Intuit.Ipp.DataAggregation.Core;
using Intuit.Ipp.DataAggregation.Data;
using Intuit.Ipp.DataAggregation.Security;

namespace DotNetAggCatSample.Services
{
    public static class AggCatService
    {
        public static AggregationCategorizationService GetService(Cache cache, String userId)
        {

            try
            {
                if (cache["AggCatService_" + userId] == null)
                {
                    string certificateFile = System.Configuration.ConfigurationManager.AppSettings["PrivateKeyPath"];
                    string password = System.Configuration.ConfigurationManager.AppSettings["PrivateKeyPassword"];
                    X509Certificate2 certificate = new X509Certificate2(certificateFile, password);

                    string consumerKey = System.Configuration.ConfigurationManager.AppSettings["ConsumerKey"];
                    string consumerSecret = System.Configuration.ConfigurationManager.AppSettings["ConsumerSecret"];
                    string issuerId = System.Configuration.ConfigurationManager.AppSettings["SAMLIdentityProviderID"];

                    SamlRequestValidator samlValidator = new SamlRequestValidator(certificate, consumerKey, consumerSecret, issuerId, userId);

                    ServiceContext ctx = new ServiceContext(samlValidator);
                    cache.Add("AggCatService_" + userId, new AggregationCategorizationService(ctx), null, DateTime.Now.AddMinutes(50),
                              Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
                return (AggregationCategorizationService)cache["AggCatService_" + userId];
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create AggCatService: " + ex.Message);
            }
        }


        public static partial class AggCatInstitutions
        {
            delegate Institutions GetInstitutionsAsyncDelegate(params object[] args);

            private static bool _retrievingInsitutions = false;
            public static bool InstitutionsRetrieved = false;

            public static void GetInstitutionsAsync(String cacheFilePath, Cache cache, String userId)
            {
                GetInstitutionsAsyncDelegate del = new GetInstitutionsAsyncDelegate(GetInstitutions);
                del.BeginInvoke(new object[] { cacheFilePath, cache, userId }, null, null);
            }

            public static Institutions GetInstitutions(object[] args)
            {
                try
                {
                    while (_retrievingInsitutions) { System.Threading.Thread.Sleep(500); }
                    _retrievingInsitutions = true;
                    String cacheFilePath = args[0].ToString();
                    Cache cache = (Cache)args[1];
                    String userId = (String)args[2];
                    if (cache["Insitutions"] == null)
                    {
                        string encryptionKey = System.Configuration.ConfigurationManager.AppSettings["FileEncryptionKey"];
                        Institutions financialInstitutions = null;
                        System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(Institutions));
                        if (System.IO.File.Exists(cacheFilePath) && (new FileInfo(cacheFilePath)).Length > 0)
                        {
                            try
                            {
                                String financialInstitutionList = Encryption.Decrypt(System.IO.File.ReadAllText(cacheFilePath), encryptionKey);
                                StringReader text = new StringReader(financialInstitutionList);
                                System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(text);
                                financialInstitutions = (Institutions)serializer.Deserialize(xmlReader);
                            }
                            catch
                            {
                            }
                        }
                        if (financialInstitutions == null)
                        {
                            financialInstitutions = Services.AggCatService.GetService(cache, userId).GetInstitutions();
                            try
                            {
                                StringBuilder financialInstitutionList = new StringBuilder();
                                System.Xml.XmlWriter xmlWriter = System.Xml.XmlWriter.Create(financialInstitutionList);
                                serializer.Serialize(xmlWriter, financialInstitutions);
                                xmlWriter.Flush();
                                xmlWriter.Close();
                                System.IO.File.WriteAllText(cacheFilePath, Encryption.Encrypt(financialInstitutionList.ToString(), encryptionKey));
                            }
                            catch
                            {
                            }
                        }

                        cache.Add("Insitutions", financialInstitutions,
                                  null,
                                  Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, null);

                        InstitutionsRetrieved = true;
                    }
                    return (Institutions)cache["Insitutions"];

                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    _retrievingInsitutions = false;
                }
            }

            public static string GetFormattedInstitutionAddress(InstitutionDetail institutionDetail)
            {
                try
                {
                    string formattedAddress = "<br />";
                    if (institutionDetail.address.address1.IsNotNullOrEmpty()) { formattedAddress += institutionDetail.address.address1 + "<br />"; }
                    if (institutionDetail.address.address2.IsNotNullOrEmpty()) { formattedAddress += institutionDetail.address.address2 + "<br />"; }
                    if (institutionDetail.address.address3.IsNotNullOrEmpty()) { formattedAddress += institutionDetail.address.address3 + "<br />"; }
                    if (institutionDetail.address.city.IsNotNullOrEmpty()) { formattedAddress += institutionDetail.address.city + ", "; }
                    if (institutionDetail.address.state.IsNotNullOrEmpty()) { formattedAddress += institutionDetail.address.state + " "; }
                    formattedAddress += institutionDetail.address.postalCode;
                    if (institutionDetail.address.country.IsNotNullOrEmpty())
                    {
                        if (formattedAddress != "<br />") { formattedAddress += "<br />"; }
                        formattedAddress += institutionDetail.address.country;
                    }
                    return formattedAddress;
                }
                catch (Exception)
                {
                    return "<br />Unable to format address";
                }
            }
        }
    }
}