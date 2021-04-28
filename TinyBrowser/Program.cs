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
            var send = "Get http://www.Acme.com HTTP/0.9\r\n\r\n";
            stream.Write(Encoding.ASCII.GetBytes(send,0,send.Length));
            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();
            Console.WriteLine(str);
            timeServer.Close();
            stream.Close();
                
            
        }
    }
}
