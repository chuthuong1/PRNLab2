using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.DataAccess.Models;

namespace WebApplication1.Pages.Admin
{
    public class dashboardModel : PageModel
    {
        private readonly NorthwindContext dBContext;
        public dashboardModel(NorthwindContext dBContext)
        {
            this.dBContext = dBContext;
        }

    }
}

