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
    //cd Documents\GitHub\gp20-2021-0426-rest-gameserver-EmilHuzell\TinyBrowser
    public class HTMLElement {

        public string Content;
        public Dictionary<string, string> Attributes;
        public HTMLElement(string HTMLString) {
            Attributes = new Dictionary<string, string>();
            
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
            while (HTMLString.IndexOf('>') != -1 && HTMLString.LastIndexOf('<') > HTMLString.IndexOf('>')) {
                
                // - HTMLString.IndexOf('>') - 1
                HTMLString = HTMLString.Substring(HTMLString.IndexOf('>') + 1, HTMLString.LastIndexOf('<') - HTMLString.IndexOf('>') - 1);
            }
            Content = HTMLString;
        }
    }
    public class HtmlParser {
        public static string getHtml(string URL) {
            

            string host = "Acme.com";
            string path = URL;

            if (URL.Contains(".com")) {
                host = URL.Substring(URL.IndexOf('w'), URL.IndexOf(".com") + 4 - URL.IndexOf('w'));
                path = URL.Substring(URL.IndexOf(".com") + 4);
            }
            Console.WriteLine($"{host} {path}");
            
            var timeServer = new TcpClient(host, 80);
            Console.WriteLine("Waiting for connection to establish");
            var stream = timeServer.GetStream();
            
            
            var send = $"GET /{path} HTTP/1.1\r\nHost: {host}\r\n\r\n";
            stream.Write(Encoding.ASCII.GetBytes(send,0,send.Length));
            var streamReader = new StreamReader(stream);
            
            var html = streamReader.ReadToEnd();
            Console.WriteLine("hej");
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
                    string htmlString = HtmlText.Substring(i, i2 - i);

                    if (!htmlString.Contains("<img")) {
                        HTMLElements.Add(new HTMLElement(htmlString));
                    }
                    
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

            string URL = String.Empty;
            
            
            while (true) {
                string html = HtmlParser.getHtml(URL);
                
                List<HTMLElement> HTMLELements = HtmlParser.extractHTMLElements(html);
                int index = 0;
                Console.WriteLine(String.Empty);
                foreach (var htmlElement in HTMLELements) {
                    if (htmlElement.Attributes.ContainsKey("href")) 
                    {
                        Console.WriteLine($"{index}: {htmlElement.Content} ({htmlElement.Attributes["href"]})");
                        index++;
                    }
                    else {
                        Console.WriteLine(htmlElement.Content);
                        Console.WriteLine(String.Empty);
                    }
                } 
                Console.WriteLine(String.Empty);
                Console.WriteLine("Enter an index for where you want to go, enter b to go back, enter h to view history or enter any other key to refresh");
                
                var response = Console.ReadLine();

                if (response.Any(char.IsDigit)) {
                    history.Push(URL);
                    URL = HTMLELements[int.Parse(response) + 1].Attributes["href"];
                }
                else if (response == "b" && history.Count > 0) {
                    URL = history.Pop();
                }
                else if (response == "h" && history.Count > 0) {
                    foreach (var path in history) {
                        Console.WriteLine(path);
                    }
                }
                
                Console.WriteLine(URL);
            }
        }
    }
}
