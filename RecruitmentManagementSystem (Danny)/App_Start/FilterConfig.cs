using System.Web;
using System.Web.Mvc;

namespace RecruitmentManagementSystem__Danny_
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
