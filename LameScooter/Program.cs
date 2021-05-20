using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

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
    public class OfflineLameScooterRental : ILameScooterRental {
        public Task<int> GetScooterCountInStation(string stationName) {
            
            var jsonText = File.ReadAllText("scooters.json");
            var stations = JsonSerializer.Deserialize<Stations>(jsonText);
            List<Station> LameScooterStationList = new List<Station>();

            //Console.WriteLine(stations.stations[0]);
            foreach (var jsonElement in stations.stations) {
                var stationInstance = JsonSerializer.Deserialize<Station>(jsonElement.ToString());
                LameScooterStationList.Add(stationInstance);
            }
            foreach (var scooterStation in LameScooterStationList) {
                if (scooterStation.name == stationName) {
                    return Task.FromResult(scooterStation.bikesAvailable);
                }
            }

            throw new NotFoundException($"{stationName} could not be found");
        }
    }

    public class DeprecatedLameScooterRental : ILameScooterRental {
        public Task<int> GetScooterCountInStation(string stationName) {
            
            var text = File.ReadAllText("scooters.txt").TrimEnd( '\r', '\n' );
            
            string[] stations = text.Split(Environment.NewLine);
            
            List<Station> LameScooterStationList = new List<Station>();
            
            foreach (var station in stations) {
                var stationInstance = new Station();
                stationInstance.name = station.Substring(0, station.IndexOf(':') - 1);
                stationInstance.bikesAvailable = int.Parse(station.Substring(station.IndexOf(':') + 1));
                LameScooterStationList.Add(stationInstance);
            }
            foreach (var scooterStation in LameScooterStationList) {
                if (scooterStation.name == stationName) {
                    return Task.FromResult(scooterStation.bikesAvailable);
                }
            }

            throw new NotFoundException($"{stationName} could not be found");
        }
    }
    
    class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string name) : base(name) {
            Console.WriteLine("Station could not be found");
        }
  
    }
    
    class Program
    {
        static async Task Main(string[] args) {

            if (args[0].Any(char.IsDigit)) {
                throw new ArgumentException(" argument string can not contain digits");
            }

            var ScooterStations = new DeprecatedLameScooterRental();
            var amount = await ScooterStations.GetScooterCountInStation(args[0]);
            
            
            Console.WriteLine($"Number of Scooters Available at this Station: {amount}"); // Add the count that is returned above to the output.


            
            
            
        }
    }
}
