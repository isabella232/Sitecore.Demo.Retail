﻿//-----------------------------------------------------------------------
// <copyright file="CheckoutDataBaseJsonResult.cs" company="Sitecore Corporation">
//     Copyright (c) Sitecore Corporation 1999-2016
// </copyright>
// <summary>Defines the CheckoutDataBaseJsonResult class.</summary>
//-----------------------------------------------------------------------
// Copyright 2016 Sitecore Corporation A/S
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file 
// except in compliance with the License. You may obtain a copy of the License at
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the 
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions 
// and limitations under the License.
// -------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Commerce.Connect.CommerceServer;
using Sitecore.Commerce.Entities.Payments;
using Sitecore.Commerce.Entities.Shipping;
using Sitecore.Commerce.Services;
using Sitecore.Foundation.Commerce.Models;

namespace Sitecore.Reference.Storefront.Models.JsonResults
{
    public class CheckoutDataBaseJsonResult : BaseJsonResult
    {
        public CheckoutDataBaseJsonResult()
        {
        }

        public CheckoutDataBaseJsonResult(ServiceProviderResult result)
            : base(result)
        {
        }

        public IEnumerable<ShippingOptionBaseJsonResult> OrderShippingOptions { get; set; }

        public IEnumerable<LineShippingOptionBaseJsonResult> LineShippingOptions { get; set; }

        public ShippingMethodBaseJsonResult EmailDeliveryMethod { get; set; }

        public ShippingMethodBaseJsonResult ShipToStoreDeliveryMethod { get; set; }

        public IDictionary<string, string> Countries { get; set; }

        public IEnumerable<PaymentOption> PaymentOptions { get; set; }

        public IEnumerable<PaymentMethod> PaymentMethods { get; set; }

        public string PaymentClientToken { get; set; }

        public IEnumerable<ShippingMethod> ShippingMethods { get; set; }

        public bool IsUserAuthenticated { get; set; }

        public AddressListItemBaseJsonResult UserAddresses { get; set; }

        public string UserEmail { get; set; }

        public string CartLoyaltyCardNumber { get; set; }

        public string CurrencyCode { get; set; }

        public CartBaseJsonResult Cart { get; set; }

        public void InitializeShippingOptions(IEnumerable<ShippingOption> shippingOptions)
        {
            if (shippingOptions == null)
            {
                return;
            }

            var shippingOptionList = new List<ShippingOptionBaseJsonResult>();

            foreach (var shippingOption in shippingOptions)
            {
                var jsonResult = new ShippingOptionBaseJsonResult();

                jsonResult.Initialize(shippingOption);
                shippingOptionList.Add(jsonResult);
            }

            OrderShippingOptions = shippingOptionList;
        }

        public void InitializeLineItemShippingOptions(IEnumerable<LineShippingOption> lineItemShippingOptionList)
        {
            if (lineItemShippingOptionList == null)
            {
                return;
            }
            var lineShippingOptions = lineItemShippingOptionList as LineShippingOption[] ?? lineItemShippingOptionList.ToArray();
            if (!lineShippingOptions.Any())
            {
                return;
            }
            var lineShippingOptionList = new List<LineShippingOptionBaseJsonResult>();

            foreach (var lineShippingOption in lineShippingOptions)
            {
                var jsonResult = new LineShippingOptionBaseJsonResult();

                jsonResult.Initialize(lineShippingOption);
                lineShippingOptionList.Add(jsonResult);
            }

            LineShippingOptions = lineShippingOptionList;

            foreach (var line in Cart.Lines)
            {
                var lineShippingOption = lineShippingOptions.FirstOrDefault(l => l.LineId.Equals(line.ExternalCartLineId, StringComparison.OrdinalIgnoreCase));
                if (lineShippingOption != null)
                {
                    line.SetShippingOptions(lineShippingOption.ShippingOptions);
                }
            }
        }
    }
}