using AutoMapper;
using Sim2.Models;
using Sim2.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sim2.Helper
{
    public class WebRequestHelper
    {
        static object control = new object();
        public const string packageurl = "https://node.arvento.com/arvento?app=desktop&user=okan.sultan82&pin1=Btn745896!&pkt=U519,PLOG,{0},";
        public async Task SendToComms(int index, List<PacketModel> _displayedDataList, SimPageComboBoxViewModel comboboxviewModel)
        {
            var filePath = "C:\\Users\\BtnTn\\OneDrive\\Masaüstü\\Simulasyon Publish\\log.txt";
            try
            {                
                using (StreamWriter logWriter = new StreamWriter(filePath, true))
                {
                    logWriter.WriteLine($"Method called with index {index}");
                }

                TaskScheduler.UnobservedTaskException += (object sender, UnobservedTaskExceptionEventArgs eventArgs) =>
                {
                    eventArgs.SetObserved();
                    ((AggregateException)eventArgs.Exception).Handle(ex =>
                    {
                        lock (control)
                        {
                            using (StreamWriter writer = new StreamWriter(filePath, true)) // Log the unobserved task exception
                            {
                                writer.WriteLine($"Unobserved task exception: {ex.GetType()}");
                                return true;
                            }
                        }
                    });
                };

                var dataToSend = _displayedDataList[index];
                var utcnow = DateTime.UtcNow; // Changing the time zone with UTC date time
                dataToSend.GMTDATETIME = utcnow.ToString("yyyyMMddHHmmss");
                var timeOut = 30000;

                if (_displayedDataList[index].BackgroundColor == Brushes.LightGreen)
                {
                    try
                    {
                        string url = GenerateUrlFromPacketModel(dataToSend, comboboxviewModel);
                        Console.WriteLine(url); // Checking url
                        try
                        {
                            lock (control)
                            {
                                using (StreamWriter writer = new StreamWriter(filePath, true)) // Log the URL and local time to the log file
                                {
                                    writer.WriteLine($"{DateTime.Now} - URL: {url}");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error writing to log file: " + ex.Message);
                        }
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.ContinueTimeout = timeOut;
                        request.Timeout = timeOut;
                        request.ReadWriteTimeout = timeOut;
                        request.Method = "GET";
                        request.ContentType = "application/json";
                        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                        using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync()) // Send the request and handle the response
                        using (Stream stream = response.GetResponseStream())
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string responseText = await reader.ReadToEndAsync();                            
                            try
                            {
                                lock (control)
                                {
                                    using (StreamWriter writer = new StreamWriter(filePath, true)) // Log the response of the url
                                    {
                                        writer.WriteLine($"{DateTime.Now} - Response: " + responseText);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                lock (control)
                                {
                                    using (StreamWriter writer = new StreamWriter(filePath, true)) // Log the response of the url
                                    {
                                        writer.WriteLine($"Error writing to log file: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (control)
                        {
                            using (StreamWriter writer = new StreamWriter(filePath, true))
                            {
                                writer.WriteLine("Exception: " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"Error: {ex.Message}");
                    writer.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }
        }
        public string GenerateUrlFromPacketModel(PacketModel packetModel, SimPageComboBoxViewModel comboboxviewModel)
        {
            var mapper = new MapperConfiguration(cfg => // Map PacketModel to PlogCsvPacketModel using AutoMapper
            {
                cfg.CreateMap<PacketModel, PlogCsvPacketModel>();
            }).CreateMapper();
            var plogCsvPacket = mapper.Map<PlogCsvPacketModel>(packetModel);

            StringBuilder sb = new StringBuilder();
            sb.Append(comboboxviewModel.PackageUrl);
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