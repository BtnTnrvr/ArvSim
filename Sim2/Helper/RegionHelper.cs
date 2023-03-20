using Sim2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sim2.Helper
{
    internal class RegionHelper
    {
        internal static List<PacketModel> ConvertRegionNames(List<PacketModel> items, List<string> regionList)
        {
            Dictionary<string, string> regionToRgMap = new Dictionary<string, string>();
            int rgCounter = 1;

            foreach (string region in regionList)
            {
                if (string.IsNullOrEmpty(region))
                {
                    continue;
                }                
                if (!regionToRgMap.ContainsKey(region)) // If the region is not already mapped to an rg value, map it to the next available rg value
                {
                    regionToRgMap[region] = "rg" + rgCounter;
                    rgCounter++;
                }
            }            
            foreach (PacketModel item in items) // Loop through the each region in regionList and update the regionList regions property to use the corresponding rg values
            {
                if (item.REGION != null)
                {
                    if (regionToRgMap.ContainsKey(item.REGION))
                    {
                        item.REGION = regionToRgMap[item.REGION];
                    }
                }
                if (item.REGION2 != null)
                {
                    if (regionToRgMap.ContainsKey(item.REGION2))
                    {
                        item.REGION2 = regionToRgMap[item.REGION2];
                    }
                }
                if (item.REGION3 != null)
                {
                    if (regionToRgMap.ContainsKey(item.REGION3))
                    {
                        item.REGION3 = regionToRgMap[item.REGION3];
                    }
                }
                if (item.REGION4 != null)
                {
                    if (regionToRgMap.ContainsKey(item.REGION4))
                    {
                        item.REGION4 = regionToRgMap[item.REGION4];
                    }
                }
                if (item.REGION5 != null)
                {
                    if (regionToRgMap.ContainsKey(item.REGION5))
                    {
                        item.REGION5 = regionToRgMap[item.REGION5];
                    }
                }
            }
            return items;
        }        
        public static List<string> GetDistinctRegions(List<PacketModel> items) //Checking All regions
        {
            return items.Where(i => i.REGION != null || i.REGION2 != null || i.REGION3 != null || i.REGION4 != null || i.REGION5 != null)
                        .SelectMany(i => new List<string> { i.REGION, i.REGION2, i.REGION3, i.REGION4, i.REGION5 })
                        .Where(r => r != null)
                        .Distinct()
                        .ToList();
        }        
        public static List<string> GetREGION(List<PacketModel> items) 
        {
            return items.Where(i => i.REGION != null).Select(i => i.REGION).Distinct().ToList();
        }        
        public static List<string> GetREGION2(List<PacketModel> items) 
        {
            return items.Where(i => i.REGION2 != null).Select(i => i.REGION2).Distinct().ToList();
        }        
        public static List<string> GetREGION3(List<PacketModel> items) 
        {
            return items.Where(i => i.REGION3 != null).Select(i => i.REGION3).Distinct().ToList();
        }        
        public static List<string> GetREGION4(List<PacketModel> items) 
        {
            return items.Where(i => i.REGION4 != null).Select(i => i.REGION4).Distinct().ToList();
        }        
        public static List<string> GetREGION5(List<PacketModel> items) 
        {
            return items.Where(i => i.REGION5 != null).Select(i => i.REGION5).Distinct().ToList();
        }
    }
}