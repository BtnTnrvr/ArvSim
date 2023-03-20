using Newtonsoft.Json;
using Sim2.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Sim2.Helper
{
    public class CalculateDeserializedHelper
    {
        public static List<PacketModel> GetAllModelFromFile(string fileContents)
        {
            var allModel = new List<PacketModel>();
            var lines = fileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                try
                {
                    dynamic data = JsonConvert.DeserializeObject(line);
                    
                    if (data.ContainsKey("DATA")) // Checking the dynamic object has a "DATA" so it can be deserialized
                    {
                        var recordData = data["DATA"];
                        string jsonData = JsonConvert.SerializeObject(recordData);
                        var packetList = JsonConvert.DeserializeObject<List<PacketModel>>(jsonData);
                        allModel.AddRange(packetList);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deserializing JSON: " + ex.Message);
                    return allModel;
                }
                Console.WriteLine("Successfully deserialized JSON");
            }
            return allModel;
        }
        public static void CalculateTimeDifference(List<PacketModel> allModel)
        {
            Console.WriteLine("Starting CalculateTimeDifference method");
            for (int i = 0; i < allModel.Count() - 1; i++)
            {
                allModel[i].PacketTime = ConvertStringToDateTime(allModel[i].GMTDATETIME);
                allModel[i + 1].PacketTime = ConvertStringToDateTime(allModel[i + 1].GMTDATETIME);

                if (allModel[i].PacketTime != null && allModel[i + 1].PacketTime != null)
                {
                    TimeSpan? timeDiff = (allModel[i + 1].PacketTime - allModel[i].PacketTime);

                    if (timeDiff.HasValue)
                    {
                        allModel[i].TimeDifferenceAsTimeSpan = timeDiff.Value;
                        Console.WriteLine("Time difference between packet " + i + " and packet " + (i + 1) + ": " + allModel[i].TimeDifferenceAsTimeSpan.TotalMilliseconds);
                    }
                }
            }
        }
        public static DateTime ConvertStringToDateTime(string value)
        {
            DateTime result;
            if (DateTime.TryParseExact(value, "yyyy-MM-dd'T'HH:mm:ss.FFFK", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Invalid datetime string value: " + value);
                return DateTime.MinValue;
            }
        }
        public static void CalculateIncrementalSizes(List<PacketModel> packetList)
        {
            var incrementalList = new List<int>();

            for (int i = 0; i < packetList.Count; i++)
            {
                packetList[i].Size = i + 1;
                
                if (i == 0)
                {
                    incrementalList.Add(packetList[i].Size);
                }
                else
                {
                    if (packetList[i].Size != 0)
                    {
                        incrementalList.Add(packetList[i].Size - packetList[i - 1].Size);
                    }
                }
            }
            Console.WriteLine("Packet sizes incremental: " + string.Join(", ", incrementalList));
        }
    }
}