using Sim2.Helper;
using Sim2.Models;
using System.Collections.Generic;
using System.Windows;

namespace Sim2.ViewModels
{
    public class SimPageRegionViewModel
    {
        public List<string> RegionList { get; set; }
        public List<string> Region1List { get; set; }
        public List<string> Region2List { get; set; }
        public List<string> Region3List { get; set; }
        public List<string> Region4List { get; set; }
        public List<string> Region5List { get; set; }
        public MessageBoxResult result {  get; set; }

        public void PopulateRegionLists(List<PacketModel> items)
        {
            RegionList = RegionHelper.GetDistinctRegions(items); // Getting the how many different region for each region and collecting them in to one list
            Region1List = RegionHelper.GetREGION(items);
            Region2List = RegionHelper.GetREGION2(items);
            Region3List = RegionHelper.GetREGION3(items);
            Region4List = RegionHelper.GetREGION4(items);
            Region5List = RegionHelper.GetREGION5(items);

            items = RegionHelper.ConvertRegionNames(items, RegionList); // Getting ConvertRegion method from the RegionHelper
        }
    }
}