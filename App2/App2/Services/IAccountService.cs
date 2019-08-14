using App2.Models;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;

namespace App2.Services
{
    public interface IAccountService
    {
        bool IsCoockiExists();
         Task<AngUserStatusBase> GetTokenForLoginAsync(LoginModel input);
        Task<string> GetResponseFromAPIAsync(string url);
        Task<string> PostDataToAPIAsync(Object input, string url);
        Task<string> PostFileDataToAPIAsync(MediaFile _mediaFile1);
    }
}
