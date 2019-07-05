using Android.Webkit;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        private async void BtnGetTokensTest_Clicked(object sender, EventArgs e)
        {
            try
            {
                CookieContainer _cookieContainer = new CookieContainer();
                // System.Net.Http.HttpClient _client = new System.Net.Http.HttpClient(new HttpClientHandler() { CookieContainer = _cookieContainer });
                var baseAddress = new Uri("https://thisishire.com");

                ////string url1 = "https://thisishire.com/jobs/detail/4366";
                using (var _Client = new HttpClient(new HttpClientHandler() { CookieContainer = _cookieContainer }) { BaseAddress = baseAddress })
                {
                    ////    var uri = new Uri(string.Format(url1, string.Empty));
                    ////    var response = await _Client.GetAsync(uri);
                    ////    if (response.IsSuccessStatusCode)
                    ////    {

                    ////        var content = await response.Content.ReadAsStringAsync();
                    ////       await  DisplayAlert("Alert", content, "OK");
                    ////    }
                    ////    else
                    ////    {
                    ////        var di = response;
                    ////       await DisplayAlert("Alert", di.ReasonPhrase, "OK");
                    ////    }
                    ///
                    _Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var json = JsonConvert.SerializeObject(new Post { Username = "rakesh.maggidi@itghar.com", Password = "20itghar@", RememberMe = false });
                    var userContent = new StringContent(json, Encoding.UTF8, "application/json");
                    var content = await _Client.PostAsync("/api/account/login", userContent);


                    if (content.IsSuccessStatusCode)
                    {
                        var Response = content.Content.ReadAsStringAsync().Result;
                        // await DisplayAlert("success status", Response, "OK");
                    }
                    else
                    {
                        var di = content;
                        await DisplayAlert("unsuccess status", di.ReasonPhrase, "OK");
                    }

                    string cookieName = CopyCookies(content, baseAddress, _cookieContainer);
                    Application.Current.Properties["appCookie"] = cookieName;
                    await DisplayAlert("cookie success", "cookie created...", "OK");

                    ////// string uri2 = "https://thisishire.com/api/recruiter/getRecruiterTopicPopUpChats/194";
                    //// //https://thisishire.com/api/recruiter/getRecruiterTopicChats/0/20";
                    ////// var uri1 = new Uri(string.Format(uri2, string.Empty));
                    //// _cookieContainer.Add(baseAddress, new Cookie("Cookie", cookieName));
                    //// var content1 = await _Client.GetAsync("/api/recruiter/getRecruiterTopicPopUpChats/194");

                    //// if (content1.IsSuccessStatusCode)
                    //// {

                    ////     var content11 = await content1.Content.ReadAsStringAsync();
                    ////     await DisplayAlert("after login", content11, "OK");
                    //// }
                    //// else
                    //// {
                    ////     var di = content1;
                    ////     await DisplayAlert("after login", di.ReasonPhrase, "OK");
                    //// }
                }

            }
            catch (Exception ex)
            {
                var exee = ex;
            }
        }

        private async void BtnGetMessages_Clicked(object sender, EventArgs e)
        {
            try
            {

                string cookiNames = Application.Current.Properties["appCookie"].ToString();

                var baseAddress = new Uri("https://thisishire.com");
                using (var _Client = new HttpClient(new HttpClientHandler() { UseCookies = false }) { BaseAddress = baseAddress })
                {
                    var message = new HttpRequestMessage(HttpMethod.Get, "api/recruiter/getRecruiterTopicPopUpChats/194");
                    message.Headers.Add("Cookie", cookiNames);
                    var content1 = await _Client.SendAsync(message);
                    content1.EnsureSuccessStatusCode();

                    if (content1.IsSuccessStatusCode)
                    {

                        var content11 = await content1.Content.ReadAsStringAsync();
                        await DisplayAlert("all messages", content11, "OK");
                    }
                    else
                    {
                        var di = await content1.Content.ReadAsStringAsync();
                        await DisplayAlert("failure data", di, "OK");
                    }
                }

            }
            catch (Exception ex)
            {
                var exee = ex;
            }
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

            // DisplayAlert("list of cookies", cookiNames, "ok");
            return cookiNames;
        }


        ////public void ReverseCopyCookies(Uri uri)
        ////{
        ////    var _cookieManager = CookieManager.Instance;
        ////    var cookie = _cookieManager.GetCookie($"{uri.Scheme}://{uri.Host}");

        ////    if (!string.IsNullOrEmpty(cookie))
        ////        _cookieContainer.SetCookies(new Uri($"{uri.Scheme}://{uri.Host}"), cookie);
        ////}

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

            //var policy = Java.Security.Policy.Handle<InvalidOperationException>()
            //                   .WaitAndRetry(new[] { TimeSpan.FromSeconds(1),
            //                                 TimeSpan.FromSeconds(1),
            //                                 TimeSpan.FromSeconds(1) });

            //// Possibility that the container changed while looping. Since we can't lock the internal field, a simple retry is the only solution.
            //return policy.Execute(() =>
            //{

            //});
        }

        private void BtnOne_Clicked(object sender, EventArgs e)
        {
            string name = "Indian ElephantOne";
            Shell.Current.GoToAsync(new ShellNavigationState("routerOne?name="+ name), true);
        }

        private void BtnTwo_Clicked(object sender, EventArgs e)
        {
            string name = "African ElephantTwo";
            Shell.Current.GoToAsync(new ShellNavigationState("routerTwo?name=" + name + "&id=44"), true);
        }

        private void BtnThree_Clicked(object sender, EventArgs e)
        {
            string name = "African ElephantThree";
            Shell.Current.GoToAsync(new ShellNavigationState("routerThree?name=" + name), true);
        }
    }
}

public class Post
{
    public string Username { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
