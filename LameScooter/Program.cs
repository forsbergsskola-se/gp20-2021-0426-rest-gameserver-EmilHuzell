using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace LameScooter
{
    public class Stations {
        public JsonElement[] stations{get;set;}
    }

    public class Station {
        public string name { get; set; }
        public int bikesAvailable { get; set; }
    }
    
    public interface ILameScooterRental
    {
        Task<int> GetScooterCountInStation(string stationName);
    }

    public class LameScooterRental : ILameScooterRental {
        Dictionary<string, int> Stations = new Dictionary<string, int>();
        public LameScooterRental(List<Station> stations) {
           foreach (var station in stations) {
                Stations.Add(jsonElement.GetProperty("name").ToString(),jsonElement.GetProperty("bikesAvailable"));
            }
        }

        public Task<int> GetScooterCountInStation(string stationName) {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://raw.githubusercontent.com/marczaku/GP20-2021-0426-Rest-Gameserver/main/assignments/scooters.json"));
                

            var response = client.Send(request);
            string jsonText =  await response.Content.ReadAsStringAsync();

           // ILameScooterRental rental = new LameScooterRental();
            
            var stations = JsonSerializer.Deserialize<Stations>(jsonText);

            //Console.WriteLine(stations.stations[0]);
            var stationer = new List<Station>();
            foreach (var jsonElement in stations.stations) {
                var stationen = JsonSerializer.Deserialize<Station>(jsonElement.ToString());
                stationer.Add(stationen);
            }

            foreach (var station in stationer) {
                Console.WriteLine($"availableBikes: {station.bikesAvailable} name {station.name}");
            }
            
            
            // Replace with new XXX() later.
            // The await keyword makes sure that we wait for the Task to complete.
            // and makes the result of the task available. Task<int> => int.
            //var count = await rental.GetScooterCountInStation(null); // Replace with command line argument.
            Console.WriteLine("Number of Scooters Available at this Station: "); // Add the count that is returned above to the output.


            
            
            
        }
    }
}
