﻿namespace IdentityServer.Models.Requests
{
    public class LoginRequestModel
    {
        public string UserName  { get; set; }
        public string Password  { get; set; }
        public string ReturnUrl { get; set; }
    }
}
