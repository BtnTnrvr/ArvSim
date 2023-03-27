using System.Net;
using System.IO;
using System.Threading;
using System;

namespace Sim2.Helper
{
    public class RequestHelper
    {
        public static Log _log;
        public RequestHelper(Log log) 
        {
            _log = log;
        }
        private class WebClient : System.Net.WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                return lWebRequest;
            }
        }
        string HttpGetInternal(string uri)
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ////request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //request.Timeout = Timeout.Infinite;
            //request.KeepAlive = true;

            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //using (Stream stream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(stream))
            //{
            //    return reader.ReadToEnd();
            //}

            string content = null;

            var wc = new WebClient();
            wc.Timeout = 5000;
            content = wc.DownloadString(uri);
            return content;
        }

        public string HttpGet(string uri)
        {
            try
            {
                return HttpGetInternal(uri);
            }
            catch (Exception ex1)
            {
                _log.WriteErrorLog(ex1);
                try
                {
                    return HttpGetInternal(uri);
                }
                catch (Exception ex2)
                {
                    _log.WriteErrorLog(ex2);
                }
                
                return "";
            }
        }
    }
}
