using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cubase.Hub.Services.Distributers.SoundCloud
{
    public static class HttpResponseExtensions
    {
        public static T GetModel<T>(this HttpResponseMessage message)
        {
            var responseAsString = message.Content.ReadAsStringAsync().Result;
            return JsonSerializer.Deserialize<T>(responseAsString);
        }
    }
}
