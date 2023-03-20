using System;
using System.Windows.Media;

namespace Sim2.Models
{
    public class PacketModel
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? PacketTime { get; set; }
        public string DATE_TIME { get; set; }

        //START OF JSON DATA COLUMN TYPES
        public double? NODE { get; set; }
        public string GMTDATETIME { get; set; }
        public string PKTTYPE { get; set; }
        public double? MED { get; set; }
        public double? EVENTTYPE { get; set; }
        public double? LATITUDE { get; set; }
        public double? LONGITUDE { get; set; }
        public double? SPEED { get; set; }
        public double? COURSE { get; set; }
        public double? ALTITUDE { get; set; }
        public double? ODOMETER { get; set; }
        public double? STANDSTILL { get; set; }
        public double? IDLING { get; set; }
        public double? IGNTIME { get; set; }
        public string REGION { get; set; }
        public string REGION2 { get; set; }
        public string REGION3 { get; set; }
        public string REGION4 { get; set; }
        public string REGION5 { get; set; }
        public string ROUTE { get; set; }
        public string ROUTEDEPARTURE { get; set; }
        public string DRIVER { get; set; }
        public double? STRINGVALUE { get; set; }
        public double? OBJECTID { get; set; }
        public double? CELLINFO { get; set; }
        public double? DOUBLEVALUE { get; set; }
        public double? DOUBLEVALUE2 { get; set; }
        public double? FUELLEVELPERCENT { get; set; }
        public double? FUELLEVELLITRE { get; set; }
        public double? TRAILERID { get; set; }
        public double? RFTAGID { get; set; }
        public double? TEMPERATURE { get; set; }
        public string Status { get; set; }

        //END OF JSON DATA COLUMN TYPES
        public double? DOUBLE_VALUE { get; set; }
        public int MinutesPassed { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MessageType { get; set; }
        public bool HasError { get; set; }
        public int Count { get; set; }
        public int Reference { get; set; }
        public int PacketNumber { get; set; }
        public long Time { get; set; }
        public TimeSpan TimeDifferenceAsTimeSpan { get; internal set; }
        public Brush BackgroundColor { get; set; }
    }
}