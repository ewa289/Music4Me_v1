using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Music4Me.Common.Extensions
{
    public static class JsonExtensions
    {
        public static T ValueByKey<T>(this JToken data, string propertyName)
        {
            var value = data[propertyName];
            return value != null ? data[propertyName].ToObject<T>() : default(T);
        }
    }
}
