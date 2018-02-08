using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using vAuto.Models;

namespace vAuto.Helpers
{
    public class VHelper
    {
        public static string datasetId;
        public static JArray arrOfIds;

        //We can use local db or some other local storage instead of these two
        public static Dictionary<int, VehicleVM> vehicles = new Dictionary<int, VehicleVM>();
        public static Dictionary<int, DealerVM> dealers = new Dictionary<int, DealerVM>();

        public static void GetDatasetId() {
            DataSetVM dataset = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://vautointerview.azurewebsites.net/api/");
                var responseTask = client.GetAsync("datasetId");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<DataSetVM>();
                    readtask.Wait();

                    dataset = readtask.Result;
                    VHelper.datasetId = dataset.datasetId;
                }
                else
                {
                    dataset = null;

                    throw new Exception("Couldn't get datasetId!");
                }
            }
        }
    }
}