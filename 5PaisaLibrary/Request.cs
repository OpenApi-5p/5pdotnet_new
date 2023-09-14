using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5PaisaLibrary
{
    public class ResponseModel
    {
        // public object RequestData { set; get; }
        public object ResponseData { set; get; }
    }
    public class LoginRequestMobileNewbyTOTP
    {
        public LoginRequestMobileNewbyOTP body { get; set; }
        public Head head { get; set; }
    }
    public class LoginRequestMobileNewbyOTP
    {
        private string _Email_id { get; set; }

        private string TOTP { get; set; }
        public string PIN { get; set; }

    }
    public class GetAccessToken
    {
        public AccessToken body { get; set; }
        public Head head { get; set; }
    }
    public class AccessToken
    {
        public string RequestToken { get; set; }
        public string EncryKey { get; set; }
        public string UserId { get; set; }


    }
    public class Head
    {
      
        public string Key { get; set; }
       
    }
   
}
