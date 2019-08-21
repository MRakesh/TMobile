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
            return ".AspNetCore.ApplicationCookie=CfDJ8DybItBGHEFJigr0ediXm60k-nTAQ1FS9MQfAf_IZXjPLvYTH2XPqKC5iAOp3-gh-iBLsg18SjL1khhA1OleBr9uCj7iM84HKyW8rNWgBVhW0P7WqoGT_bKkpuw3XFndU_zCXKFrwU8g3bDvmjdiTl85uKFQDEEf57FyL1yrDko_LY4yyw1iCsf1g3XrI52Ed9lLRfC4sS7oeoY-WZsszB0E59l-eUU0J3jWtC--BaZeaZMs0NyiNn-RsrNSK02tpChC6fL5762G7Sew2iOQjfPJhe_4HL9HQhLBAZ-XUJaheyqOQO_C_OEcDpVkTXyfEVH9XvW5IM2ZqKQ1XCNzR4qmwxlzhi1f55UL5pNwoXli7K3ie8y-PbSxFR6HGV2HAVVaoOI7J3dqR4GKQSpXYjzzwXdonlvWlNAlo8OLbidvVyf_HV3Ru9AOshmQNX4TLXOOLujDd4pzo9xHRXwbuB_q__SPEeZEQAb8ysz-D-9yTT7_q5QLu4IbnbZ0x6uFJhreUhkuDZUTiBWLwRKtcTylnrfwwsU8tCUFkYex5mWDU0BQzoq3RDULDPMD0HrabMvr0pAPA7Q0DAWS45zmHIkBhVEnTExkkPbz7yatsULtN59ZlfatKgRTlwXVCgk3eAsbND2fL6OBH20xAUQf2POB1SlPdVOyWcOi3t7BlZhKletjIKEavpJl7IaM9bxa3TA09_TeXLm8jzDur08K7BrxCQ3t9s_mNsYCoCgSTmk_5W2a9o_Oh-TwuIBopmiWbnUwGV2mj4_RmPFRPcZl76dGeQ0v0WRduEN9HoYp9WatxXHxGhhkmHLSFAMs2haKp5cnxRKybzHF-ahvECLZweCWhIMHV_BWGLqamxxBCxyd3QvxRHRwlaoWwgsYg9IBoZmuP_mHOMKtmpi_fRcVI2Pto6ykKfD9gLpbgoaH29SG5lsb6FLu0nrOwlE3gWDLkLI2IIQtDd28xTAlzSRaffO5FcDyQXKMGWKborgOgQ30rp_Z-l6DhW9syBj9dYBMi7mRsVkIjTuDf0QRFQ8m51aSefkqNgUep1ZljwQAJ9BXS-5Ma667adWtSCM4Sd8_bgpETEM_z9WFOi9WP2z5KyxeifTzHHpDmcY_h25TXt6amMM83hOOCTHCpBd2gBVQ1hxJbdS6w5Y298vRUWnrz9HGHCuV1ULEUA30tkk57yVU0TJUfMIkoDXsI5Zqcn9otceQdxA-IMDVAGAkrYWupjBIVzbyhTK9l67VOnWhZSPWlCfaDEJAeGDcRJmI1HBZ2_xNSdomaeLQHJT-9UC864Vnyg-pdPtVyyrvuKk7r5s1cQuKAEawJMZocj3JpawLgWq0XbpM-1rZwiRxTWNQVg_akvVrMjqqo-UCrSBzuqSdBivUr5uKD6rYZhcvmFqJjRFflIhsrSvIh7z0LTL2nGbA8VRw6Urn6Y71WWZLB2a7HdOX3i6LEZ2w9v-aXvzGc3zFxy83YmTQ1LMU11bCCb5UP5S08mPaUteGk4CqhF__AIctpWek1Bbv5xmn6zVp3eYcCGd9Hkm3r6US4aStB1OnbCN9UaIIjk2MBheoqmIUAZJ6CF9g18l7Pn9JrrtCFayz2xyLovc7DsRS2OAUkyNlKoefM4qotxX4iCxAxfz0-xD49k5L-_SEA3RTah6exQ";
        }

    }
}
