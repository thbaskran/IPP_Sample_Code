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

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Intuit.Ipp.DataAggregation.Data;
using AggCatDotNetMvc4SampleApp.Mvc4.Models;
using AggCatDotNetMvc4SampleApp.MvcWeb.ServiceOperations;

namespace AggCatDotNetMvc4SampleApp.Mvc4.Controllers
{
    public class InstitutionsController : Controller
    {
        private ServiceOperations serviceOperations;

        public InstitutionsController()
        {
            this.serviceOperations = new ServiceOperations();
        }

        public ActionResult Index()
        {
            InstitutionModel model = null;
            if (Session["Institutions"] == null)
            {
                model = this.serviceOperations.GetDefaultInstitutions();
                Session["Institutions"] = model.Institutions;
            }
            else
            {
                model = new InstitutionModel { Institutions = Session["Institutions"] as List<Institution>, Error = null, Success = true };
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(InstitutionModel institutionModel)
        {
            InstitutionModel model = this.serviceOperations.GetInstitutionDetails(institutionModel);
            if (model.Institutions == null)
            {
                model.Institutions = Session["Institutions"] as List<Institution>;
            }

            return View(model);
        }

        public ActionResult DiscoverAndAddLogin(string id)
        {
            long longId = Convert.ToInt64(id);
            InstitutionModel model = this.serviceOperations.GetInstitutionDetails(new InstitutionModel { InstitutionId = longId });
            Session["InstitutionDetail"] = model.InstitutionDetail;
            return View(model);
        }

        [HttpPost]
        public ActionResult DiscoverAndAddLogin(InstitutionModel model)
        {
            model.InstitutionDetail = Session["InstitutionDetail"] as InstitutionDetail;
            DiscoverAddModel discoverModel = this.serviceOperations.DiscoverAndAdd(model);
            Session["DAM"] = discoverModel;
            if (discoverModel.MFA)
            {
                Session.Remove("InstitutionDetail");
                return RedirectToAction("DiscoverAndAddChallenge", "Institutions");
            }
            else
            {
                Session.Remove("InstitutionDetail");
                return RedirectToAction("DiscoverAndAddResult", "Institutions");
            }
        }

        public ActionResult DiscoverAndAddChallenge()
        {
            DiscoverAddModel dam = Session["DAM"] as DiscoverAddModel;
            return View(dam);
        }

        [HttpPost]
        public ActionResult DiscoverAndAddChallenge(DiscoverAddModel dam)
        {
            DiscoverAddModel model = Session["DAM"] as DiscoverAddModel;
            model.UseSame = dam.UseSame;
            model.Answer = dam.Answer;
            model = this.serviceOperations.DiscoverAndAddResponse(model);
            Session["DAM"] = model;
            return RedirectToAction("DiscoverAndAddResult", "Institutions");
        }

        public ActionResult DiscoverAndAddResult()
        {
            DiscoverAddModel dam = Session["DAM"] as DiscoverAddModel;
            return View(dam);
        }

        public ActionResult GetImageStream()
        {
            DiscoverAddModel dam = Session["DAM"] as DiscoverAddModel;
            byte[] byteArray = dam.Challenges.challenge[0].AnyIntuitObjects[1] as byte[];
            return new FileContentResult(byteArray, "image/jpeg");
        }
    }
}
