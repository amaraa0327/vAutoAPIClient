using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using vAuto.Helpers;
using vAuto.Models;

namespace vAuto.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllVehicles()
        {
            if (VHelper.vehicles.Count == 0) {
                ModelState.AddModelError("", "Data is not loaded yet. Please wait few second! And refresh again.");
                return View(new List<VehicleVM>());
            }
            if (VHelper.vehicles.Count != VHelper.arrOfIds.Count) {
                ModelState.AddModelError("", "Data is still loading. Refresh again after few second, it will be fully loaded!");
            }
            List<VehicleVM> vehicles = VHelper.vehicles.Select(v => v.Value).ToList();
            return View(vehicles);
        }

        public ActionResult AllDealers()
        {
            ViewBag.isComplete = true;
            if (VHelper.dealers.Count == 0)
            {
                ViewBag.isComplete = false;
                ModelState.AddModelError("", "Data is not loaded yet. Please wait few second! And refresh again.");
                return View(new List<DealerVM>());
            }
            if (VHelper.dealers.SelectMany(d => d.Value.vehicles).Count() != VHelper.arrOfIds.Count || VHelper.vehicles.Count != VHelper.arrOfIds.Count)
            {
                ViewBag.isComplete = false;
                ModelState.AddModelError("", "Data is still loading. Refresh again after few second, it will be fully loaded!");
            }
            List<DealerVM> vehicles = VHelper.dealers.Select(d => d.Value).ToList();
            return View(vehicles);
        }
        
        public ActionResult PostAnswer()
        {
            JObject objStatus = new JObject();
            JObject json = new JObject();
            json["dealers"] = JToken.FromObject(VHelper.dealers.Select(d => d.Value).ToList<DealerVM>());

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://vautointerview.azurewebsites.net/api/");
                var postTask = client.PostAsJsonAsync<JObject>(VHelper.datasetId+"/answer", json);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readtask = result.Content.ReadAsAsync<JObject>();
                    readtask.Wait();

                    objStatus = readtask.Result;
                }
            }
            ViewBag.JsonResult = objStatus;
            return View();
        }
    }
}