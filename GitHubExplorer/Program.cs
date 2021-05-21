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
                

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://api.github.com/users/{username}"));
                

                var response = client.Send(request);
                string contents =  await response.Content.ReadAsStringAsync();
                
                //Console.WriteLine(contents);

                var profile = JsonSerializer.Deserialize<UserResponse>(contents);
                PropertyInfo[] props = Type.GetType("GitHubExplorer.UserResponse").GetProperties();
                

                foreach (PropertyInfo property in props) {
                    if (property.GetValue(profile) == null) {
                        continue;
                    }
                   
                    Console.WriteLine($"{property.Name} {property.GetValue(profile)}");
                    Console.WriteLine();
                    
                }
                //cd Documents\GitHub\gp20-2021-0426-rest-gameserver-EmilHuzell\GitHubExplorer
                
                //Console.WriteLine(String.Empty);
                //Console.WriteLine("Name: " + user["location"]);
                //Console.WriteLine("Location: " + profile.location);
                //Console.WriteLine($"{profile.organizations_url}");
                
            }
        }
    }
}
