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
using Google.Apis.Requests;
using GooglePayAPI.Data;
using System;

namespace GooglePayAPI
{
    /*******************************
    *
    *  These are services that you would expose to front end so they can generate save links or buttons.
    *
    *  Depending on your needs, you only need to implement 1 of the services.
    *
    *******************************/
    public class Services
    {
        private readonly Config _config;

        public Services(Config config)
        {
            _config = config;
        }
        /*******************************
        *
        *
        *  See all the verticals: https://developers.google.com/pay/passes/guides/overview/basics/about-google-pay-api-for-passes
        *
        *******************************/
        public enum VerticalType
        {
            OFFER,
            EVENTTICKET,
            FLIGHT,     // also referred to as Boarding Passes
            GIFTCARD,
            LOYALTY,
            TRANSIT
        }
        /// <summary>
        /// Generates a signed "fat" JWT.
        /// No REST calls made.
        /// Use fat JWT in JS web button.
        /// Fat JWT is too long to be used in Android intents.
        /// Possibly might break in redirects.
        /// </summary>
        /// <param name="verticalType"> pass type to created</param>
        /// <param name="classId">the unique identifier for the class</param>
        /// <param name="objectId">the unique identifier for the object</param>
        /// <returns></returns>
        public string MakeFatJwt(VerticalType verticalType, IDirectResponseSchema classElem, IDirectResponseSchema objectElem)
        {
            RestMethods restMethods = new RestMethods(_config);
            // create JWT to put objects and class into JSON Web Token (JWT) format for Google Pay API for Passes
            Jwt googlePassJwt = new Jwt(_config);
            // get class definition, object definition and see if Ids exist. for a fat JWT, first time a user hits the save button, the class and object are inserted            
            try
            {
                string classId;
                string objectId;

                switch (verticalType)
                {
                    case VerticalType.OFFER:
                        OfferClass offerClass = classElem as OfferClass;
                        OfferObject offerObject = objectElem as OfferObject;

                        classId = offerClass.Id;
                        objectId = offerObject.Id;

                        Console.WriteLine("\nMaking REST call to get class and object to see if they exist.");
                        OfferClass classResponse = restMethods.getOfferClass(classId);
                        OfferObject objectResponse = restMethods.getOfferObject(objectId);
                        // check responses
                        if (!(classResponse is null))
                        {
                            Console.WriteLine($"classId: {classId} already exists.");
                        }
                        if (!(objectResponse is null))
                        {
                            Console.WriteLine($"objectId: {objectId} already exists.");
                        }
                        if (!(classResponse is null) && objectResponse.ClassId != offerObject.ClassId)
                        {
                            Console.WriteLine($"the classId of inserted object is ({objectResponse.ClassId}). " +
                            $"It does not match the target classId ({offerObject.ClassId}). The saved object will not " +
                            "have the class properties you expect.");
                        }
                        // need to add both class and object resource definitions into JWT because no REST calls made to pre-insert
                        googlePassJwt.AddOfferClass(offerClass);
                        googlePassJwt.AddOfferObject(offerObject);
                        break;
                    case VerticalType.LOYALTY:
                        LoyaltyClass loyaltyClass = classElem as LoyaltyClass;
                        LoyaltyObject loyaltyObject = objectElem as LoyaltyObject;

                        classId = loyaltyClass.Id;
                        objectId = loyaltyObject.Id;

                        Console.WriteLine("\nMaking REST call to get class and object to see if they exist.");
                        LoyaltyClass loyaltyClassResponse = restMethods.getLoyaltyClass(classId);
                        LoyaltyObject loyaltyObjectResponse = restMethods.getLoyaltyObject(objectId);
                        // check responses
                        if (!(loyaltyClassResponse is null))
                        {
                            Console.WriteLine($"classId: {classId} already exists.");
                        }
                        if (!(loyaltyObjectResponse is null))
                        {
                            Console.WriteLine($"objectId: {objectId} already exists.");
                        }
                        if (!(loyaltyClassResponse is null) && loyaltyObjectResponse.ClassId != loyaltyObject.ClassId)
                        {
                            Console.WriteLine($"the classId of inserted object is ({loyaltyObjectResponse.ClassId}). " +
                            $"It does not match the target classId ({loyaltyObject.ClassId}). The saved object will not " +
                            "have the class properties you expect.");
                        }
                        // need to add both class and object resource definitions into JWT because no REST calls made to pre-insert
                        googlePassJwt.AddLoyaltyClass(loyaltyClass);
                        googlePassJwt.AddLoyaltyObject(loyaltyObject);
                        break;
                    case VerticalType.EVENTTICKET:
                        EventTicketClass eventTicketClass = classElem as EventTicketClass;
                        EventTicketObject eventTicketObject = objectElem as EventTicketObject;

                        classId = eventTicketClass.Id;
                        objectId = eventTicketObject.Id;

                        Console.WriteLine("\nMaking REST call to get class and object to see if they exist.");
                        EventTicketClass eventTicketClassResponse = restMethods.getEventTicketClass(classId);
                        EventTicketObject eventTicketObjectResponse = restMethods.getEventTicketObject(objectId);
                        // check responses
                        if (!(eventTicketClassResponse is null))
                        {
                            Console.WriteLine($"classId: {classId} already exists.");
                        }
                        if (!(eventTicketObjectResponse is null))
                        {
                            Console.WriteLine($"objectId: {objectId} already exists.");
                        }
                        if (!(eventTicketClassResponse is null) && eventTicketObjectResponse.ClassId != eventTicketObject.ClassId)
                        {
                            System.Console.WriteLine($"the classId of inserted object is ({eventTicketObjectResponse.ClassId}). " +
                            $"It does not match the target classId ({eventTicketObject.ClassId}). The saved object will not " +
                            "have the class properties you expect.");
                        }
                        // need to add both class and object resource definitions into JWT because no REST calls made to pre-insert
                        googlePassJwt.AddEventTicketClass(eventTicketClass);
                        googlePassJwt.AddEventTicketObject(eventTicketObject);
                        break;
                    case VerticalType.FLIGHT:
                        FlightClass flightClass = classElem as FlightClass;
                        FlightObject flightObject = objectElem as FlightObject;

                        classId = flightClass.Id;
                        objectId = flightObject.Id;

                        Console.WriteLine("\nMaking REST call to get class and object to see if they exist.");
                        FlightClass flightClassResponse = restMethods.getFlightClass(classId);
                        FlightObject flightObjectResponse = restMethods.getFlightObject(objectId);
                        // check responses
                        if (!(flightClassResponse is null))
                        {
                            Console.WriteLine($"classId: {classId} already exists.");
                        }
                        if (!(flightObjectResponse is null))
                        {
                            Console.WriteLine($"objectId: {objectId} already exists.");
                        }
                        if (!(flightClassResponse is null) && flightObjectResponse.ClassId != flightObject.ClassId)
                        {
                            Console.WriteLine($"the classId of inserted object is ({flightObjectResponse.ClassId}). " +
                            $"It does not match the target classId ({flightObject.ClassId}). The saved object will not " +
                            "have the class properties you expect.");
                        }
                        // need to add both class and object resource definitions into JWT because no REST calls made to pre-insert
                        googlePassJwt.AddFlightClass(flightClass);
                        googlePassJwt.AddFlightObject(flightObject);
                        break;
                    case VerticalType.GIFTCARD:
                        GiftCardClass giftCardClass = classElem as GiftCardClass;
                        GiftCardObject giftCardObject = objectElem as GiftCardObject;

                        classId = giftCardClass.Id;
                        objectId = giftCardObject.Id;

                        Console.WriteLine("\nMaking REST call to get class and object to see if they exist.");
                        GiftCardClass giftCardClassResponse = restMethods.getGiftCardClass(classId);
                        GiftCardObject giftCardObjectResponse = restMethods.getGiftCardObject(objectId);
                        // check responses
                        if (!(giftCardClassResponse is null))
                        {
                            Console.WriteLine($"classId: {classId} already exists.");
                        }
                        if (!(giftCardObjectResponse is null))
                        {
                            Console.WriteLine($"objectId: {objectId} already exists.");
                        }
                        if (!(giftCardClassResponse is null) && giftCardObjectResponse.ClassId != giftCardObject.ClassId)
                        {
                            Console.WriteLine($"the classId of inserted object is ({giftCardObjectResponse.ClassId}). " +
                            $"It does not match the target classId ({giftCardObject.ClassId}). The saved object will not " +
                            "have the class properties you expect.");
                        }
                        // need to add both class and object resource definitions into JWT because no REST calls made to pre-insert
                        googlePassJwt.AddGiftCardClass(giftCardClass);
                        googlePassJwt.AddGiftCardObject(giftCardObject);
                        break;
                    case VerticalType.TRANSIT:
                        TransitClass transitClass = classElem as TransitClass;
                        TransitObject transitObject = objectElem as TransitObject;

                        classId = transitClass.Id;
                        objectId = transitObject.Id;

                        Console.WriteLine("\nMaking REST call to get class and object to see if they exist.");
                        TransitClass transitClassResponse = restMethods.getTransitClass(classId);
                        TransitObject transitObjectResponse = restMethods.getTransitObject(objectId);
                        // check responses
                        if (!(transitClassResponse is null))
                        {
                            Console.WriteLine($"classId: {classId} already exists.");
                        }
                        if (!(transitObjectResponse is null))
                        {
                            Console.WriteLine($"objectId: {objectId} already exists.");
                        }
                        if (!(transitClassResponse is null) && transitObjectResponse.ClassId != transitObject.ClassId)
                        {
                            Console.WriteLine($"the classId of inserted object is ({transitObjectResponse.ClassId}). " +
                            $"It does not match the target classId ({transitObject.ClassId}). The saved object will not " +
                            "have the class properties you expect.");
                        }
                        // need to add both class and object resource definitions into JWT because no REST calls made to pre-insert
                        googlePassJwt.AddTransitClass(transitClass);
                        googlePassJwt.AddTransitObject(transitObject);
                        break;
                }
                // return "fat" JWT. Try putting it into JS web button
                // note button needs to be rendered in local web server who's domain matches the ORIGINS
                // defined in the JWT. See https://developers.google.com/pay/passes/reference/s2w-reference
                return googlePassJwt.GenerateSignedJwt();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        /// <summary>
        /// Generates a signed "object" JWT.
        /// 1 REST call is made to pre-insert class.
        /// If this JWT only contains 1 object, usually isn't too long; can be used in Android intents/redirects.
        /// </summary>
        /// <param name="verticalType"> pass type to created</param>
        /// <param name="classId">the unique identifier for the class</param>
        /// <param name="objectId">the unique identifier for the object</param>
        /// <returns></returns>
        public string MakeObjectJwt(VerticalType verticalType, IDirectResponseSchema element)
        {
            RestMethods restMethods = new RestMethods(_config);
            // create JWT to put objects and class into JSON Web Token (JWT) format for Google Pay API for Passes
            Jwt googlePassJwt = new Jwt(_config);
            // get class, object definitions, insert class (check in Merchant center GUI: https://pay.google.com/gp/m/issuer/list)
            try
            {
                switch (verticalType)
                {
                    case VerticalType.OFFER:
                        OfferObject offerObject = element as OfferObject;
                        googlePassJwt.AddOfferObject(offerObject);
                        break;
                    case VerticalType.LOYALTY:
                        LoyaltyObject loyaltyObject = element as LoyaltyObject;
                        googlePassJwt.AddLoyaltyObject(loyaltyObject);
                        break;
                    case VerticalType.EVENTTICKET:
                        EventTicketObject eventTicketObject = element as EventTicketObject;
                        googlePassJwt.AddEventTicketObject(eventTicketObject);
                        break;
                    case VerticalType.FLIGHT:
                        FlightObject flightObject = element as FlightObject;
                        googlePassJwt.AddFlightObject(flightObject);
                        break;
                    case VerticalType.GIFTCARD:
                        GiftCardObject giftCardObject = element as GiftCardObject;
                        googlePassJwt.AddGiftCardObject(giftCardObject);
                        break;
                    case VerticalType.TRANSIT:
                        TransitObject transitObject = element as TransitObject;
                        googlePassJwt.AddTransitObject(transitObject);
                        break;
                }
                // return "object" JWT.Try putting it into save link.
                // See https://developers.google.com/pay/passes/guides/get-started/implementing-the-api/save-to-google-pay#add-link-to-email
                return googlePassJwt.GenerateSignedJwt();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        /// <summary>
        /// Generates a signed "skinny" JWT.
        /// 2 REST calls are made to pre-insert class and object
        /// This JWT can be used in JS web button.
        /// This is the shortest type of JWT; recommended for Android intents/redirects.
        /// </summary>
        /// <param name="verticalType"> pass type to created</param>
        /// <param name="objectId">the unique identifier for the object</param>
        /// <returns></returns>
        public string MakeSkinnyJwt(VerticalType verticalType, string objectId)
        {
            RestMethods restMethods = new RestMethods(_config);
            // create JWT to put objects and class into JSON Web Token (JWT) format for Google Pay API for Passes
            Jwt googlePassJwt = new Jwt(_config);
            // get class, object definitions, insert class (check in Merchant center GUI: https://pay.google.com/gp/m/issuer/list)
            try
            {
                switch (verticalType)
                {
                    case VerticalType.OFFER:
                        var offerObject = new OfferObject() { Id = objectId };
                        googlePassJwt.AddOfferObject(offerObject);
                        break;
                    case VerticalType.LOYALTY:
                        var loyaltyObject = new LoyaltyObject() { Id = objectId };
                        googlePassJwt.AddLoyaltyObject(loyaltyObject);
                        break;
                    case VerticalType.EVENTTICKET:
                        var eventTicketObject = new EventTicketObject() { Id = objectId };
                        googlePassJwt.AddEventTicketObject(eventTicketObject);
                        break;
                    case VerticalType.FLIGHT:
                        var flightObject = new FlightObject() { Id = objectId };
                        googlePassJwt.AddFlightObject(flightObject);
                        break;
                    case VerticalType.GIFTCARD:
                        var giftCardObject = new GiftCardObject() { Id = objectId };
                        googlePassJwt.AddGiftCardObject(giftCardObject);
                        break;
                    case VerticalType.TRANSIT:
                        var transitObject = new TransitObject() { Id = objectId };
                        googlePassJwt.AddTransitObject(transitObject);
                        break;
                }
                // return "skinny" JWT. Try putting it into save link.
                // See https://developers.google.com/pay/passes/guides/get-started/implementing-the-api/save-to-google-pay#add-link-to-email
                return googlePassJwt.GenerateSignedJwt();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}