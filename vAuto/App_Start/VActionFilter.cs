using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using vAuto.Models;

namespace vAuto.App_Start
{
    public class VActionFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.RouteData.Values["action"].Equals("AllVehicles"))
            {
                
            }

            base.OnActionExecuting(filterContext);
        }
    }
}