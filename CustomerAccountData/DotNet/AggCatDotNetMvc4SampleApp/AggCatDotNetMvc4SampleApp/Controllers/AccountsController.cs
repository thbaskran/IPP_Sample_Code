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

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Intuit.Ipp.DataAggregation.Data;
using AggCatDotNetMvc4SampleApp.Mvc4.Models;
using AggCatDotNetMvc4SampleApp.MvcWeb.ServiceOperations;

namespace AggCatDotNetMvc4SampleApp.Mvc4.Controllers
{
    public class AccountsController : Controller
    {
        private ServiceOperations serviceOperations;
        private static List<Account> accounts;

        public AccountsController()
        {
            this.serviceOperations = new ServiceOperations();
        }

        public ActionResult Index()
        {
            AccountModel model = this.serviceOperations.GetCustomerAccounts();
            accounts = model.Accounts;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(AccountModel accountModel)
        {
            AccountModel model = this.serviceOperations.GetCustomerAccountTransactions(accountModel);
            if (model.Accounts == null)
            {
                model.Accounts = accounts;
            }

            return View(model);
        }

        public ActionResult AccInfo(string id)
        {
            if (accounts != null)
            {
                Account account = accounts.FirstOrDefault(a => a.accountNumber.Equals(id));
                if (account != null)
                {
                    return PartialView("AccInfo", account);
                    //return Content(new Intuit.Ipp.DataAggregation.Utility.XmlObjectSerializer().Serialize(account), "text/xml");
                }
            }

            return Content(string.Format("<?xml version=\"1.0\"?><error desc=\"No such account found.\" accid={0} />", id), "text/xml");
        }

        public ActionResult Delete(string id)
        {
            return View(this.serviceOperations.DeleteAccount(id));
        }
    }
}
