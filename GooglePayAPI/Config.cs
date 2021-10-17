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

using System;
using System.Collections.Generic;

namespace GooglePayAPI
{
    /// <summary>
    ///     Config
    ///     Used to define constants for:
    ///     a) authorizing REST calls
    ///     b) sign JSON Web Token (JWT)
    /// </summary>
    public class Config
    {
        public string ApplicationName { get; private set; } // this isn't required
        public string Audience { get; private set; }
        public string IssuerId { get; private set; }
        public string JwtType { get; private set; }
        public List<string> Origins { get; private set; }
        public List<string> Scopes { get; private set; }
        public string ServiceAccountEmailAddress { get; private set; }
        public string ServiceAccountFile { get; private set; }

        public Config()
        {

        }

        public Config(string serviceAccountEmailAddress, string serviceAccountFile,
            string issuerId, string applicationName = "", List<string> origins = null) : this()
        {
            try
            {
                // Identifiers of Service account
                ServiceAccountEmailAddress = serviceAccountEmailAddress;
                ServiceAccountFile = serviceAccountFile; //Path to file with private key and Google credential config

                // Used by the Google Pay API for Passes Client library
                ApplicationName = applicationName;

                // Identifier of Google Pay API for Passes Merchant Center
                IssuerId = issuerId;

                // List of origins for save to phone button. Used for JWT
                if (origins != null && origins.Count > 0)
                {
                    Origins = origins;
                }

                // Constants that are application agnostic. Used for JWT
                Audience = "google";
                JwtType = "savetoandroidpay";
                Scopes = new List<string>
                {
                    "https://www.googleapis.com/auth/wallet_object.issuer"
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}