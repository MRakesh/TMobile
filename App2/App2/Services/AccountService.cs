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
            string cookiNames = GetPrivateCookie(); //Application.Current.Properties["appCookie"].ToString();

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

        //public async Task<string> GetCustomResponseFromAPIAsync(string url)
        //{

        //    string cookiNames = GetPrivateCookie();

        //    var baseAddress = new Uri("https://thisishire.com");
        //    using (var _Client = new HttpClient(new HttpClientHandler() { UseCookies = false }) { BaseAddress = baseAddress })
        //    {
        //        var message = new HttpRequestMessage(HttpMethod.Get, url);
        //        message.Headers.Add("Cookie", cookiNames);
        //        var content = await _Client.SendAsync(message);
        //        content.EnsureSuccessStatusCode();

        //        if (content.IsSuccessStatusCode)
        //        {
        //            return await content.Content.ReadAsStringAsync();
        //        }
        //        return content.ReasonPhrase;
        //    }

        //}


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
            return ".AspNetCore.ApplicationCookie=CfDJ8DybItBGHEFJigr0ediXm62I5qNSvmzV3pyxy_lx3QRetgkg03mZ6Dij_OY5IiDJ2o--yBG9q8fZOCOn12C6P1nkkI5mqGY5L180aHqGYjMAM646pr-ovVy1QYkejEUheER248Rr2YP2-B-plJVhCl7prg7V3b6HaXkZUHQTHyOERekWVqKcY8Gy35UF12JVKEzYDn3INqht-GtoJpeI3IAgZHfokTLRDAYaa54h0ss0Fdg1rxd1QD-1bqOwd3QtZcpEXAs0C8d_YZqTAWVSImMi77oyUYYzYWXzmFAzaGYQItfbYQg4ShJpyEhv7djkpoH0Br2wTC7khb5uWtGVKbprkxl7n6O72__mSrMLGqC7XZCaODrNQu9g-gpU6Frm7Qzc1TiZjgFY-9wI8URP_d6WRl-ZuwOHmWvgdf3ffuofSSRPzfQIZY5xnmb0PhaWHu_7smfnr0gh5p9ZbQJ0NFRiTGjkznpTh1xa6MDGVbRHHzUlvl0A33O1g_SermiTsm-Q_M-zyMbsR7FaiuwPm8-lKJHppi2whiOkWRYisTcj2M6IZ-9LBMoB-UmJATXp5GxzQiwNzGq0vJjQzBUQf6RFYeOOGkriJSbswkYACugxHa0SZEZMoH02KtDnHF3mFvu-7Rf3vJcPZzamdPyoJSek42UxTWD-jBQqdsz_cFeUBlJx2IFatXg9RttC6QYn9vK8SVeJGENECzq7XU5bBvLUVEf9LUIcjcZc2TC4JIvuz3EbqY3fPj7Ydov2T-SA7Rx4YPiPRN7kiaQIF2o-eM1P50YPQYALzC59Z-_VR4X9ja9zlM5c7N0qRXaOJXlqqvqZbdOrtAXdEyHOwucpICUPitq9aVOw4WNYTiQ-OJ8V8Vbgns9gH7lj92k1qRwAaQymRnkTPdX-prw81g5TeKK2qBXrb6ETQMEErZkECOanp_EDvENbHgvL7fHHMGY09TqaglGY8iI9TvykfUe_5LvH5mgL-29kIdUBlCfrKGRBCrinz-IAwf1b985hLKbZ4Eo5-ZHp9CZU5AUhdanPq_sOG7vNtbFMRAQqfHAfdhNtwe5eRHepXALnP1rzFcUL3fbGFV9wi8hDkliihvfKqcs6hY-bkepvujbScqNI9MHBuU9-60PvffsdZzxNjVWJeX2e-4s_J4S2_QLXv_jLnRE2MvyZyxLANhKQMaKeogdRmcRfSU8-d12sxXwStvGnOF6sOTfRU7MJaVFjOQJ54YaowjsS3O1lOeGqu3fOxRP57GxdSo9YWX91DgQD-jwrxEm_Cu0pPJ4a6lKn-lIIenb1R9UrhpqmngeDwoI1NgraESqkSxajoSFzp6BMdZB0_HqYxt5A1AFmnJLFI8vXLhvk_wP8DpeiAdT-hW0e7qmiNYtY-BxLgTYl4hEfwuZpH8WnYe4bNR6oIXq-Zv6ay9Ale5NUqu88GOhpBujITljBBUFiHYxPEK-b_qTPJ9eb4xY07PU5iXevymXwr9Bf0ZADCwMeQWoZLPuG8qqetciX0Lt-kZfDuUNkqdExFaAAncOud0ui4Mtp6-9dGn1z6vgjyalZlc6lr1rTllRESBgNO8nou0Yp0zVWDtTJSvZqXZhbXgUa--5lLXiMefOnRis3KCvNcPF212iKlg6V0_ai";
        }

    }
}
