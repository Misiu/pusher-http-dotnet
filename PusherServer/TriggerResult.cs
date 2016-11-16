﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using PusherServer.Exceptions;
using PusherServer.Util;
using RestSharp;

namespace PusherServer
{
    internal class TriggerResult : RequestResult, ITriggerResult
    {
        private readonly IDictionary<string, string> _eventIds;

        /// <summary>
        /// Constructs a new instance of a TriggerResult based upon a passed in Rest Response
        /// </summary>
        /// <param name="response">The Rest Response which will form the basis of this Trigger Result</param>
        public TriggerResult(IRestResponse response) : base(response)
        {
            EventIdData eventIdData = null;

            try
            {
                eventIdData = JsonConvert.DeserializeObject<EventIdData>(response.Content);
            }
            catch (Exception)
            {
                string msg =
                    string.Format("The response body from the Pusher HTTP endpoint could not be parsed as JSON: {0}{1}",
                                  Environment.NewLine,
                                  response.Content);
                throw new TriggerResponseException(msg);
            }

            _eventIds = new ReadOnlyDictionary<string, string>(eventIdData.event_ids);
        }

        /// <inheritDoc/>
        public IDictionary<string, string> EventIds
        {
            get { return this._eventIds; }
        }
    }

    public class TriggerResult2 : RequestResult2, ITriggerResult
    {
        /// <summary>
        /// Constructs a new instance of a TriggerResult based upon a passed in Rest Response
        /// </summary>
        /// <param name="response">The Rest Response which will form the basis of this Trigger Result</param>
        /// <param name="responseContent">The response content as a string</param>
        public TriggerResult2(HttpResponseMessage response, string responseContent) : base(response, responseContent)
        {
            EventIdData eventIdData = null;

            try
            {
                eventIdData = JsonConvert.DeserializeObject<EventIdData>(responseContent);
            }
            catch (Exception)
            {
                string msg = $"The response body from the Pusher HTTP endpoint could not be parsed as JSON: {Environment.NewLine}{response.Content}";
                throw new TriggerResponseException(msg);
            }

            EventIds = new ReadOnlyDictionary<string, string>(eventIdData.event_ids);
        }

        /// <inheritDoc/>
        public IDictionary<string, string> EventIds { get; }
    }
}