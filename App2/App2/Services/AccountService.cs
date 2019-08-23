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
            return ".AspNetCore.ApplicationCookie=CfDJ8DybItBGHEFJigr0ediXm60xPpNwkWsQjikio3kGbfryuuicoY4mAdWIHQoyD-hzAJG0SgbweDRDCnHV01jof80Nr7YK-n8k9owC5eBJwm_-7PgFy-VLBsT6pAFQuARFsn1Jg9AV2dB82UtN195PhVTszhpWqCtHVGmQPICcFL9dGGEvL6Je9cLam6T8Fw1lF3UN_TbsaWZvokQnu6GW9FNOCV6vZJUGv_v65Y3g7V992p9lrDHuOms08BMpgNutXxDtfWHBmpueS4Z7iXWPtpaUKhl0sSZeykYqz_U1uQyEP5fwvGHTtQ_OzqGSaXJFRKp2IKtnFxRz1vAFmwYhcm89GzmZcnGZcs1nJzKCl8rVMBjWRnUK6s1mdtEIUZsWMH907P3ty-fEc9a1QXUEmFveV0FdWDXxuEhqaZVMLTMGsx9lY_vXiXNbD3LhwhDvLGUzjbqyXFVTynuA58Sc2K5f3tdZ82T8F52dVROFFlmLfZfi4SjzOOLJAIR2xunz8J_jGJ4Y0jroQh5P13gz3L45GYWLMtS1wQbkYgMu__X4h90a5JuXqJVMqwYG7YV-0Ap3DgUXrzqvk93vaecUdQbNL8SQNjMqt9EB1LTZrSyJtUyVbp9An2Dz_IrixYpDe9JJTxm-oMgQCh1RsAyhpSQBWe-Qkh5Y1oRsZAk5P3zd9sRZASUyXs1DE5XGI5CUzk6NZfWCcMC9_D5__RJdl2jdfksVEBXxciSU8_Fm8EqGC3oSuXzL1EbxmMN1gzAsCFEcbXOqm3NUdOCsBBpRw5WEFU3_a2wQKoOoJeIWCID0Z9-FOtbnDTUVCJnKqKgInpQEQChO7b7VIlu-p6_FxN5DExjyEmlg6_W7OiIC1CXXwaNLtVmy-8HJN5z_1fkwnQTb8AK1igeRSlP8Q0U2LagoBJKhuEYJU97gw5N3j7QIInWgJCgqSZwon142jIWjHCpEMZeny1g2a6RG0XVXwP5sjAf_agAQWFQqr8oKILppu9pRRbCyWT-Q5on2FBtB0BGBGITXHB2bkMtWTgoTH5BL9m2i_X24QJfuHyZ5JiumU62iyrgHRsUdyNtvaOE-0WxmMV5Fodqb1CL08tyLAxQ5g-xX1hxFwdMC-HgqlHdKxdZYN7kGip3Q7a4emxaaOXSZ97UUkPhiulfzkhCKPMqy0RygV2jeYAvWnpISVXt2KiHyWCtI3t_YKjlhreaI-RQ1bLZaxP31W5S-adJEO_UUrvDwpSl8xAk0bDFBlRQGbCPIyNLbwVk1IMCOt9AKjcxZq1bfrC0uBEYH5c9Ly1LQR83IQHijlqHitrXFgpPGq5VFfSoxj8ijARxVQciFdR0hbE2grVfNOo3g-ItyjEKYkw1Kt9mmBDcETjzuyAq5PaIAw2VwN43H4bd_GVxctDtWCll80BcH0x0t-qJQaL7zHFqYwVHble8EvYWuDv_eg1Q-KAJdqdTZyWCdHfVsmwvAvM4ZfNoof5SySwhEHMcM-qzfkjpbZ_zas2svG5lQPRvvdundtacIL12eJ5r8Tyq5nI7K4eHXc52DN6l_wYFQrKvqivA5yW2Cgh2aga8zwpI_O4cyAvNp9dzkRDcqOUGQmFvEB2_LJgWYpOZ7kYp05e-OYYZqS1KQlx_R_UDg";
        }

    }
}
