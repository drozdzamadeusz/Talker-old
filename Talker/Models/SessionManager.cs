using System.Threading.Tasks;

using Talker.Models.DTO;
using Talker.Models.DTO.Requests;

namespace Talker.Models
{

    /// <summary>
    /// 
    /// User session credentials manager.
    /// Bearer token manager - stores authorize information about currently logged user.
    /// It is not required to store the user's password while the program is running.
    /// So it is not possible to get user's password while the user uses pragram.
    /// Only bearer is userd to verify user.
    /// 
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// 
        /// Instance of RestHelper used for loging user into the system
        /// 
        /// </summary>
        static readonly RestHelper<Token> SessionRestHelper = new RestHelper<Token>();

        /// <summary>
        /// 
        /// Instance of RestHelper used for registering a new user into the system
        /// 
        /// </summary>
        static readonly RestHelper<RestStatus> RegisterRestHelper = new RestHelper<RestStatus>();

        /// <summary>
        /// 
        /// Contains URL of root login page.
        /// 
        /// </summary>
        static public string RootPageUrl
        {
            get; set;
        }

        /// <summary>
        /// 
        /// Contains URL user authorize token
        /// 
        /// </summary>
        static public Token UserToken
        {
            get; set;
        }

        /// <summary>
        /// 
        /// Asynchronous method used to register a new user to talker.
        /// 
        /// </summary>
        /// <param name="userRequest">UserRequest object with new user data.</param>
        public static async Task<Token> UserLoginAsyncRequest(UserRequest userRequest)
        {
            UserToken = await SessionRestHelper.sendAsyncRequest("auth/login", userRequest);

            return UserToken;
        }

        /// <summary>
        /// 
        /// Asynchronous method used to login to the system using User credentials
        /// 
        /// </summary>
        /// <param name="userRequest">User Request object with user credentials - login and password</param>
        public static async Task<RestStatus> UserRegisterAsyncRequest(UserRequest userRequest)
        {

            return await RegisterRestHelper.sendAsyncRequest("auth/register", userRequest);

        }
    }
}
