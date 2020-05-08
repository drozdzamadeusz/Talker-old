using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Models
{
    public static class SessionManager
    {

        static RestHelper<Token> SessionRestHelper = new RestHelper<Token>();

        static RestHelper<RestStatus> RegisterRestHelper = new RestHelper<RestStatus>();
        static public string RootPageUrl
        {
            get; set;
        }
        static public Token UserToken
        {
            get; set;
        }


        public static async Task<Token> UserLoginAsyncRequest(UserRequest userRequest)
        {
            UserToken = await SessionRestHelper.sendAsyncRequest("auth/login", userRequest);

            return UserToken;
        }

        public static async Task<RestStatus> UserRegisterAsyncRequest(UserRequest userRequest)
        {

            return await RegisterRestHelper.sendAsyncRequest("auth/register", userRequest);

        }
    }
}
