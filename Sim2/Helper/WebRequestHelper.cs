using Sim2.Models;
using Sim2.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Media;

namespace Sim2.Helper
{
    public class WebRequestHelper
    {
        public const string packageurl = "xxxxxxx";
        public void SendToComms(int index, List<PacketModel> displayedDataList, SimPageProcessViewModel processViewModel, int maxRetryCount = 3, int retryDelayInSeconds = 1)
        {
            var dataToSend = displayedDataList[index];
            var utcnow = DateTime.UtcNow; // Changing the time zone with UTC date time
            dataToSend.GMTDATETIME = utcnow.ToString("yyyyMMddHHmmss");

            if (displayedDataList[index].BackgroundColor == Brushes.LightGreen)
            {
                int retryCount = 0;
                bool success = false;
                var log = new Log();

                while (retryCount <= maxRetryCount && !success)
                {
                    try
                    {
                        string url = GenerateUrlFromPacketModel(dataToSend, processViewModel);
                        log.WriteUrlLog(url);
                        var response = RequestHelper.HttpGet(url);
                        log.WriteResponseLog(response);

                        success = true;
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        if (retryCount > maxRetryCount)
                        {
                            log.WriteErrorLog(ex);
                        }
                        else
                        {
                            log.WriteRetryLog(retryCount, retryDelayInSeconds);
                        }
                    }
                }
            }
        }
        public string GenerateUrlFromPacketModel(PacketModel packetModel, SimPageProcessViewModel processViewModel)
        {
            var mapper = new MapperConfiguration(cfg => // Map PacketModel to PlogCsvPacketModel using AutoMapper
            {
                cfg.CreateMap<PacketModel, PlogCsvPacketModel>();
            }).CreateMapper();
            var plogCsvPacket = mapper.Map<PlogCsvPacketModel>(packetModel);

            StringBuilder sb = new StringBuilder();
            sb.Append(processViewModel.PackageUrl);
            sb.Append(
                WebUtility.UrlEncode(plogCsvPacket.PktType) + "," +
                plogCsvPacket.GMTDateTime + "," +
                plogCsvPacket.Longitude + "," +
                plogCsvPacket.Latitude + "," +
                plogCsvPacket.Speed + "," +
                plogCsvPacket.Course + "," +
                plogCsvPacket.Odometer + "," +
                plogCsvPacket.Region + "," +
                plogCsvPacket.Altitude + "," +
                plogCsvPacket.StandStill + "," +
                plogCsvPacket.Idling + "," +
                plogCsvPacket.Ignition + "," +
                plogCsvPacket.GPSNumOfSat + "," +
                plogCsvPacket.SCDriver + "," +
                plogCsvPacket.UserMapObject + "," +
                plogCsvPacket.Building + "," +
                plogCsvPacket.Area + "," +
                plogCsvPacket.Road + "," +
                plogCsvPacket.Neighbourhood + "," +
                plogCsvPacket.Village + "," +
                plogCsvPacket.Town + "," +
                plogCsvPacket.City + "," +
                plogCsvPacket.Country + "," +
                plogCsvPacket.LakeRiverSea + "," +
                plogCsvPacket.PostalCode + "," +
                plogCsvPacket.Route + "," +
                plogCsvPacket.ChargePercent + "," +
                plogCsvPacket.RouteDeparture + "," +
                plogCsvPacket.FuelLevelPercent + "," +
                plogCsvPacket.FuelLevelLitre + "," +
                plogCsvPacket.Temperature + "," +
                plogCsvPacket.Humidity + "," +
                plogCsvPacket.LiquidLevelPercent + "," +
                plogCsvPacket.Medium + "," +
                plogCsvPacket.FullAddress + "," +
                plogCsvPacket.SpeedLimit + "," +
                plogCsvPacket.EngineSpeed + "," +
                plogCsvPacket.BuildingNumber + "," +
                plogCsvPacket.CameraRegion + "," +
                plogCsvPacket.ResponsibilityArea + "," +
                plogCsvPacket.FuelDiff + "," +
                plogCsvPacket.PacketAge + "," +
                plogCsvPacket.CellInfo + "," +
                plogCsvPacket.LiquidLevelLitre + "," +
                plogCsvPacket.PowerLevel + "," +
                plogCsvPacket.Addr1 + "," +
                plogCsvPacket.Addr2 + "," +
                plogCsvPacket.IPAddress + "," +
                plogCsvPacket.RFTagID + "," +
                plogCsvPacket.TrailerID + "," +
                plogCsvPacket.MixerSpeed + "," +
                plogCsvPacket.DriverGSM + "," +
                plogCsvPacket.ObjectID + "," +
                plogCsvPacket.StringValue + "," +
                plogCsvPacket.DoubleValue + "," +
                plogCsvPacket.DailyDistance + "," +
                plogCsvPacket.VehicleState + "," +
                plogCsvPacket.TimeLag + "," +
                plogCsvPacket.Url + "," +
                plogCsvPacket.DoubleValue2 + "," +
                plogCsvPacket.ObjectGUID + "," +
                plogCsvPacket.UserGroupGUID);
            return sb.ToString();
        }
    }
}