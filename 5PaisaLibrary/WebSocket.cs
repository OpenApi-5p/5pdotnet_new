using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using Websocket.Client;

namespace _5PaisaLibrary
{
    public class WebSocket : IWebsocket

    {
        ManualResetEvent receivedEvent = new ManualResetEvent(false);
        int receivedCount = 0;
        WebsocketClient _ws;
        string _url = "wss://Openfeed.5paisa.com/Feeds/api/chat?Value1=";
        public event EventHandler<MessageEventArgs> MessageReceived;

        public WebSocket()
        {

        }
        public void Close(bool Abort = false)
        {
            if (_ws.IsRunning)
            {
                if (Abort)
                    _ws.Stop(WebSocketCloseStatus.NormalClosure, "Close");
                else
                {
                    _ws.Dispose();
                }
            }
        }

       

        public void FetchMarketFeed(WebsocketConnect list)
        {
            var dataStringSession = JsonConvert.SerializeObject(new
            {

                    MarketFeedData = list.WebsokectMarketFeedData,

                    Method = list.Method,
                    Operation = list.Operation,
                    ClientCode=list.ClientCode,
            }) ;

           

            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(dataStringSession);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public bool IsConnected()
        {
            if (_ws is null)
                return false;

            return _ws.IsStarted;
        }
        public void Receive(string Message)
        {
            MessageEventArgs args = new MessageEventArgs();
            //args.Message = Helpers.DecodeBase64(Message);
            args.Message = Message;
            EventHandler<MessageEventArgs> handler = MessageReceived;
            if (handler != null)
            {
                handler(this, args);
            }
            receivedCount++;
            if (receivedCount >= 10)
                receivedEvent.Set();
        }
        public void Send(string Message)
        {
            if (_ws.IsStarted)
            {
                try
                {
                    _ws.Send(Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void ConnectForFeed(string jwttoken, string clientcode)
        {
            string finalurl = _url  + jwttoken +"|" + clientcode;
            var url = new Uri(finalurl);

            _ws = new WebsocketClient(url);

            _ws.MessageReceived.Subscribe(msg => Receive(msg.Text));

            _ws.Start();
        }

        public class MessageEventArgs : EventArgs
        {
            public string Message { get; set; }
        }
    }
}
