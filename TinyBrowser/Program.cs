using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace TinyBrowser
{
    class Program
    {
        static void Main(string[] args) {
            
            
            var timeServer = new TcpClient("Acme.com", 80);
            Console.WriteLine("Waiting for connection to establish");
            var stream = timeServer.GetStream();
            var send = "GET / HTTP/1.1\r\nHost: acme.com\r\n\r\n";
            stream.Write(Encoding.ASCII.GetBytes(send,0,send.Length));
            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();
            int startIndex = str.IndexOf("<title>") + 7;
            
            var title = str.Substring(startIndex,str.IndexOf("</title>") - startIndex);
            Console.WriteLine(title);
            timeServer.Close();
            stream.Close();
                
            
        }
    }
}
