/**
* Copyright 2019 Google Inc. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

// These handle formatting the JSON resource definitions
// download the latest client at:  https://developers.google.com/pay/passes/support/libraries#libraries
using System.Collections.Generic;
using GooglePayAPI.Data;

namespace GooglePayAPI
{
    public class ResourceDefinitions
    {
        private static ResourceDefinitions resourceDefinitions = new ResourceDefinitions();

        private ResourceDefinitions()
        {
        }

        public static ResourceDefinitions getInstance()
        {
            if (resourceDefinitions == null)
            {
                resourceDefinitions = new ResourceDefinitions();
            }

            return resourceDefinitions;
        }

        /// <summary>
        /// Generate a loyalty object
        /// See https://developers.google.com/pay/passes/reference/v1/loyaltyobject
        /// </summary>
        /// <param name="objectId"> the unique identifier for a object</param>
        /// <returns>the inserted loyalty object</returns>
        public LoyaltyObject MakeLoyaltyObjectResource(string objectId, string classId, string accountId, string accountName, List<Uri> links = null)
        {
            // Define the resource representation of the Object
            // values should be from your DB/services; here we hardcode information
            // below defines an loyalty object. For more properties, check:
            //// https://developers.google.com/pay/passes/reference/v1/loyaltyobject/insert
            //// https://developers.google.com/pay/passes/guides/pass-verticals/loyalty/design

            // There is a client lib to help make the data structure. Newest client is on
            // devsite:
            //// https://developers.google.com/pay/passes/support/libraries#libraries
            LoyaltyObject payload = new LoyaltyObject();
            //required fields
            payload.Id = objectId;
            payload.ClassId = classId;
            payload.State = "active";
            //optional fields. See design and reference api for more
            payload.AccountId = accountId;
            payload.AccountName = accountName;
            payload.Barcode = new Barcode()
            {
                Type = "qrCode",
                Value = accountId,
                AlternateText = accountId
            };

            payload.LinksModuleData = new LinksModuleData()
            {
                Uris = links
            };
            payload.InfoModuleData = new InfoModuleData()
            {
                ShowLastUpdateTime = true
            };

            return payload;
        }
    }
}