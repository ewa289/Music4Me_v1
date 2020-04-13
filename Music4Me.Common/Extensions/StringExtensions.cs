using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Music4Me.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Serialize(
           this object data,
           bool indentedFormating = false,
           DefaultValueHandling defaultValueHandling = DefaultValueHandling.Include)
        {
            var result = JsonConvert.SerializeObject(data, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "yyyy-MM-ddTHH:mm:sszzz",
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = defaultValueHandling,
                Formatting = indentedFormating ? Formatting.Indented : Formatting.None
            });
            return result;
        }

        public static T Deserialize<T>(this string content)
        {
            if (content == null) return default(T);

            var item = JsonConvert.DeserializeObject<T>(content,
                 new JsonSerializerSettings {
                     NullValueHandling = NullValueHandling.Ignore,
                     DateFormatString = "yyyy-MM-ddTHH:mm:sszzz",
                     ContractResolver = new CamelCasePropertyNamesContractResolver()
                 }
            );
            return item;
        }
        public static string NullIfEmpty(this string value)
        {
            if (value == null || value.Trim() == String.Empty) {
                return null;
            } else
                return value;
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        public static T ToEnum<T>(this string input)
        {
            return input.IsNullOrEmpty()
                ? default(T)
                : (T)Enum.Parse(typeof(T), input, true);
        }

        public static bool IsValidEmail(this string input)
        {
            if (input.IsNullOrEmpty()) return false;

            var regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");

            return regex.IsMatch(input);
        }
    }
}
