using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static _5PaisaLibrary.Enum;

namespace _5PaisaLibrary
{
    public class APIRequest
    {
        public  string SynchronizeApiConsume(string url, APIType ObjApiType, APIHeaders ObjApiHeader, string ParamString = "", string AuthorizationKey = "", string CustomHeader1 = "", string CustomHeader2 = "", bool ShouldUseLoginCookie = false, bool VendorCall = false, string CustomHeader3 = "", bool ShouldUseJWTToken = false)
        {
            string Result = null;
            try
            {
                if (ObjApiType == APIType.GET)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "GET";
                    Stream objstream;
                    objstream = request.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(objstream);
                    Result = reader.ReadToEnd();
                }
                else if (ObjApiType == APIType.POST)
                {
                    CookieContainer container = new CookieContainer();
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    //Headers Condition
                    if (ObjApiHeader == APIHeaders.Authorization && AuthorizationKey != "")
                    {
                        request.Headers.Add("Authorization", AuthorizationKey);
                    }
                    else if (ObjApiHeader == APIHeaders.Custom && CustomHeader1 != "" && CustomHeader2 != "" && CustomHeader3 != "" && CustomHeader1.Contains(',') && CustomHeader2.Contains(',') && CustomHeader3.Contains(','))
                    {
                        request.Headers.Add(CustomHeader1.Split(',')[0], CustomHeader1.Split(',')[1]);
                        request.Headers.Add(CustomHeader2.Split(',')[0], CustomHeader2.Split(',')[1]);
                        request.Headers.Add(CustomHeader3.Split(',')[0], CustomHeader3.Split(',')[1]);
                    }
                    else if (ObjApiHeader == APIHeaders.Custom && CustomHeader1 != "" && CustomHeader2 != "" && CustomHeader1.Contains(',') && CustomHeader2.Contains(','))
                    {
                        request.Headers.Add(CustomHeader1.Split(',')[0], CustomHeader1.Split(',')[1]);
                        request.Headers.Add(CustomHeader2.Split(',')[0], CustomHeader2.Split(',')[1]);
                    }

                    request.CookieContainer = container;
                    //if (ShouldUseLoginCookie)
                    //{
                    //    CookieContainer requestCookieContainer = new CookieContainer();
                    //    if (HttpContext.Current == null)
                    //    {
                    //        var CookieData = (CookieCollection)(HttpRuntime.Cache["ClientSwarajCookie"]);
                    //        if (Convert.ToString(HttpContext.Current.Session["RegeneratedJwtToken"]) != "")
                    //            CookieData[1].Value = Convert.ToString(HttpContext.Current.Session["RegeneratedJwtToken"]);

                    //        var CookieDataName = (ShouldUseJWTToken ? CookieData[1].Name : CookieData[0].Name);
                    //        var CookieDataVal = (ShouldUseJWTToken ? CookieData[1].Value : CookieData[0].Value);
                    //        Cookie requiredCookie = new Cookie(CookieDataName, CookieDataVal);
                    //        requiredCookie.Domain = (ShouldUseJWTToken ? CookieData[1].Domain : CookieData[0].Domain);
                    //        requestCookieContainer.Add(requiredCookie);
                    //        request.CookieContainer = requestCookieContainer;
                    //    }
                    //    else if (HttpContext.Current.Session["ClientSwarajCookie"] != null)
                    //    {
                    //        var CookieData = (CookieCollection)(HttpContext.Current.Session["ClientSwarajCookie"]);

                    //        if (Convert.ToString(HttpContext.Current.Session["RegeneratedJwtToken"]) != "")
                    //            CookieData[1].Value = Convert.ToString(HttpContext.Current.Session["RegeneratedJwtToken"]);

                    //        var CookieDataName = (ShouldUseJWTToken ? CookieData[1].Name : CookieData[0].Name);
                    //        var CookieDataVal = (ShouldUseJWTToken ? CookieData[1].Value : CookieData[0].Value);
                    //        Cookie requiredCookie = new Cookie(CookieDataName, CookieDataVal);
                    //        requiredCookie.Domain = (ShouldUseJWTToken ? TTAPItokenDomain : CookieData[0].Domain);
                    //        requestCookieContainer.Add(requiredCookie);
                    //        request.CookieContainer = requestCookieContainer;
                    //        if (VendorCall == true)
                    //        {
                    //            request.Headers.Add("Cookie", Convert.ToString(CookieData[0]));
                    //        }
                    //    }
                    //}
                    request.PreAuthenticate = true;
                    byte[] data = Encoding.UTF8.GetBytes(ParamString);
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(data, 0, data.Length);
                    dataStream.Close();
                    //  System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    

                    Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    //if (CustomHeader3 == "" && response.Cookies != null && response.Cookies.Count > 0)
                    //    LoginCookie(response.Cookies);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;

        }

    }
}
