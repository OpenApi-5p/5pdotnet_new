using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Diagnostics;
using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Dynamic;

namespace _5PaisaLibrary
{
    public class _5PaisaAPI
    {
        private string _root = "https://Openapi.5paisa.com/VendorsAPI/Service1.svc/";
        private string _apiKey;
        private string EncryptionKey;
        private string encryptUserId;
        Token Token { get; set; }
        public _5PaisaAPI(string apiKey, string encryptionKey, string encryptUserId,string Root = null)
        {

            _apiKey = apiKey;
            EncryptionKey = encryptionKey;
            this.encryptUserId = encryptUserId;
           
        }
        /* Makes a POST request */
        private string POSTWebRequest(Token agr, string URL, string Data)
        {
            try
            {
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + agr.AccessToken);
                httpWebRequest.Headers.Add("5Paisa-API-Uid", "nosniff");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                byte[] byteArray = Encoding.UTF8.GetBytes(Data);
                httpWebRequest.ContentLength = byteArray.Length;
                string Json = "";

                Stream dataStream = httpWebRequest.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                WebResponse response = httpWebRequest.GetResponse();

                using (dataStream = response.GetResponseStream())
                {

                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "PostError:" + ex.Message;
            }
        }

        private string GETWebRequest(Token agr, string URL)
        {
            try
            {
                //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                HttpWebRequest httpWebRequest = null;
                httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                if (agr != null)
                    httpWebRequest.Headers.Add("Authorization", "Bearer " + agr.AccessToken);
                httpWebRequest.Headers.Add("5Paisa-API-Uid", "nosniff");
                //httpWebRequest.Headers.Add("X-UserType", USER);
                //httpWebRequest.Headers.Add("X-SourceID", SourceID);
                //httpWebRequest.Headers.Add("X-ClientLocalIP", ClientLocalIP);
                //httpWebRequest.Headers.Add("X-ClientPublicIP", ClientPublicIP);
                //httpWebRequest.Headers.Add("X-MACAddress", MACAddress);
                //httpWebRequest.Headers.Add("X-PrivateKey", PrivateKey);
                httpWebRequest.Method = "GET";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Accept = "application/json";

                string Json = "";
                WebResponse response = httpWebRequest.GetResponse();
                // Display the status.
                //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

                // Get the stream containing content returned by the server.
                // The using block ensures the stream is automatically closed.
                using (Stream dataStream = response.GetResponseStream())
                {
                    // Open the stream using a StreamReader for easy access.
                    StreamReader reader = new StreamReader(dataStream);
                    // Read the content.
                    Json = reader.ReadToEnd();
                }
                return Json;
            }
            catch (Exception ex)
            {
                return "GetError:" + ex.Message;
            }
        }

        #region OuthLogin
        public OutputBaseClass GetOuthLogin(string RequestToken)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                TokenResponse agr = new TokenResponse();
                string URL = _root + "GetAccessToken";
                var dataStringSession = JsonConvert.SerializeObject(new
                {
                    head = new { Key = _apiKey },
                    //body = new { ClientCode= ClientCode, JWTToken = Token, Key= VendorKey, AllowMap = Allowmap }
                    body = new { RequestToken = RequestToken, EncryKey = EncryptionKey, UserId = encryptUserId }

                });
                var json = POSTWebRequest(null, URL, dataStringSession);
                agr = JsonConvert.DeserializeObject<TokenResponse>(json);
                if (agr.body.Status == "0")
                {

                    res.TokenResponse = agr.body;
                    res.status = agr.body.Status;
                    res.http_error = agr.body.Message;
                    res.http_code = agr.errorcode;
                    this.Token = agr.body;
                }
                else
                {

                    res.status = agr.body.Status;
                    res.http_error = agr.body.Message;
                }
            }
            catch (Exception ex)
            {

                res.http_error = ex.Message.ToString();
            }
            return res;
        }
        #endregion

        #region TOTPLogin
        public OutputBaseClass TOTPLogin(string _EmailId,string _TOTP,string _Pin)

        {
            OutputBaseClass res = new OutputBaseClass();


            try
            {
                TokenResponse agr = new TokenResponse();
                string URL = _root + "TOTPLogin";
                var dataStringSession = JsonConvert.SerializeObject(new
                {
                    head = new { Key = _apiKey },
                    body = new { Email_ID = _EmailId, TOTP = _TOTP, PIN = _Pin }

                });
                var json = POSTWebRequest(null, URL, dataStringSession);
                agr = JsonConvert.DeserializeObject<TokenResponse>(json);
                if (agr.body.Status == "0")
                {
                    res.TokenResponse = agr.body;
                    res.status = agr.body.Status;
                    res.http_error = agr.body.Message;
                    this.Token = agr.body;
                }
                else
                {
                    res.status = agr.body.Status;
                    res.http_error = agr.body.Message;

                }
            }
            catch (Exception ex)
            {
                //res.status = false;

                res.http_error = ex.Message;
            }
            return res;
        }
        #endregion

        #region PlaceOrder/ModifyOrder/CancelOrder

        public OutputBaseClass placeOrder(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();
            
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/PlaceOrderRequest";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                Exchange = order.Exchange,
                                ExchangeType = order.ExchangeType,
                                ScripCode = order.ScripCode,
                                ScripData = order.ScripData,
                                Price = order.Price,
                                OrderType = order.OrderType,
                                Qty = order.Qty,
                                DisQty = order.DisQty,
                                StopLossPrice = order.StopLossPrice,
                                IsIntraday = order.IsIntraday,
                                iOrderValidity = order.iOrderValidity,
                                AppSource = order.AppSource,
                                RemoteOrderID = order.RemoteOrderID

                            }

                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                        if (pres.body.Status=="0")
                        {
                           // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.PlaceOrderResponse = pres;
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                           
                        }
                        else
                        {
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status ="-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                //res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        public OutputBaseClass ModifyOrder(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/ModifyOrderRequest";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                
                                Price = order.Price==null?0: order.Price,
                                Qty = order.Qty==null?0:order.Qty,
                                StopLossPrice = order.StopLossPrice==null?0:order.StopLossPrice,
                                ExchOrderID=order.ExchOrderID
                            }

                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                        if (pres.body.Status == "0")
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.PlaceOrderResponse = pres;
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "false";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                res.status = "-1";
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        public OutputBaseClass CancelOrder(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/CancelOrderRequest";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                ExchOrderID = order.ExchOrderID,

                            }

                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                        if (pres.body.Status == "0")
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.PlaceOrderResponse = pres;
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                        }
                        else
                        {
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        #endregion

        #region TradeBook
        public OutputBaseClass TradeBook(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/TradeBook";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                ClientCode = order.ClientCode,

                            }
                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        TradeBookResponse pres = JsonConvert.DeserializeObject<TradeBookResponse>(Json);
                        if (pres.body.status == "0")
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.TradeBook = pres;
                            res.status = pres.body.status;
                            res.http_error = pres.body.message;

                        }
                        else
                        {
                            res.status = pres.body.status;
                            res.http_error = pres.body.message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        #endregion

        #region TradeHistory
        public OutputBaseClass TradeHistory(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/TradeHistory";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                ClientCode = order.ClientCode,
                                ExchOrderIDs = order.ExchOrderList

                            }

                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        TradeHistoryResponse pres = JsonConvert.DeserializeObject<TradeHistoryResponse>(Json);
                        if (pres.body.Status == "0")
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.TradeHistory = pres;
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                res.http_error = ex.Message;
            }
            return res;

        }
        #endregion

        #region OrderBook  
        public OutputBaseClass OrderBook(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V3/OrderBook";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                ClientCode = order.ClientCode,
                            }

                        });
                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        OrderBookResponse pres = JsonConvert.DeserializeObject<OrderBookResponse>(Json);
                        if (pres.body.Status == "0")
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.OrderBook = pres;
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = pres.body.Status;
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
               // res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        #endregion

        #region HistoricalData
        public OutputBaseClass historical(string Exch,string ExchType,int scripcode,string Day,DateTime fromDate,DateTime ToDate)
        {

            OutputBaseClass res = new OutputBaseClass();
            dynamic obj = new ExpandoObject();


            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        //string URL = "https://openapi.5paisa.com/V2/historical/N/C/1630/1d?from=2023-02-23&end=2023-04-23";
                      string URL = "https://openapi.5paisa.com/" + "V2/historical"+"/"+ Exch+"/"+"/"+ ExchType +"/"+ scripcode + "/" + Day+"?"+"from"+"="+fromDate+"&"+ "end"+"="+ ToDate;

                       string Result = GETWebRequest(Token,URL);
                       var obj1 = JsonConvert.DeserializeObject<dynamic>(Result);
                      // obj = obj1.data.candles;
                        res.HistoricalData= obj1.data.candles;
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                // res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }
        #endregion

        #region ScripMaster
        public string ScripMaster(string Segment)
        {
            string ScripOut =null;
            try
            {

                string URL = _root + "ScripMaster/segment/" + Segment;
                ScripOut = GETWebRequest(Token, URL);
             
            }
            catch (Exception ex)
            {
                ScripOut = ex.Message;
            }
            return ScripOut;
          
        }

        #endregion

        #region MarketFeed
        public OutputBaseClass MarketFeed(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/MarketFeed";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                MarketFeedData = order.MarketFeedData,

                                LastRequestTime = order.LastRequestTime,
                                RefreshRate = order.RefreshRate
                            }

                        }); ;
                    
                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        MarketFeedResponse pres = JsonConvert.DeserializeObject<MarketFeedResponse>(Json);
                        if (pres.body.Status == 0)
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.MarketFeed = pres;
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                // res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }
        #endregion

        #region NetPositionNetWise
        public OutputBaseClass NetPositionNetWise(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V2/NetPositionNetWise";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                ClientCode = order.ClientCode
                    }

                        }); 

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        NetPositionNetWiseRes pres = JsonConvert.DeserializeObject<NetPositionNetWiseRes>(Json);
                        if (pres.body.Status == 0)
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.NetPositionNetWise = pres;
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                // res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        #endregion

        #region MultiOrderMargin
        public OutputBaseClass MultiOrderMargin(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "MultiOrderMargin";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },       //MultiOrderMargin
                            body = new
                            {
                                ClientCode = order.ClientCode,
                                CoverPositions = order.CoverPositions,
                                Orders = order.Orders

                            }
                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        MultiOrderMarginRes pres = JsonConvert.DeserializeObject<MultiOrderMarginRes>(Json);

                        if (pres.body.Status == 0)
                        {
                            res.MultiOrderMargin = pres;
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;
                        }
                        else
                        {
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not valid";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                res.http_error = ex.Message;
            }
            return res;
        }
        #endregion

        #region NetPositionNetWiseV3
        public OutputBaseClass NetPositionNetWiseV3(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V3/NetPositionNetWise";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                ClientCode = order.ClientCode
                            }

                        });

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        NetPositionNetWiseV3Res pres = JsonConvert.DeserializeObject<NetPositionNetWiseV3Res>(Json);
                        if (pres.body.Status == 0)
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.NetPositionNetWiseV3 = pres;
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                // res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }

        #endregion

        #region MarketSnapshotV1
        public OutputBaseClass MarketSnapshot(OrderInfo order)
        {
            OutputBaseClass res = new OutputBaseClass();

            res.http_code = "200";
            try
            {
                Token Token = this.Token;
                if (Token != null)
                {
                    if (ValidateToken(Token))
                    {
                        string URL = _root + "V1/MarketSnapshot";
                        var dataStringSession = JsonConvert.SerializeObject(new
                        {
                            head = new { key = _apiKey },
                            body = new
                            {
                                Data = order.Data,
                                ClientCode = order.ClientCode
                            }

                        }); ;

                        string Json = POSTWebRequest(Token, URL, dataStringSession);
                        MarketSnapshotV1Res pres = JsonConvert.DeserializeObject<MarketSnapshotV1Res>(Json);
                        if (pres.body.Status == 0)
                        {
                            // OrderResponse pres = JsonConvert.DeserializeObject<OrderResponse>(Json);
                            res.MarketSnapshotV1 = pres;
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;

                        }
                        else
                        {
                            res.status = Convert.ToString(pres.body.Status);
                            res.http_error = pres.body.Message;
                            //res.http_error = Json.Replace("PostError:", "");
                        }
                    }
                    else
                    {
                        res.status = "-1";
                        res.http_error = "Token not exist";
                    }
                }
                else
                {
                    res.status = "-1";
                    res.http_error = "Token not exist";
                }
            }
            catch (Exception ex)
            {
                //res.status = false;
                // res.http_code = "404";
                res.http_error = ex.Message;
            }
            return res;

        }
        #endregion


        #region ValidateToken
        private bool ValidateToken(Token token)
        {
            bool result = false;
            if (token != null)
            {
                if (token.AccessToken != "")
                {
                    result = true;
                }
            }
            else
                result = false;

            return result;
        }
        #endregion
    }
}



