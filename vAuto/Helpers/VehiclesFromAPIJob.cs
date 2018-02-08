using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using vAuto.Models;
using System.Net.Http;

namespace vAuto.Helpers
{
    public class VehiclesFromAPIJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            GetAllVehicles();
        }

        private void GetAllVehicles()
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

                    foreach (int jobj in VHelper.arrOfIds)
                    {
                        tempV = GetSingleVehicle(client, jobj);
                        if (VHelper.vehicles.ContainsKey(tempV.vehicleId))
                            VHelper.vehicles.Remove(tempV.vehicleId);
                        VHelper.vehicles.Add(tempV.vehicleId, tempV);
                    }
                }
                else 
                {
                    allVehicleIDs = null;

                    throw new Exception("Server error. Please contact administrator.");
                }
            }
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