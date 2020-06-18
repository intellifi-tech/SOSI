using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SOSI.IyziPayHelper
{
    class IyzicoHelper
    {
        string APIAnahtari = "sandbox-S8fBp3d3O6g2v4iLlweEymY7jRkFBQnV";
        string GuvenlikAnahtari = "sandbox-trdXadVcZmdSN8GFnf6Cmb5pzGr8JIYE";
        private static readonly String AUTHORIZATION = "Authorization";
        private static readonly String CONVERSATION_ID_HEADER_NAME = "x-conversation-id";
        private static readonly String CLIENT_VERSION_HEADER_NAME = "x-iyzi-client-version";
        private static readonly String IYZIWS_V2_HEADER_NAME = "IYZWSv2 ";
        Options Options1 = new Options() {
            ApiKey = "sandbox-S8fBp3d3O6g2v4iLlweEymY7jRkFBQnV",
            BaseUrl = "https://sandbox-api.iyzipay.com",
            SecretKey = "sandbox-trdXadVcZmdSN8GFnf6Cmb5pzGr8JIYE"
        };

        public string MusteriOlustur(CreateCustomerRequest CreateCustomerRequest1)
        {
            var client = new RestSharp.RestClient("https://sandbox-api.iyzipay.com/v2/subscription/customers");
            client.Timeout = -1;
            var request = new RestSharp.RestRequest(RestSharp.Method.POST);


            var Tokenn = PrepareAuthorizationStringWithRequestBody(null, "https://sandbox-api.iyzipay.com/v2/subscription/customers", Options1);
            request.AddHeader("Authorization", Tokenn);

            string randomString = $"{DateTime.Now:yyyyMMddHHmmssfff}";
            CreateCustomerRequest1 = new CreateCustomerRequest
            {
                Email = $"iyzico-{randomString}@intellifi.tech",
                Locale = Locale.TR.ToString(),
                Name = "Mesut",
                Surname = "Polat",
                BillingAddress = new Address
                {
                    City = "İstanbul",
                    Country = "Türkiye",
                    Description = "Ortabayır Mahallesi Talatpaşa Caddesi No:67/5 Kat:3 Levent/Kağıthane - İSTANBUL",
                    ContactName = "Mesut Polat",
                    ZipCode = "010101"
                },
                ShippingAddress = new Address
                {
                    City = "İstanbul",
                    Country = "Türkiye",
                    Description = "Ortabayır Mahallesi Talatpaşa Caddesi No:67/5 Kat:3 Levent/Kağıthane - İSTANBUL",
                    ContactName = "Mesut Polat",
                    ZipCode = "010102"
                },
                ConversationId = "123456789",
                GsmNumber = "+905350000000",
                IdentityNumber = "55555555555"
            };


            request.AddHeader("Content-Type", "*/*");
            //if (!isLogin)
            //{
            //    request.AddHeader("Authorization", "Bearer " + GetApiToken());
            //}
            //request.AddParameter("locale", );
            //request.AddParameter("conversationId", "");
            request.AddParameter("name", CreateCustomerRequest1.Name);
            request.AddParameter("surname", CreateCustomerRequest1.Surname);
            request.AddParameter("identitynumber", CreateCustomerRequest1.IdentityNumber);
            request.AddParameter("email", CreateCustomerRequest1.Email);
            request.AddParameter("gsmNumber", CreateCustomerRequest1.GsmNumber);
            request.AddParameter("billingAddress.contactName", CreateCustomerRequest1.BillingAddress.ContactName);
            request.AddParameter("billingAddress.city", CreateCustomerRequest1.BillingAddress.City);
            request.AddParameter("billingAddress.country", CreateCustomerRequest1.BillingAddress.Country);
            request.AddParameter("billingAddress.address", CreateCustomerRequest1.BillingAddress.Description);
            request.AddParameter("billingAddress.zipCode", CreateCustomerRequest1.BillingAddress.ZipCode);
            request.AddParameter("shippingAddress.contactName", CreateCustomerRequest1.ShippingAddress.ContactName);
            request.AddParameter("shippingAddress.city", CreateCustomerRequest1.ShippingAddress.City);
            request.AddParameter("shippingAddress.country", CreateCustomerRequest1.ShippingAddress.Country);
            request.AddParameter("shippingAddress.address", CreateCustomerRequest1.ShippingAddress.Description);
            request.AddParameter("shippingAddress.zipCode", CreateCustomerRequest1.ShippingAddress.ZipCode);
            
            RestSharp.IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.Unauthorized &&
                response.StatusCode != HttpStatusCode.InternalServerError &&
                response.StatusCode != HttpStatusCode.BadRequest &&
                response.StatusCode != HttpStatusCode.Forbidden &&
                response.StatusCode != HttpStatusCode.MethodNotAllowed &&
                response.StatusCode != HttpStatusCode.NotAcceptable &&
                response.StatusCode != HttpStatusCode.RequestTimeout &&
                response.StatusCode != HttpStatusCode.NotFound)
            {
                return response.Content;
            }
            else
            {
                return "Hata";
            }

        }





        #region DTOS
        public class CreateCustomerRequest : BaseRequestV2
        {
            public string Name { get; set; }
            public string Surname { get; set; }
            public string IdentityNumber { get; set; }
            public string Email { get; set; }
            public string GsmNumber { get; set; }
            public Address BillingAddress { get; set; }
            public Address ShippingAddress { get; set; }
        }
        public class Address
        {
            [JsonProperty(PropertyName = "Address")]
            public String Description { get; set; }
            public String ZipCode { get; set; }
            public String ContactName { get; set; }
            public String City { get; set; }
            public String Country { get; set; }

            public String ToPKIRequestString()
            {
                return ToStringRequestBuilder.NewInstance()
                    .Append("address", Description)
                    .Append("zipCode", ZipCode)
                    .Append("contactName", ContactName)
                    .Append("city", City)
                    .Append("country", Country)
                    .GetRequestString();
            }
        }

        //------------------------------------------
        public class Options
        {
            public String ApiKey { get; set; }
            public String SecretKey { get; set; }
            public String BaseUrl { get; set; }
        }

        public class BaseRequestV2
        {
            public String Locale { get; set; }
            public String ConversationId { get; set; }
        }


        public sealed class Locale
        {
            private readonly String value;

            public static readonly Locale EN = new Locale("en");
            public static readonly Locale TR = new Locale("tr");

            private Locale(String value)
            {
                this.value = value;
            }

            public override String ToString()
            {
                return value;
            }
        }



        #endregion

        #region  Default Helper Classes

        public class ToStringRequestBuilder
        {
            private String _requestString;

            private ToStringRequestBuilder(String requestString)
            {
                this._requestString = requestString;
            }

            public static ToStringRequestBuilder NewInstance()
            {
                return new ToStringRequestBuilder("");
            }

            public static ToStringRequestBuilder NewInstance(String requestString)
            {
                return new ToStringRequestBuilder(requestString);
            }

            public ToStringRequestBuilder AppendSuper(String superRequestString)
            {
                if (superRequestString != null)
                {
                    superRequestString = superRequestString.Substring(1);
                    superRequestString = superRequestString.Substring(0, superRequestString.Length - 1);

                    if (superRequestString.Length > 0)
                    {
                        this._requestString = this._requestString + superRequestString + ",";
                    }
                }
                return this;
            }

            public ToStringRequestBuilder Append(String key, Object value = null)
            {
                if (value != null)
                {
                    if (value is RequestStringConvertible)
                    {
                        AppendKeyValue(key, ((RequestStringConvertible)value).ToPKIRequestString());
                    }
                    else
                    {
                        AppendKeyValue(key, value.ToString());
                    }
                }
                return this;
            }

            public ToStringRequestBuilder AppendPrice(String key, String value)
            {
                if (value != null)
                {
                    AppendKeyValue(key, RequestFormatter.FormatPrice(value));
                }
                return this;
            }

            public ToStringRequestBuilder AppendList<T>(String key, List<T> list = null) where T : RequestStringConvertible
            {
                if (list != null)
                {
                    String appendedValue = "";
                    foreach (RequestStringConvertible value in list)
                    {
                        appendedValue = appendedValue + value.ToPKIRequestString() + ", ";
                    }
                    AppendKeyValueArray(key, appendedValue);
                }
                return this;
            }

            public ToStringRequestBuilder AppendList(String key, List<int> list = null)
            {
                if (list != null)
                {
                    String appendedValue = "";
                    foreach (int value in list)
                    {
                        appendedValue = appendedValue + value + ", ";
                    }
                    AppendKeyValueArray(key, appendedValue);
                }
                return this;
            }

            private ToStringRequestBuilder AppendKeyValue(String key, String value)
            {
                if (value != null)
                {
                    this._requestString = this._requestString + key + "=" + value + ",";
                }
                return this;
            }

            private ToStringRequestBuilder AppendKeyValueArray(String key, String value)
            {
                if (value != null)
                {
                    value = value.Substring(0, value.Length - 2);
                    this._requestString = this._requestString + key + "=[" + value + "],";
                }
                return this;
            }

            private ToStringRequestBuilder AppendPrefix()
            {
                this._requestString = "[" + this._requestString + "]";
                return this;
            }

            private ToStringRequestBuilder RemoveTrailingComma()
            {
                if (!string.IsNullOrEmpty(this._requestString))
                {
                    this._requestString = this._requestString.Substring(0, this._requestString.Length - 1);
                }
                return this;
            }

            public String GetRequestString()
            {
                RemoveTrailingComma();
                AppendPrefix();
                return _requestString;
            }
        }
        public interface RequestStringConvertible
        {
            String ToPKIRequestString();
        }
        public class RequestFormatter
        {
            public static String FormatPrice(String price)
            {
                if (!price.Contains("."))
                {
                    return price + ".0";
                }
                int subStrIndex = 0;
                String priceReversed = StringHelper.Reverse(price);
                for (int i = 0; i < priceReversed.Length; i++)
                {
                    if (priceReversed[i].Equals('0'))
                    {
                        subStrIndex = i + 1;
                    }
                    else if (priceReversed[i].Equals('.'))
                    {
                        priceReversed = "0" + priceReversed;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
                return StringHelper.Reverse(priceReversed.Substring(subStrIndex));
            }
        }
        class StringHelper
        {
            public static string Reverse(string s)
            {
                char[] charArray = s.ToCharArray();
                Array.Reverse(charArray);
                return new string(charArray);
            }
        }

        private static String PrepareAuthorizationStringWithRequestBody(BaseRequestV2 request, String url, Options options)
        {
            String randomKey = GenerateRandomKey();
            String uriPath = FindUriPath(url);

            String payload = request != null ? uriPath + JsonBuilder.SerializeObjectToPrettyJson(request) : uriPath;
            String dataToEncrypt = randomKey + payload;
            String hash = HashGeneratorV2.GenerateHash(options.ApiKey, options.SecretKey, randomKey, dataToEncrypt);
            return IYZIWS_V2_HEADER_NAME + hash;
        }

        private static String GenerateRandomKey()
        {
            return DateTime.Now.ToString("ddMMyyyyhhmmssffff");
        }
        private static String FindUriPath(String url)
        {
            int startIndex = url.IndexOf("/v2");
            int endIndex = url.IndexOf("?");
            int length = endIndex == -1 ? url.Length - startIndex : endIndex - startIndex;
            return url.Substring(startIndex, length);
        }
        public class HashGeneratorV2
        {
            private HashGeneratorV2()
            {
            }
            public static String GenerateHash(String apiKey, String secretKey, String randomString, String dataToEncrypt)
            {
                HashAlgorithm algorithm = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));
                byte[] computedHash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(dataToEncrypt));
                String computedHashAsHex = BitConverter.ToString(computedHash).Replace("-", "").ToLower();
                String authorizationString = "apiKey:" + apiKey + "&randomKey:" + randomString + "&signature:" + computedHashAsHex;
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(authorizationString));
            }
        }
        public class BaseRequest : RequestStringConvertible
        {
            public String Locale { get; set; }
            public String ConversationId { get; set; }

            public virtual String ToPKIRequestString()
            {
                return ToStringRequestBuilder.NewInstance()
                    .Append("locale", Locale)
                    .Append("conversationId", ConversationId)
                    .GetRequestString();
            }
        }
        public class JsonBuilder
        {
            public static string SerializeToJsonString(BaseRequest request)
            {
                return JsonConvert.SerializeObject(request, new JsonSerializerSettings()
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }

            public static string SerializeToJsonString(BaseRequestV2 request)
            {
                return JsonConvert.SerializeObject(request, new JsonSerializerSettings()
                {
                    Formatting = Formatting.None,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }

            public static string SerializeObjectToPrettyJson(BaseRequestV2 value)
            {
                StringBuilder sb = new StringBuilder(256);
                StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);

                var jsonSerializer = JsonSerializer.CreateDefault();
                jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
                jsonSerializer.DefaultValueHandling = DefaultValueHandling.Ignore;
                jsonSerializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
                jsonSerializer.Formatting = Formatting.Indented;

                using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.IndentChar = '\t';
                    jsonWriter.Indentation = 1;

                    jsonSerializer.Serialize(jsonWriter, value, typeof(BaseRequestV2));
                }

                string json = sw.ToString();
                return json.Replace("\r", "");
            }

            public static StringContent ToJsonString(BaseRequest request)
            {
                return new StringContent(SerializeToJsonString(request), Encoding.Unicode, "application/json");
            }

            public static StringContent ToJsonString(BaseRequestV2 request)
            {
                return new StringContent(SerializeObjectToPrettyJson(request), Encoding.UTF8, "application/json");
            }
        }
        #endregion
    }
}