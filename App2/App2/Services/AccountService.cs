using Android.Webkit;
using App2.Models;
using Newtonsoft.Json;
using Plugin.Media.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2.Services
{
    public class AccountService : IAccountService
    {

        public AccountService()
        {

        }
        public async Task<AngUserStatusBase> GetTokenForLoginAsync(LoginModel input)
        {
            CookieContainer _cookieContainer = new CookieContainer();
            var baseAddress = new Uri("https://thisishire.com");

            ////string url1 = "https://thisishire.com/jobs/detail/4366";
            using (var _Client = new HttpClient(new HttpClientHandler() { CookieContainer = _cookieContainer }) { BaseAddress = baseAddress })
            {
                _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(input);
                //var json = JsonConvert.SerializeObject(new Post { Username = "rakesh.maggidi@itghar.com", Password = "20itghar@", RememberMe = false });
                var userContent = new StringContent(json, Encoding.UTF8, "application/json");
                var content = await _Client.PostAsync("/api/account/login", userContent);

                content.EnsureSuccessStatusCode();

                HttpStatusCode cpde = content.StatusCode;

                if (content.IsSuccessStatusCode)
                {
                    var response = await content.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<AngUserStatusBase>(response);

                    string cookieName = CopyCookies(content, baseAddress, _cookieContainer);
                    Application.Current.Properties["appCookie"] = cookieName;
                    model.targetUrl = cookieName;
                    return model;
                    //return "Login successful....";
                }
                return null; //content.ReasonPhrase;
            }
        }

        public async Task<string> GetResponseFromAPIAsync(string url)
        {
            string cookiNames = Application.Current.Properties["appCookie"].ToString();// GetPrivateCookie(); //

            var baseAddress = new Uri("https://thisishire.com");
            using (var _Client = new HttpClient(new HttpClientHandler() { UseCookies = false }) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Cookie", cookiNames);
                var content = await _Client.SendAsync(message);
                content.EnsureSuccessStatusCode();

                if (content.IsSuccessStatusCode)
                {
                    //IReadOnlyList<LoginModel>

                    return cookiNames;
                    //    return await content.Content.ReadAsStringAsync();
                }
                return content.ReasonPhrase;
            }
        }

        public async Task<string> GetCustomResponseFromAPIAsync(string url)
        {

            string cookiNames = GetPrivateCookie();

            var baseAddress = new Uri("https://thisishire.com");
            using (var _Client = new HttpClient(new HttpClientHandler() { UseCookies = false }) { BaseAddress = baseAddress })
            {
                var message = new HttpRequestMessage(HttpMethod.Get, url);
                message.Headers.Add("Cookie", cookiNames);
                var content = await _Client.SendAsync(message);
                content.EnsureSuccessStatusCode();

                if (content.IsSuccessStatusCode)
                {
                    return await content.Content.ReadAsStringAsync();
                }
                return content.ReasonPhrase;
            }

        }


        public async Task<string> PostDataToAPIAsync(Object input, string url)
        {
            var baseAddress = new Uri("https://thisishire.com");

            ////string url1 = "https://thisishire.com/jobs/detail/4366";
            using (var _Client = new HttpClient() { BaseAddress = baseAddress })
            {
                _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(input);
                var userContent = new StringContent(json, Encoding.UTF8, "application/json");
                var content = await _Client.PostAsync(url, userContent);
                content.EnsureSuccessStatusCode();
                if (content.IsSuccessStatusCode)
                {
                    return await content.Content.ReadAsStringAsync();
                }
                var some = content.ReasonPhrase;
                return content.ReasonPhrase;
            }
        }

        public async Task<string> PostFileDataToAPIAsync(MediaFile _mediaFile1)
        {
            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(_mediaFile1.GetStream()),
                "\"file\"",
                $"\"{_mediaFile1.Path}\"");

            var httpClient = new HttpClient();

            var uploadServiceBaseAddress = "https://thisishire.com/api/candidate/uplaodResumefile";
            //"http://localhost:12214/api/Files/Upload";

            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);

            string message = await httpResponseMessage.Content.ReadAsStringAsync();
            var fileModel = JsonConvert.DeserializeObject<UploadedFileModelBase>(message);
            string fileName = System.IO.Path.GetFileName(_mediaFile1.Path);
            Application.Current.Properties["MsGUID"] = fileModel.result;
            Application.Current.Properties["MsFileName"] = fileName;
            return fileModel.result;
        }
        public bool IsCoockiExists()
        {
            return Application.Current.Properties.ContainsKey("appCookie");
        }

        private string CopyCookies(HttpResponseMessage result, Uri uri, CookieContainer _cookieContainer)
        {
            var _cookieManager = CookieManager.Instance;
            foreach (var header in result.Headers)
                if (header.Key.ToLower() == "set-cookie")
                    foreach (var value in header.Value)
                        _cookieManager.SetCookie($"{uri.Scheme}://{uri.Host}", value);

            var allCookies = GetAllCookies(_cookieContainer);
            string cookiNames = string.Empty;

            foreach (var cookie in allCookies)
            {
                if (cookie.Name.Contains("ApplicationCookie"))
                {
                    _cookieManager.SetCookie(cookie.Domain, cookie.ToString());
                    cookiNames = cookie.ToString();
                }
            }
            return cookiNames;
        }

        private IEnumerable<Cookie> GetAllCookies(CookieContainer cookieContainer)
        {
            var _cookieManager = CookieManager.Instance;
            Hashtable domains = (Hashtable)cookieContainer.GetType().GetField("m_domainTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(cookieContainer);

            IList<Cookie> cookies = new List<Cookie>();

            foreach (DictionaryEntry element in domains)
            {
                SortedList list = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);


                foreach (var element1 in list)
                {
                    var collection = (CookieCollection)((DictionaryEntry)element1).Value;
                    foreach (Cookie cookie in collection)
                    {
                        cookies.Add(cookie);

                    }
                }
            }
            return cookies;
        }


        public string GetPrivateCookie()
        {
            return ".AspNetCore.ApplicationCookie=CfDJ8DybItBGHEFJigr0ediXm617gYf9FLMliEDB60eqgXJXQL8eoV6vnKKiHGwjLKKLxgP3V48Szn43Irc3arBK81Rdc-MiW-nyxddTRTBYd6HwnPyuq5jyr7_W31_VwdXDT2dGCF3YDnaQ7OEqXmHp-cLHE-xu2yDr6t8JmxRGPA3fsmiIq_eKUdtFB-t4WSPLS4AfBGXDA38PVrkkFCEKlFsgeEy9dJkiRsIcfhEpo_7Bt_ovNcg8k3J2K3qiAprInVfTDyCp-WZYqjXTKMf9DBlAMhoEZmYDoMzYVdI2ujUHzoo5JjiIsp9mBygO-tepXgfclpaQB0vfzrUoeiwP64HAb00YO43HYWSA6MkGve3hrcVkVjBhaRGCkFHPeP4MAm0zXx-OhL0GtvKzsrzEbiv5SC2wm1wUtY2laR6DPfc0EXK1h_OfRahU2EMAcpdzK-Hv7gXVtdAizZxdJhzW5__oROmK5fNXKOe7ZXR7xZxpS245sDF6_mY62yb44dZjxQNlfIYbFq7QB2SKxzo_r1Uj17_9k-u64CEz0zp-6Kh0gJJ2fn9MGiR8yOoLpW158ZBKc0CTD_Ao0RuGfUPrxFIyq89d3705KOus-F50becZ1PNQuafe8fvsCFYS58LeoRE-NMft2jo_h_nMY6VGmcYt7fcxEdDxOezmaP775l1BqQQqIj65Cg3CwbsmzvjOvbsf96Jm4ClgAUaWDbZwZ8UhcLnVLRWGieiSRCABFCedqu_gUZti-_v4_qgfSatVsU94Gld18fAHwqrii2AvNvfAUmhcUxVFf6Uyvn3oXysM9Us9pY2LkWIf0yimgz0M8egD-wVx4DAEBdmOsipJSQHI82LMBruCGbbo6MY-kS3SdbvAdmskCp9jGzM8gluYNKOp_7SAjRE8WZTtnjwDX5Oh6KyqdZME5SkPG0KELuDTw13gLe9_6T-gBZo1y1CLzs75P1eUOlM2o29F6gBKmAFWNtO7KVM4Qp8Gy9kE2ym7VnZPxHDyli6FkM00M-8aK0S1YJeWnikT4c4F-qwN5UJCeLPw3qq6p2xz5douSNvkUzCkR29j-3GHr_uiAjw_McW1__Q2fid1_qIPU-RDunkLBQ8rAJWdn6ftySwnlT5LfDyU1J8SdUFTsroUBx_GPQ9eYk_etCQCJ0xjPY1xxOve5ljFh9P9MGyQLvXDUkeROFaJrIsOyCDl1IQFKNMCD1l3gdN-EZIl-J574F2NyB6exWJb92sbKue_azWmgviH2dlpSbhqYgAaKnwglQbkCQlwsSe68Ov_ycbgZsAt6-jkfILhvWbsaG4CO_HCjseTuwV-vSmE_uwangYmz8AcrgCr9Y_ieXpHk9hWiEKvScFyDt2eFgWlnFlyYZ_1gCy3dkKDliseTY5yeddAVR_GrGsePnY51VUt9DEce42rcmSYE8x9VVaPl1xgm6MyJfhaULF4ps6t1CZXFs5XK2BWZd1PAJQLO2aY52CPmJxyUZSVGayw32HLC-tKF8aI5TYv6cugKXenowcoObt_Z7pV_kvD4x8ZWB8mjhovvbCtZ82HzrCByhgZ4ljO-lMLysEZsqR-4f8Fsc8m0Zel__9vJg9ZRIaCMGsl_rly7dADfGaxeIRjlqJrTVPw7Pm8RbYP";
        }

    }
}
