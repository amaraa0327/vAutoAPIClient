using System.Web;
using System.Web.Mvc;
using vAuto.App_Start;

namespace vAuto
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new VActionFilter());
        }
    }
}
