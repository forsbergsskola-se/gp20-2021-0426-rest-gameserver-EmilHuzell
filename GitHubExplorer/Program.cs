using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubExplorer
{
    public class UserResponse {
        public string name{get;set;} 
        public string job {get; set;}
        public string location {get; set;}
        public string company { get; set;}
        public string organizations_url { get; set;}
    }
    public class OrgaizationResponse {
        
        public string description { get; set; }
    }

    
    class Program
    {
        private static string token = "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";
        static async Task Main(string[] args) {
            //e
            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            
            while(true)
            {
                Dictionary<string, string> user = new Dictionary<string, string>();
                Console.WriteLine("Submit a username you'd like to explore");
            
                string username = Console.ReadLine();
                Console.WriteLine(String.Empty);
                

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://api.github.com/users/{username}"));
                

                var response = client.Send(request);
                string contents =  await response.Content.ReadAsStringAsync();
                
                //Console.WriteLine(contents);

                var profile = JsonSerializer.Deserialize<UserResponse>(contents);
                //Console.WriteLine(profile.name);
                
                PropertyInfo[] props = Type.GetType("GitHubExplorer.UserResponse").GetProperties();

                //string organizationLink;

                foreach (PropertyInfo property in props) {
                    if (property.GetValue(profile) == null) {
                        continue;
                    }
                    Console.WriteLine($"{property.Name}: {property.GetValue(profile)}");
                }
            }
        }
    }
}
