// © 2021 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using IdentityModel.Client;
using System;

namespace POCRenderingHostAPI.Models
{
    public class TokenResult
    {
        public bool IsError { get; set; }
        public Exception Exception { get; set; }
        public int ExpiresIn { get; set; }
        public string AccessToken { get; set; }
        public string Error { get; set; }

        public TokenResult()
        {
        }

        public TokenResult(TokenResponse tokenResponse)
        {
            IsError = tokenResponse.IsError;
            Exception = tokenResponse.Exception;
            ExpiresIn = tokenResponse.ExpiresIn;
            AccessToken = tokenResponse.AccessToken;
            Error = tokenResponse.Error;
        }
    }
}
