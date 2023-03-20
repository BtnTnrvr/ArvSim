using Sim2.Models;  
using System.Windows;

namespace Sim2.UserControls
{
    public partial class EditPage : Window
    {
        public PacketModel EditedItem;
        public EditPage(PacketModel selectedItem)
        {
            InitializeComponent();
            
            EditedItem = new PacketModel() // Create a copy of the selected item to modify
            {
                NODE = selectedItem.NODE,
                PKTTYPE = selectedItem.PKTTYPE,
                MED = selectedItem.MED,
                EVENTTYPE = selectedItem.EVENTTYPE,
                LATITUDE = selectedItem.LATITUDE,
                LONGITUDE = selectedItem.LONGITUDE,
                SPEED = selectedItem.SPEED,
                COURSE = selectedItem.COURSE,
                ALTITUDE = selectedItem.ALTITUDE,
                ODOMETER = selectedItem.ODOMETER,
                STANDSTILL = selectedItem.STANDSTILL,
                IDLING = selectedItem.IDLING,
                IGNTIME = selectedItem.IGNTIME,
                REGION = selectedItem.REGION,
                REGION2 = selectedItem.REGION2,
                REGION3 = selectedItem.REGION3,
                REGION4 = selectedItem.REGION4,
                REGION5 = selectedItem.REGION5,
                ROUTE = selectedItem.ROUTE,
                ROUTEDEPARTURE = selectedItem.ROUTEDEPARTURE,
                DRIVER = selectedItem.DRIVER,
                STRINGVALUE = selectedItem.STRINGVALUE,
                OBJECTID = selectedItem.OBJECTID,
                CELLINFO = selectedItem.CELLINFO,
                DOUBLEVALUE = selectedItem.DOUBLEVALUE,
                DOUBLEVALUE2 = selectedItem.DOUBLEVALUE2,
                FUELLEVELPERCENT = selectedItem.FUELLEVELPERCENT,
                FUELLEVELLITRE = selectedItem.FUELLEVELLITRE,
                TRAILERID = selectedItem.TRAILERID,
                RFTAGID = selectedItem.RFTAGID,
                TEMPERATURE = selectedItem.TEMPERATURE,
                TimeDifferenceAsTimeSpan = selectedItem.TimeDifferenceAsTimeSpan
            };
            DataContext = EditedItem;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
