﻿using App2.Models;
using System.Threading.Tasks;

namespace App2.Services
{
    public interface IAccountService
    {
        bool IsCoockiExists();
         Task<AngUserStatusBase> GetTokenForLogin(LoginModel input);
        Task<string> GetResponseFromAPI(string url);
    }
}