using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GitHubExplorer
{
    public class UserResponse {
        public string name{get;set;} 
        public string job {get; set;}
    }
    class Program
    {
        public static string token = "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";
        static async Task Main(string[] args) {
            
            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");
            while(true)
            {
                Console.WriteLine("Submit a username you'd like to explore");
            
                string username = Console.ReadLine();
                

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri($"https://api.github.com/users/{username}"));
                

                var response = client.Send(request);
                string contents =  await response.Content.ReadAsStringAsync();

                var profile = JsonSerializer.Deserialize<UserResponse>(contents);
                Console.WriteLine(profile.job + profile.name);
            }
            
            
           
            //Console.WriteLine(response.ToString());
            
        }

        
    }
}
