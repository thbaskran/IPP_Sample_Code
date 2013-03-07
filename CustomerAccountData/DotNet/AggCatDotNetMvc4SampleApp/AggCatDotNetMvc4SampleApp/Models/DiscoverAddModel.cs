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

using Intuit.Ipp.DataAggregation.AggregationCategorizationServices;
using Intuit.Ipp.DataAggregation.Data;

namespace AggCatDotNetMvc4SampleApp.Mvc4.Models
{
    public class DiscoverAddModel : ErrorModel
    {
        public InstitutionDetail InstitutionDetail { get; set; }
        public AccountList AccountList { get; set; }
        public ChallengeSession ChallengeSession { get; set; }
        public Challenges Challenges { get; set; }
        public bool MFA { get; set; }
        public string Answer { get; set; }
        public bool UseSame { get; set; }
    }
}