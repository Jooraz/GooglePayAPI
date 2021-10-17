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
using System.IO;
using System.Text;
using Google.Apis.Auth.OAuth2;
using GooglePayAPI.Data;
using Newtonsoft.Json;
using DateTime = System.DateTime;

/*******************************
*
* class that defines JWT format for a Google Pay Pass.
*
* to check the JWT protocol for Google Pay Passes, check:
* https://developers.google.com/pay/passes/reference/s2w-reference#google-pay-api-for-passes-jwt
*
* also demonstrates RSA-SHA256 signing implementation to make the signed JWT used
* in links and buttons. Learn more:
* https://developers.google.com/pay/passes/guides/get-started/implementing-the-api/save-to-google-pay
*
*******************************/
namespace GooglePayAPI
{
    /// <summary>
    ///     class that defines JWT format for a Google Pay Pass.
    ///     to check the JWT protocol for Google Pay Passes, check:
    ///     https://developers.google.com/pay/passes/reference/s2w-reference#google-pay-api-for-passes-jwt
    ///     also demonstrates RSA-SHA256 signing implementation to make the signed JWT used
    ///     in links and buttons. Learn more:
    ///     https://developers.google.com/pay/passes/guides/get-started/implementing-the-api/save-to-google-pay
    /// </summary>
    public class Jwt
    {
        public string aud;
        private readonly ServiceAccountCredential credential;
        public int iat;
        public string iss;
        public List<string> origins;

        public JwtPayload payload;
        public string typ;

        public Jwt(Config config)
        {
            aud = config.Audience;
            iat = (int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            typ = config.JwtType;
            iss = config.ServiceAccountEmailAddress;
            origins = config.Origins;
            payload = new JwtPayload();
            var serviceAccountFile = config.ServiceAccountFile;
            using (var fs = new FileStream(serviceAccountFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                credential = ServiceAccountCredential.FromServiceAccountData(fs);
            }
        }

        public void AddOfferClass(OfferClass offerClass)
        {
            if (payload.offerClasses == null) payload.offerClasses = new List<OfferClass>();
            payload.offerClasses.Add(offerClass);
        }

        public void AddOfferObject(OfferObject offerObject)
        {
            if (payload.offerObjects == null) payload.offerObjects = new List<OfferObject>();
            payload.offerObjects.Add(offerObject);
        }

        public void AddLoyaltyClass(LoyaltyClass loyaltyClass)
        {
            if (payload.loyaltyClasses == null) payload.loyaltyClasses = new List<LoyaltyClass>();
            payload.loyaltyClasses.Add(loyaltyClass);
        }

        public void AddLoyaltyObject(LoyaltyObject loyaltyObject)
        {
            if (payload.loyaltyObjects == null) payload.loyaltyObjects = new List<LoyaltyObject>();
            payload.loyaltyObjects.Add(loyaltyObject);
        }

        public void AddGiftCardClass(GiftCardClass giftCardClass)
        {
            if (payload.giftCardClasses == null) payload.giftCardClasses = new List<GiftCardClass>();
            payload.giftCardClasses.Add(giftCardClass);
        }

        public void AddGiftCardObject(GiftCardObject giftCardObject)
        {
            if (payload.giftCardObjects == null) payload.giftCardObjects = new List<GiftCardObject>();
            payload.giftCardObjects.Add(giftCardObject);
        }

        public void AddEventTicketClass(EventTicketClass eventTicketClass)
        {
            if (payload.eventTicketClasses == null) payload.eventTicketClasses = new List<EventTicketClass>();
            payload.eventTicketClasses.Add(eventTicketClass);
        }

        public void AddEventTicketObject(EventTicketObject eventTicketObject)
        {
            if (payload.eventTicketObjects == null) payload.eventTicketObjects = new List<EventTicketObject>();
            payload.eventTicketObjects.Add(eventTicketObject);
        }

        public void AddFlightClass(FlightClass flightClass)
        {
            if (payload.flightClasses == null) payload.flightClasses = new List<FlightClass>();
            payload.flightClasses.Add(flightClass);
        }

        public void AddFlightObject(FlightObject flightObject)
        {
            if (payload.flightObjects == null) payload.flightObjects = new List<FlightObject>();
            payload.flightObjects.Add(flightObject);
        }

        public void AddTransitClass(TransitClass transitClass)
        {
            if (payload.transitClasses == null) payload.transitClasses = new List<TransitClass>();
            payload.transitClasses.Add(transitClass);
        }

        public void AddTransitObject(TransitObject transitObject)
        {
            if (payload.transitObjects == null) payload.transitObjects = new List<TransitObject>();
            payload.transitObjects.Add(transitObject);
        }

        public string GenerateSignedJwt()
        {
            var header = "{\"alg\":\"RS256\",\"typ\":\"JWT\"}";
            var payload = JsonConvert.SerializeObject(this,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            var encodedSafeHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(header)).Replace('+', '-')
                .Replace('/', '_').Replace("=", "");
            var encodedSafePayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(payload)).Replace('+', '-')
                .Replace('/', '_').Replace("=", "");
            ;
            var signature = credential
                .CreateSignature(Encoding.UTF8.GetBytes($"{encodedSafeHeader}.{encodedSafePayload}")).Replace('+', '-')
                .Replace('/', '_').Replace("=", "");

            var token = $"{encodedSafeHeader}.{encodedSafePayload}.{signature}";
            return token;
        }

        public class JwtPayload
        {
            public List<EventTicketClass> eventTicketClasses;
            public List<EventTicketObject> eventTicketObjects;
            public List<FlightClass> flightClasses;
            public List<FlightObject> flightObjects;
            public List<GiftCardClass> giftCardClasses;
            public List<GiftCardObject> giftCardObjects;
            public List<LoyaltyClass> loyaltyClasses;
            public List<LoyaltyObject> loyaltyObjects;
            public List<OfferClass> offerClasses;
            public List<OfferObject> offerObjects;
            public List<TransitClass> transitClasses;
            public List<TransitObject> transitObjects;
        }
    }
}