using System;
using System.IO;
using System.Net;

namespace DragonFiesta.Util
{
    public static class HttpHelper
    {
         public static string GetExternIp()
         {
             String direction = "";
             WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
             using (WebResponse response = request.GetResponse())
             using (StreamReader stream = new StreamReader(response.GetResponseStream()))
             {
                 direction = stream.ReadToEnd();
             }

             //Search for the ip in the html
             int first = direction.IndexOf("Address: ", StringComparison.Ordinal) + 9;
             int last = direction.LastIndexOf("</body>", StringComparison.Ordinal);
             direction = direction.Substring(first, last - first);


             Logs.Network.DebugFormat("Extern IP: {0}", direction);
             return direction;
         }
    }
}