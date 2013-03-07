//  Copyright Intuit, Inc 2013
//  Agg Cat Sample Application
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

using System.Linq;
using Intuit.Ipp.DataAggregation.AggregationCategorizationServices;
using Intuit.Ipp.DataAggregation.Core;
using Intuit.Ipp.DataAggregation.Data;
using Intuit.Ipp.DataAggregation.Exception;
using Intuit.Ipp.DataAggregation.Security;
using AggCatDotNetMvc4SampleApp.Mvc4.Models;
using AggCatDotNetMvc4SampleApp.Mvc4;
using System;
using System.Collections.Generic;

namespace AggCatDotNetMvc4SampleApp.MvcWeb.ServiceOperations
{
    public class ServiceOperations
    {
        public InstitutionModel GetDefaultInstitutions()
        {
            InstitutionModel model = new InstitutionModel();

            //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
            SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                      AggCatAppSettings.ConsumerKey,
                                                                      AggCatAppSettings.ConsumerSecret,
                                                                      AggCatAppSettings.SamlIdentityProviderId,
                                                                      AggCatAppSettings.CustomerId);
            ServiceContext ctx = new ServiceContext(validator);
            AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
            try
            {
                List<Institution> institutions = svc.GetInstitutions().institution.ToList<Institution>();
                model.Institutions = institutions;
                model.Error = null;
                model.Success = true;
            }
            catch (AggregationCategorizationException ex)
            {
                model.Institutions = null;
                model.Error = ex.ToString();
                model.Success = false;
            }

            return model;
        }

        internal InstitutionModel GetInstitutionDetails(InstitutionModel institutionModel)
        {
            try
            {
                //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
                SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                          AggCatAppSettings.ConsumerKey,
                                                                          AggCatAppSettings.ConsumerSecret,
                                                                          AggCatAppSettings.SamlIdentityProviderId,
                                                                          AggCatAppSettings.CustomerId);
                ServiceContext ctx = new ServiceContext(validator);
                AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
                institutionModel.InstitutionDetail = svc.GetInstitutionDetails(institutionModel.InstitutionId);
                institutionModel.Success = true;
                institutionModel.Error = null;
            }
            catch (AggregationCategorizationException ex)
            {
                institutionModel.InstitutionDetail = null;
                institutionModel.Success = false;
                institutionModel.Error = ex.ToString();
            }

            return institutionModel;
        }

        internal AccountModel GetCustomerAccounts()
        {
            AccountModel model = new AccountModel();

            //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
            SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                           AggCatAppSettings.ConsumerKey,
                                                                           AggCatAppSettings.ConsumerSecret,
                                                                           AggCatAppSettings.SamlIdentityProviderId,
                                                                           AggCatAppSettings.CustomerId);
            ServiceContext ctx = new ServiceContext(validator);
            AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
            try
            {
                AccountList accountList = svc.GetCustomerAccounts();
                if (accountList != null && accountList.AnyIntuitObjects != null)
                {
                    model.Accounts = accountList.AnyIntuitObjects.ToList();
                }
                else
                {
                    accountList = null;
                }

                model.Error = null;
                model.Success = true;
            }
            catch (AggregationCategorizationException ex)
            {
                model.Accounts = null;
                model.Error = ex.ToString();
                model.Success = false;
            }

            return model;
        }

        internal AccountModel GetCustomerAccountTransactions(AccountModel accountModel)
        {
            try
            {
                //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
                SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                          AggCatAppSettings.ConsumerKey,
                                                                          AggCatAppSettings.ConsumerSecret,
                                                                          AggCatAppSettings.SamlIdentityProviderId,
                                                                          AggCatAppSettings.CustomerId);
                ServiceContext ctx = new ServiceContext(validator);
                AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
                accountModel.Transactions = svc.GetAccountTransactions(accountModel.AccountId, new DateTime(2011, 01, 01)).AnyIntuitObjects.ToList();
                accountModel.Success = true;
                accountModel.Error = null;
            }
            catch (AggregationCategorizationException ex)
            {
                accountModel.Transactions = null;
                accountModel.Success = false;
                accountModel.Error = ex.ToString();
            }

            return accountModel;
        }

        internal DiscoverAddModel DiscoverAndAdd(InstitutionModel institutionModel)
        {
            DiscoverAddModel discoverAddModel = new DiscoverAddModel();
            try
            {

                //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
                SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                          AggCatAppSettings.ConsumerKey,
                                                                          AggCatAppSettings.ConsumerSecret,
                                                                          AggCatAppSettings.SamlIdentityProviderId,
                                                                          AggCatAppSettings.CustomerId);
                ServiceContext ctx = new ServiceContext(validator);
                AggregationCategorizationService svc = new AggregationCategorizationService(ctx);

                InstitutionLogin instLogin = new InstitutionLogin();
                Credentials creds = new Credentials();
                List<Credential> credentials = new List<Credential>();
                Credential cred = new Credential();
                cred.name = institutionModel.InstitutionDetail.keys.FirstOrDefault(k => k.displayOrder == 1).name;
                cred.value = institutionModel.Value1;
                credentials.Add(cred);

                cred = new Credential();
                cred.name = institutionModel.InstitutionDetail.keys.FirstOrDefault(k => k.displayOrder == 2).name;
                cred.value = institutionModel.Value2;
                credentials.Add(cred);

                IEnumerable<InstitutionDetailKey> idk = institutionModel.InstitutionDetail.keys.Where(k => k.displayOrder != 1 && k.displayOrder != 2);
                foreach (InstitutionDetailKey item in idk)
                {
                    cred = new Credential();
                    cred.name = item.name;
                    cred.value = item.val;
                    credentials.Add(cred);
                }

                creds.credential = credentials.ToArray();
                instLogin.AnyIntuitObject = creds;

                Challenges challenges = null;
                ChallengeSession challengeSession = null;
                AccountList accountList = svc.DiscoverAndAddAccounts(institutionModel.InstitutionDetail.institutionId, instLogin, out challenges, out challengeSession);
                discoverAddModel.AccountList = accountList;
                discoverAddModel.Challenges = challenges;
                discoverAddModel.ChallengeSession = challengeSession;
                discoverAddModel.Success = true;
                discoverAddModel.Error = null;
                if (accountList == null && challenges != null)
                {
                    discoverAddModel.MFA = true;
                }
            }
            catch (AggregationCategorizationException ex)
            {
                discoverAddModel.AccountList = null;
                discoverAddModel.Challenges = null;
                discoverAddModel.ChallengeSession = null;
                discoverAddModel.Success = false;
                discoverAddModel.Error = ex.ToString();
            }

            return discoverAddModel;
        }

        internal DiscoverAddModel DiscoverAndAddResponse(DiscoverAddModel model)
        {
            try
            {

                //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
                SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                          AggCatAppSettings.ConsumerKey,
                                                                          AggCatAppSettings.ConsumerSecret,
                                                                          AggCatAppSettings.SamlIdentityProviderId,
                                                                          AggCatAppSettings.CustomerId);
                ServiceContext ctx = new ServiceContext(validator);
                AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
                ChallengeResponses challengeResponses = new ChallengeResponses();
                if (model.Challenges.challenge[0].AnyIntuitObjects.Count() == 2)
                {
                    if (model.UseSame)
                    {
                        challengeResponses.response = new string[] { model.Challenges.challenge[0].AnyIntuitObjects[1].ToString() };
                    }
                }
                else
                {
                    challengeResponses.response = new string[] { model.Answer };
                }

                AccountList accountList = svc.DiscoverAndAddAccountsResponse(challengeResponses, model.ChallengeSession);
                model.AccountList = accountList;
                model.Success = true;
                model.Error = null;
            }
            catch (AggregationCategorizationException ex)
            {
                model.AccountList = null;
                model.Success = false;
                model.Error = ex.ToString();
            }

            return model;
        }

        internal ErrorModel DeleteCustomer()
        {
            ErrorModel errorModel = new ErrorModel();

            //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
            SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                      AggCatAppSettings.ConsumerKey,
                                                                      AggCatAppSettings.ConsumerSecret,
                                                                      AggCatAppSettings.SamlIdentityProviderId,
                                                                      AggCatAppSettings.CustomerId);
            ServiceContext ctx = new ServiceContext(validator);
            AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
            try
            {
                svc.DeleteCustomer();
                errorModel.Error = null;
                errorModel.Success = true;
            }
            catch (AggregationCategorizationException ex)
            {
                errorModel.Error = ex.ToString();
                errorModel.Success = false;
            }

            return errorModel;
        }

        internal ErrorModel DeleteAccount(string id)
        {
            ErrorModel errorModel = new ErrorModel();

            //Demo purposes only.  The OAuth tokens returned by the SAML assertion are valid for 1 hour and do not need to be requested before each API call.
            SamlRequestValidator validator = new SamlRequestValidator(AggCatAppSettings.Certificate,
                                                                      AggCatAppSettings.ConsumerKey,
                                                                      AggCatAppSettings.ConsumerSecret,
                                                                      AggCatAppSettings.SamlIdentityProviderId,
                                                                      AggCatAppSettings.CustomerId);
            ServiceContext ctx = new ServiceContext(validator);
            AggregationCategorizationService svc = new AggregationCategorizationService(ctx);
            try
            {
                long longId = Convert.ToInt64(id);
                svc.DeleteAccount(longId);
                errorModel.Error = null;
                errorModel.Success = true;
            }
            catch (AggregationCategorizationException ex)
            {
                errorModel.Error = ex.ToString();
                errorModel.Success = false;
            }

            return errorModel;
        }
    }
}