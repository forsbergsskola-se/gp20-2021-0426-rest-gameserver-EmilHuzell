using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace TinyBrowser
{
    public class HTMLElement {

        public string Content;
        public Dictionary<string, string> Attributes;
        public HTMLElement(string HTMLString) {
            //Console.WriteLine(HTMLString);
            Attributes = new Dictionary<string, string>();
                
            Content = HTMLString.Substring(HTMLString.IndexOf('>') + 1, HTMLString.LastIndexOf('<') - HTMLString.IndexOf('>') - 1);
            //Console.WriteLine(HTMLString);
            foreach (string word in HTMLString.Split(' ')) {
                if (word.Contains('=')) {
                    try {
                        string key = word.Substring(0, word.IndexOf('='));
                        string value = word.Substring(word.IndexOf('"') + 1, word.LastIndexOf('"') - word.IndexOf('"') - 1);
                        Attributes.Add(key, value);

                    }
                    catch{
                        
                    }
                        
                    
                }
            }
            
        }
    }
    public static class HtmlParser {
        
        public static string getHtml(string link) {
            var timeServer = new TcpClient("Acme.com", 80);
            Console.WriteLine("Waiting for connection to establish");
            var stream = timeServer.GetStream();
            var send = $"GET / HTTP/1.1\r\nHost: {link}\r\n\r\n";
            stream.Write(Encoding.ASCII.GetBytes(send,0,send.Length));
            var sr = new StreamReader(stream);
            var str = sr.ReadToEnd();
            
            timeServer.Close();
            stream.Close();

            return str;

        }
        
        public static List<HTMLElement> getHTMLElements(string HtmlText) {

            List<HTMLElement> HTMLElements = new List<HTMLElement>();
            List<string> htmlTags = new List<string>{"<title","<a"};
            
            
            foreach (var tag in htmlTags) {
                int i = 0;
                int i2 = 0;
                
                i = HtmlText.IndexOf(tag, StringComparison.Ordinal);
                i2 = HtmlText.IndexOf(tag.Insert(1,"/"), StringComparison.Ordinal) + tag.Length + 2;

                while (i != -1) {
                    //Console.WriteLine(tag);
                
                    
                    //Console.WriteLine(i);
                   // Console.WriteLine(i2);
                    HTMLElements.Add(new HTMLElement(HtmlText.Substring(i, i2 - i)));
                    HtmlText = HtmlText.Remove(i, i2 - i);
                    i = HtmlText.IndexOf(tag, StringComparison.Ordinal);
                    i2 = HtmlText.IndexOf(tag.Insert(1,"/"), StringComparison.Ordinal) + tag.Length + 2;
                }    
                   //string text = HtmlText.Remove(i, i2 - i);

            }
            return HTMLElements;
        }
        
    }
    
    
    class Program {


        

        
        static void Main(string[] args) {







            string link = "Acme.com";
            
            while (true) {
                string html = HtmlParser.getHtml(link);
                List<HTMLElement> HTMLELements = HtmlParser.getHTMLElements(html);
                int index = 0;
                Console.WriteLine(String.Empty);
                foreach (var htmlElement in HTMLELements) {
                
                    if (htmlElement.Attributes.ContainsKey("href")) 
                    {
                        Console.WriteLine($"{index} {htmlElement.Content}");
                        index++;
                    }
                    else {
                        Console.WriteLine(htmlElement.Content);
                        Console.WriteLine(String.Empty);
                    }
                } 
                Console.WriteLine(String.Empty);
                Console.WriteLine("chose an index for where you want to go");
                int response = int.Parse(Console.ReadLine());
                link = HTMLELements[response].Attributes["href"];

            }
            

            
            
            
                
            
        }
        
        
    }
}
