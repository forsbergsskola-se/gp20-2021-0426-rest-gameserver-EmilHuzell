using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Attributes = new Dictionary<string, string>();
            Content = HTMLString.Substring(HTMLString.IndexOf('>') + 1, HTMLString.LastIndexOf('<') - HTMLString.IndexOf('>') - 1);

            foreach (string word in HTMLString.Split(' ')) {
                if (word.Contains('=')) {
                    try {
                        string key = word.Substring(0, word.IndexOf('='));
                        string value = word.Substring(word.IndexOf('"') + 1 , word.LastIndexOf('"') - word.IndexOf('"') - 1);
                        Attributes.Add(key, value);
                    }
                    catch{
                    }
                }
            }
        }
    }
    public class HtmlParser {
        public static string getHtml(string link) {
            var timeServer = new TcpClient("Acme.com", 80);
            Console.WriteLine("Waiting for connection to establish");
            var stream = timeServer.GetStream();
            Console.WriteLine($"Acme.com{link}");
            var send = $"GET /{link} HTTP/1.1\r\nHost: Acme.com\r\n\r\n";
            stream.Write(Encoding.ASCII.GetBytes(send,0,send.Length));
            var streamReader = new StreamReader(stream);
            
            var html = streamReader.ReadToEnd();
            
            timeServer.Close();
            stream.Close();

            return html;
        }
        public static List<HTMLElement> extractHTMLElements(string HtmlText) {

            List<HTMLElement> HTMLElements = new List<HTMLElement>();
            List<string> htmlTags = new List<string>{"<title","<a"};
            
            foreach (var tag in htmlTags) {
                int i = 0;
                int i2 = 0;
                
                i = HtmlText.IndexOf(tag, StringComparison.Ordinal);
                i2 = HtmlText.IndexOf(tag.Insert(1,"/"), StringComparison.Ordinal) + tag.Length + 2;

                while (i != -1) {
                    HTMLElements.Add(new HTMLElement(HtmlText.Substring(i, i2 - i)));
                    HtmlText = HtmlText.Remove(i, i2 - i);
                    i = HtmlText.IndexOf(tag, StringComparison.Ordinal);
                    i2 = HtmlText.IndexOf(tag.Insert(1,"/"), StringComparison.Ordinal) + tag.Length + 2;
                }
            }
            return HTMLElements;
        }
    }
    class Program {
        static void Main(string[] args) {

            Stack<string> history = new Stack<string>();

            string link = String.Empty;
            
            while (true) {
                string html = HtmlParser.getHtml(link);
                
                
                List<HTMLElement> HTMLELements = HtmlParser.extractHTMLElements(html);
                int index = 0;
                Console.WriteLine(String.Empty);
                foreach (var htmlElement in HTMLELements) {
                
                    if (htmlElement.Attributes.ContainsKey("href")) 
                    {
                        Console.WriteLine($"{index}: {htmlElement.Content} {htmlElement.Attributes["href"]}");
                        index++;
                    }
                    else {
                        Console.WriteLine(htmlElement.Content);
                        Console.WriteLine(String.Empty);
                    }
                } 
                Console.WriteLine(String.Empty);
                Console.WriteLine("chose an index for where you want to go");
                
                var response = Console.ReadLine();

                if (response.Any(char.IsDigit)) {
                    history.Push(link);
                    link = HTMLELements[int.Parse(response) + 1].Attributes["href"];
                }
                else if (response == "b") {
                    link = history.Pop();
                }
                
                
                Console.WriteLine(link);
            }
        }
    }
}
