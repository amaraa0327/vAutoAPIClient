using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using vAuto.Models;

namespace vAuto.Helpers
{
    public class DealersFromAPIJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            GetAllDealers();
        }

        private void GetAllDealers()
        {
            JObject allVehicleIDs;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://vautointerview.azurewebsites.net/api/");
                var responseTask = client.GetAsync(VHelper.datasetId + "/vehicles");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<JObject>();
                    readtask.Wait();

                    allVehicleIDs = readtask.Result;

                    VHelper.arrOfIds = (JArray)allVehicleIDs["vehicleIds"];

                    VehicleVM tempV = null;
                    DealerVM tempD = null;

                    foreach (int jobj in VHelper.arrOfIds)
                    {
                        tempV = VHelper.vehicles.ContainsKey(jobj) ? VHelper.vehicles[jobj] : GetSingleVehicle(client, jobj);

                        tempD = GetSingleDealer(client, tempV.dealerId);
                        tempD.vehicles = new List<VehicleVM>() { tempV };
                        if (VHelper.dealers.ContainsKey(tempD.dealerId))
                        {
                            //tempD.vehicles.AddRange(VHelper.dealers[tempD.dealerId].vehicles);
                            if (!VHelper.dealers[tempD.dealerId].vehicles.Contains(tempV))
                                VHelper.dealers[tempD.dealerId].vehicles.Add(tempV);
                            //VHelper.dealers.Remove(tempD.dealerId);
                        }
                        else
                        {
                            VHelper.dealers.Add(tempD.dealerId, tempD);
                        }
                    }
                }
                else
                {
                    allVehicleIDs = null;

                    throw new Exception("Server error. Please contact administrator.");
                }
            }
        }

        private DealerVM GetSingleDealer(HttpClient client, int dealerID)
        {
            DealerVM dealer = null;

            var responseTask = client.GetAsync(VHelper.datasetId + "/dealers/" + dealerID);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readtask = result.Content.ReadAsAsync<DealerVM>();
                readtask.Wait();

                dealer = readtask.Result;
            }
            else
            {
                throw new Exception("Server error. Please contact administrator.");
            }

            return dealer;
        }

        private VehicleVM GetSingleVehicle(HttpClient client, int vehicleID)
        {
            VehicleVM vehicle = null;


            var responseTask = client.GetAsync(VHelper.datasetId + "/vehicles/" + vehicleID);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readtask = result.Content.ReadAsAsync<VehicleVM>();
                readtask.Wait();

                vehicle = readtask.Result;
            }
            else
            {
                throw new Exception("Server error. Please contact administrator.");
            }

            return vehicle;
        }
    }
}