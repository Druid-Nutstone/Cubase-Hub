using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public static class HttpResponseExtensions
    {
        public static T? GetModel<T>(this HttpResponseMessage message, Action<string> onErrorResponse)
        {
            var responseAsString = message.Content.ReadAsStringAsync().Result;
            if (message.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<T>(responseAsString);
            }
            else
            {
                onErrorResponse.Invoke(responseAsString);
                return default;
            }
        }

        public static string GetErrorResponse(this HttpResponseMessage message)
        {
            return message.Content.ReadAsStringAsync().Result;  
        }

        public static string GetResponseAsString(this HttpResponseMessage message)
        {
            return message.Content.ReadAsStringAsync().Result;
        }    
    }
}
