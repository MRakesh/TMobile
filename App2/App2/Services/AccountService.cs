using Android.Webkit;
using App2.Models;
using App2.Models.General;
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
                    //IReadOnlyList<LoginModel>

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
            return "CfDJ8DybItBGHEFJigr0ediXm63X9bIIztxy9ju7qOLbsjYkWTHNZCLYEtIek2s4GkX2KQqzUKSCjU8HqsJX0L9kCm_j9p9NZ4atKcX89gM02ZnjnKGNH94wW7CEkRTuDt0rElzHLYLzafVSxaBUiw6cdYBs6endVt17XYWjg_zLMsT8f49zxDDkOd9mG1rRC50Lqon8C9iUAlwC5JHefOqvm7VHRYHCf2JBnkgH3bXmdj_2gJE8LAgrYdnMOzQkXUlbp_m1RNqAeL91YUltPemxHYkghhldqM65HFgIMXKs0isObmG4Z8kEmUDAmxjMrUYr9wlgosUjmNBGwVfuWUyM_akAega3_6pg6BBsF2Sifzxv1XgKFhhHpYnXTFtSvZqPyvDJFsciv6dFRjmLWWqx3nKiFXXUgmhsHM0ai9SAJX2zF3mmFw5v6RjgGTW3jZ8A8D9H5aVf7H5Es760ytw3gIyRDvdcBx7d86RlFUfrHLJ9RYTO877fTrb-_x_UfZ-fnvGsyhH9jO6fh7JkL8fB4qkc-tUtuLieHTqZ3BuDvSyoKDDN7JYj6Kf_HbT_W-GgumSqA7l-iFUA8wyvxASivbToFl9Fv1T3MCUxUt4sEeUOsfYu-a9ic6nhKXcaQSd5JW56FebHN7J70MwLtJ1xKJdVvl-TjuNezWQokqtlN0ned79PabqbSbrF6X43avhdkIiGmus_C8gDFaSx4G9EjyLzuZMYGlbnwU6hoRyW1PjW7ZkmqdeFj33n1_79OHHuA86p3SooVoja3SXqQ3bVx9LDLKabfLQFwl5OKEh_ziYrdLhei5ie8WNa2eTcmoXwlfScX9kbnOLAf0kXUpb8yaHRbOABMBWG1GzU7Iz-oN9l3mrufv7bHyiwOXOItdzzoJcildKy_N0sGnS_rJf6xJ7fMvYXi29jTAIifEbslEGCjxJ1jJsQDqcFNUDRp3yEm3nG50VroI10g4OsjUxaC_U2czJIhmBXxZVcz1pdNfPJxa8J6BbHHBwXfSZS57iRus28wTK9UQ8jgsEjnxS9SRMxU9feWz39_wgZ0IpuN_D9tThp6vAFKcZdcNlKvf9A3Ss7PbGSqVC3vVxhQOAPXheqBwOPnvxVV-ZwbqTCbbZno454RjeWBdfnEboQFh20YV07uZ2FEjzWSo1PqcTpT-K1QJITQkequy3lRgjgqXKjy2xELdbE7L23QfvQ3jLjoCgRq4CnIpn5s908uQfSXeXBE4z5fDkz3DO8a025ggFIioO7vVd74vKF80liOcg11M01r-47Te_j3jNa0iuOGElXb639WkPNIuxOPsMBjnuARDH-oZ48Dt__tgVnS42CUZpm0OOBpJ27zA-cC8nXkZ9ndlJyfPy7v_vW871D164SBjwuNF7NRSv50QnnDE3xaNDCLOgllTDYRxXCo_uyLiZMCdWOP2QQMY4bZDggz1nBrzy4MkDllMr7urSkdgDEOX_U7y6asgaiyiIwEGlf27I46VZz5r_Smb9DKonUJKdz3re6xKAsG9TVlER5B6m1_rkwWb6WseyIHmjrNm322hQdau5j1OU7TlhuvV-sQ6LQJEElkAeNkoJ_vHjtpx6ACKKuZNS5ihJaU_p3ZqLgyfk";
        }

    }
}
