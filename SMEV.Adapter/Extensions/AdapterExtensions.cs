﻿using Newtonsoft.Json;
using SMEV.Adapter.Enums;
using SMEV.Adapter.Helpers;
using SMEV.Adapter.Models.Find;
using SMEV.Adapter.Models.Send;
using System.ComponentModel;

namespace SMEV.Adapter.Extensions
{
    internal static class AdapterExtensions
    {
        internal static async Task<Model> ExecuteRequestToSmev<Model>(this HttpClient httpClient, EndpointAdapter endpoint, object messageBody)
        {
            var uri = new Uri(@$"{httpClient.BaseAddress}/{endpoint}");

            var messageData = JsonConvert.SerializeObject(messageBody);

            var response = await httpClient.GetResponse(uri, messageData);

            object model = endpoint switch
            {
                EndpointAdapter.send => JsonHelper.DeserializeResponseSmev<ResponseSentMessage>(response),
                EndpointAdapter.find => JsonHelper.DeserializeResponseSmev<QueryResult>(response),
                EndpointAdapter.get => JsonHelper.DeserializeResponseSmev<string>(response),
                _ => throw new InvalidEnumArgumentException(nameof(endpoint)),
            };

            return (Model)Convert.ChangeType(model, typeof(Model));
        }
    }
}
