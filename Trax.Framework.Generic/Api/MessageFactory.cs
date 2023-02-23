using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Trax.Models.Generic.Identity.Response;

namespace Trax.Framework.Generic.Api
{
    public class MessageFactory
    {
        private Logger.Logger _Logger;

        public MessageFactory(Logger.Logger Logger)
        {
            this._Logger = Logger;
        }
        public T SendRequest<T>(string EndPointUrl, string Token, string Action, string Payload, HttpMethod Method)
        {
            string _Response = string.Empty;
            DateTime dt1 = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
                using (HttpClient _HttpClient = new HttpClient())
                using (HttpRequestMessage _HttpRequestMessage = new HttpRequestMessage(Method, EndPointUrl + Action))
                {
                    _HttpClient.DefaultRequestHeaders.Clear();
                    _HttpClient.MaxResponseContentBufferSize = 2147483647;
                    _HttpClient.Timeout = TimeSpan.FromMilliseconds(5400000);
                    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    _HttpRequestMessage.Content = new StringContent(Payload, Encoding.UTF8, "application/json");
                    using (HttpResponseMessage _HttpResponseMessage = _HttpClient.SendAsync(_HttpRequestMessage).Result)
                    {
                        if (!_HttpResponseMessage.IsSuccessStatusCode)
                        {
                            switch (_HttpResponseMessage.StatusCode)
                            {
                                case System.Net.HttpStatusCode.BadRequest:
                                    _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                    _Logger.LogText(EndPointUrl + Action);
                                    _Logger.LogText(Payload);
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                case System.Net.HttpStatusCode.Unauthorized:
                                case System.Net.HttpStatusCode.NotFound:
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                default:
                                    TimeSpan span = (DateTime.Now) - dt1;
                                    _Logger.LogText("Execution time: " + span.TotalMilliseconds.ToString() + " milliseconds, retried " + i.ToString() + " times.");
                                    _Logger.LogText(EndPointUrl + Action);
                                    _Logger.LogText(Payload);
                                    if (i >= 3)
                                    {
                                        _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                        _Logger.LogText(EndPointUrl + Action);
                                        _Logger.LogText(Payload);
                                        throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                    }
                                    break;
                            }
                            continue;
                        }
                        _Response = _HttpResponseMessage.Content.ReadAsStringAsync().Result;
                        break;
                    }
                }
            }
            return Serializer.JsonSerializer.Deserialize<T>(_Response);
        }
        public string SendRequest(string EndPointUrl, string Action, string Payload, HttpMethod Method)
        {
            string _Response = string.Empty;
            DateTime dt1 = DateTime.Now;
            for (int i = 0; i < 4; i++)
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
                using (HttpClient _HttpClient = new HttpClient())
                using (HttpRequestMessage _HttpRequestMessage = new HttpRequestMessage(Method, EndPointUrl + Action))
                {
                    _HttpClient.DefaultRequestHeaders.Clear();
                    _HttpClient.MaxResponseContentBufferSize = 2147483647;
                    _HttpClient.Timeout = TimeSpan.FromMilliseconds(5400000);
                    if (!string.IsNullOrEmpty(Payload)) _HttpRequestMessage.Content = new StringContent(Payload, Encoding.UTF8, "application/json");
                    using (HttpResponseMessage _HttpResponseMessage = _HttpClient.SendAsync(_HttpRequestMessage).Result)
                    {
                        if (!_HttpResponseMessage.IsSuccessStatusCode)
                        {
                            switch (_HttpResponseMessage.StatusCode)
                            {
                                case System.Net.HttpStatusCode.BadRequest:
                                    _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                    _Logger.LogText(EndPointUrl + Action);
                                    _Logger.LogText(Payload);
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                case System.Net.HttpStatusCode.Unauthorized:
                                case System.Net.HttpStatusCode.NotFound:
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                default:
                                    TimeSpan span = (DateTime.Now) - dt1;
                                    _Logger.LogText("Execution time: " + span.TotalMilliseconds.ToString() + " milliseconds, retried " + i.ToString() + " times.");
                                    _Logger.LogText(EndPointUrl + Action);
                                    _Logger.LogText(Payload);
                                    if (i >= 3)
                                    {
                                        _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                        _Logger.LogText(EndPointUrl + Action);
                                        _Logger.LogText(Payload);
                                        throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                    }
                                    break;
                            }
                            continue;
                        }
                        _Response = _HttpResponseMessage.Content.ReadAsStringAsync().Result;
                        break;
                    }
                }
            }
            return _Response;
        }
        public LoginResponseDTO GetToken(string EndPointUrl, string Action, string UserName, string Password, HttpMethod Method)
        {
            string _Response = string.Empty;
            DateTime dt1 = DateTime.Now;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = 9999;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
            var _formContent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", UserName),
                        new KeyValuePair<string, string>("password", Password),
            });
            for (int i = 0; i < 4; i++)
            {
                using (HttpClient _HttpClient = new HttpClient())
                using (HttpRequestMessage _HttpRequestMessage = new HttpRequestMessage(Method, EndPointUrl + Action))
                {
                    _HttpClient.DefaultRequestHeaders.Clear();
                    _HttpClient.MaxResponseContentBufferSize = 2147483647;
                    _HttpClient.Timeout = TimeSpan.FromMilliseconds(5400000);
                    _HttpRequestMessage.Content = _formContent;
                    using (HttpResponseMessage _HttpResponseMessage = _HttpClient.SendAsync(_HttpRequestMessage).Result)
                    {
                        if (!_HttpResponseMessage.IsSuccessStatusCode)
                        {
                            switch (_HttpResponseMessage.StatusCode)
                            {
                                case System.Net.HttpStatusCode.BadRequest:
                                case System.Net.HttpStatusCode.Unauthorized:
                                case System.Net.HttpStatusCode.NotFound:
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                default:
                                    TimeSpan span = (DateTime.Now) - dt1;
                                    _Logger.LogText("Execution time: " + span.TotalMilliseconds.ToString() + " milliseconds, retried " + i.ToString() + " times.");
                                    _Logger.LogText(EndPointUrl + Action);
                                    if (i >= 3)
                                    {
                                        _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                        _Logger.LogText(EndPointUrl + Action);
                                        throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + _HttpResponseMessage.Content.ReadAsStringAsync().Result);
                                    }
                                    break;
                            }
                            continue;
                        }
                        _Response = _HttpResponseMessage.Content.ReadAsStringAsync().Result;
                        break;
                    }
                }
            }
            return Serializer.JsonSerializer.Deserialize<LoginResponseDTO>(_Response);
        }
        public async Task<T> SendRequestAsync<T>(string EndPointUrl, string Token, string Action, string Payload, HttpMethod Method)
        {
            DateTime dt1 = DateTime.Now;
            string _Response = string.Empty;
            for (int i = 0; i < 4; i++)
            {
                System.Net.ServicePointManager.Expect100Continue = false;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Ssl3;
                using (HttpClient _HttpClient = new HttpClient())
                using (HttpRequestMessage _HttpRequestMessage = new HttpRequestMessage(Method, EndPointUrl + Action))
                {
                    _HttpClient.DefaultRequestHeaders.Clear();
                    _HttpClient.MaxResponseContentBufferSize = 2147483647;
                    _HttpClient.Timeout = TimeSpan.FromMilliseconds(5400000);
                    _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                    _HttpRequestMessage.Content = new StringContent(Payload, Encoding.UTF8, "application/json");
                    using (HttpResponseMessage _HttpResponseMessage = await _HttpClient.SendAsync(_HttpRequestMessage))
                    {
                        if (!_HttpResponseMessage.IsSuccessStatusCode)
                        {
                            switch (_HttpResponseMessage.StatusCode)
                            {
                                case System.Net.HttpStatusCode.BadRequest:
                                    _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + await _HttpResponseMessage.Content.ReadAsStringAsync());
                                    _Logger.LogText(EndPointUrl + Action);
                                    _Logger.LogText(Payload);
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + await _HttpResponseMessage.Content.ReadAsStringAsync());
                                case System.Net.HttpStatusCode.Unauthorized:
                                case System.Net.HttpStatusCode.NotFound:
                                    throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + await _HttpResponseMessage.Content.ReadAsStringAsync());
                                default:
                                    TimeSpan span = (DateTime.Now) - dt1;
                                    _Logger.LogText("Execution time " + span.TotalMilliseconds.ToString() + " milliseconds, retried " + i.ToString() + " times.");// ms.ToString("#,##0.00") + " minutes, retried " + i.ToString("0") + " times");
                                    _Logger.LogText(EndPointUrl + Action);
                                    _Logger.LogText(Payload);
                                    if (i >= 3)
                                    {
                                        _Logger.LogText(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + await _HttpResponseMessage.Content.ReadAsStringAsync());
                                        _Logger.LogText(EndPointUrl + Action);
                                        _Logger.LogText(Payload);
                                        throw new Exception(((int)_HttpResponseMessage.StatusCode).ToString() + " " + _HttpResponseMessage.ReasonPhrase + " " + await _HttpResponseMessage.Content.ReadAsStringAsync());
                                    }
                                    break;
                            }
                            continue;
                        }
                        _Response = _HttpResponseMessage.Content.ReadAsStringAsync().Result;
                        break;
                    }
                }
            }
            return Serializer.JsonSerializer.Deserialize<T>(_Response);
        }
       
    }
}
