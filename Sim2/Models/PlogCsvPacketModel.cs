using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim2.Models
{
    public class PlogCsvPacketModel
    {
        public int PLOG { get; set; } 
        public int Node { get; set; } 
        public string PktType { get; set; } 
        public string GMTDateTime { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public double? Speed { get; set; }
        public double? Course { get; set; }
        public double? Odometer { get; set; }
        public string Region { get; set; }
        public double? Altitude { get; set; }
        public double? StandStill { get; set; }
        public double? Idling { get; set; }
        public double? Ignition { get; set; }
        public double? GPSNumOfSat { get; set; }
        public string SCDriver { get; set; }
        public double? UserMapObject { get; set; }
        public string Building { get; set; }
        public string Area { get; set; }
        public string Road { get; set; }
        public string Neighbourhood { get; set; }
        public string Village { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string LakeRiverSea { get; set; }
        public string PostalCode { get; set; }
        public string Route { get; set; }
        public double? ChargePercent { get; set; }
        public string RouteDeparture { get; set; }
        public double? FuelLevelPercent { get; set; }
        public double? FuelLevelLitre { get; set; }
        public double? Temperature { get; set; }
        public double? Humidity { get; set; }
        public double? LiquidLevelPercent { get; set; }
        public double? Medium { get; set; }
        public string FullAddress { get; set; }
        public int SpeedLimit { get; set; }
        public int EngineSpeed{ get; set; }
        public int BuildingNumber { get; set; }
        public string CameraRegion { get; set; }
        public string ResponsibilityArea { get; set; }
        public double? FuelDiff { get; set; }
        public int PacketAge { get; set; }
        public int CellInfo { get; set; }
        public double? LiquidLevelLitre { get; set; }
        public double? PowerLevel { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public int IPAddress { get; set; }
        public double? RFTagID { get; set; }
        public double? TrailerID { get; set; }
        public double? MixerSpeed { get; set; }
        public string DriverGSM { get; set; }
        public double? ObjectID { get; set; }
        public double? StringValue { get; set; }
        public double? DoubleValue { get; set; }
        public int DailyDistance { get; set; }
        public string VehicleState { get; set; }
        public int TimeLag { get; set; }
        public string Url { get; set; }
        public double? DoubleValue2 { get; set; }
        public double? ObjectGUID { get; set; }
        public double? UserGroupGUID { get; set; }
    }
}