using Newtonsoft.Json;
using Plugin.FileUploader;
using Plugin.FileUploader.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Fileload : ContentPage
    {
        private MediaFile _mediaFile;
        public Fileload()
        {
            InitializeComponent();

            takePhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;
                filePath = file.Path;
                paths.Enqueue(filePath);
                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();

                    file.Dispose();
                    return stream;
                });
            };

            pickPhoto.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var _mediaFile = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();


                if (_mediaFile == null)
                    return;

                filePath = _mediaFile.Path;
                paths.Enqueue(filePath);

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = _mediaFile.GetStream();
                    //  file.Dispose();
                    return stream;
                });

                await StoreMedia(_mediaFile);
            };

            takeVideo.Clicked += async (sender, args) =>
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                {
                    Name = "video.mp4",
                    Directory = "DefaultVideos",
                });

                if (file == null)
                    return;
                filePath = file.Path;
                paths.Enqueue(filePath);
                await DisplayAlert("Video Recorded", "Location: " + file.Path, "OK");

                file.Dispose();
            };

            pickVideo.Clicked += async (sender, args) =>
            {
                if (!CrossMedia.Current.IsPickVideoSupported)
                {
                    await DisplayAlert("Videos Not Supported", ":( Permission not granted to videos.", "OK");
                    return;
                }
                var file = await CrossMedia.Current.PickVideoAsync();

                if (file == null)
                    return;
                filePath = file.Path;
                paths.Enqueue(filePath);
                await DisplayAlert("Video Selected", "Location: " + file.Path, "OK");
                file.Dispose();
            };

            CrossFileUploader.Current.FileUploadCompleted += Current_FileUploadCompleted;
            CrossFileUploader.Current.FileUploadError += Current_FileUploadError;
            CrossFileUploader.Current.FileUploadProgress += Current_FileUploadProgress;
        }

        private void Current_FileUploadProgress(object sender, FileUploadProgress e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                progress.Progress = e.Percentage / 100.0f;
            });
        }

        private void Current_FileUploadError(object sender, FileUploadResponse e)
        {
            isBusy = false;
            System.Diagnostics.Debug.WriteLine($"{e.StatusCode} - {e.Message}");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("File Upload", "Upload Failed", "Ok");
                progress.IsVisible = false;
                progress.Progress = 0.0f;
            });
        }

        private void Current_FileUploadCompleted(object sender, FileUploadResponse e)
        {
            isBusy = false;
            System.Diagnostics.Debug.WriteLine($"{e.StatusCode} - {e.Message}");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("File Upload", "Upload Completed", "Ok");
                progress.IsVisible = false;
                progress.Progress = 0.0f;
            });
        }

        void OnUpload(object sender, EventArgs args)
        {
            if (isBusy)
                return;
            isBusy = true;
            progress.IsVisible = true;


            //Uploading multiple images at once

            /*List<FilePathItem> pathItems = new List<FilePathItem>();
            while (paths.Count > 0)
            {
                pathItems.Add(new FilePathItem("image",paths.Dequeue()));
            }*/

            //CrossFileUploader.Current.UploadFileAsync("<URL HERE>", new FilePathItem("<FIELD NAME HERE>", filePath), new Dictionary<string, string>()
            //{
            //    /*<HEADERS HERE>*/
            //});

        }


        async Task StoreMedia(MediaFile _mediaFile1)
        {
            try
            {
                var content = new MultipartFormDataContent();

                content.Add(new StreamContent(_mediaFile1.GetStream()),
                    "\"file\"",
                    $"\"{_mediaFile1.Path}\"");

                var httpClient = new HttpClient();

                var uploadServiceBaseAddress = "https://thisishire.com/api/homepage/homejobs?skillName=undefined&location=undefined&offset=0";
                //https://thisishire.com/api/candidate/uplaodfile";
                //"http://localhost:12214/api/Files/Upload";

                var httpResponseMessage = await httpClient.GetStringAsync(uploadServiceBaseAddress);
                var tr = JsonConvert.DeserializeObject<APIListResponeInfo<Jobs>>(httpResponseMessage);
                var inst = tr.result.jobs[2];
                lbl.Text = $"{inst.state} {inst.city} {inst.id}";
                ////var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);

                ////string message = await httpResponseMessage.Content.ReadAsStringAsync();
                ////lbl1.Text = message;
                ////txtFileName1.Text = message;
                ////string[] msgList = message.Replace("{", "").Replace("}", "").Split(',');
                ////var filePathItem = JsonConvert.DeserializeObject<FilePathItem>(message);
                ////lbl.Text = filePathItem.result;
                ////txtFileName2.Text = msgList[0].Split(':')[1];
            }
            catch(Exception ex)
            {
                lbl1.Text = ex.Message;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        Queue<string> paths = new Queue<string>();
        string filePath = string.Empty;
        bool isBusy = false;


    }
}

class Jobs
{
    public string state { get; set; }
    public string location { get; set; }
    public int id { get; set; }
    public string title { get; set; }
    public string city { get; set; }
    public DateTime posted { get; set; }
}

class APIListResultInfo<T> where T : class
{
    public List<T> jobs { get; set; }
}

class APIResultInfo<T> where T : class
{
    public T jobs { get; set; }
}

#region These are common for all


class BasePath
{
    public string targetUrl { get; }
    public bool success { get; }
    public string error { get; }
    public bool unAuthorizedRequest { get; }
    public bool __abp { get; }
}
class APIListResponeInfo<T> : BasePath where T : class
{
    public APIListResultInfo<T> result { get; set; }
}
class APIResponeInfo<T> : BasePath where T : class
{
    public APIResultInfo<T> result { get; set; }
}

#endregion

public class FilePathItem
{
    public string result { get; }    
}

