using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Account.Service.Contract;
using Account.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account.Controllers
{
    [Route("[controller]")]
    public class DailyController : Controller
    {
        private readonly IDailyService _dailyService;

        public DailyController(IDailyService dailyService)
        {
            _dailyService = dailyService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    DateTime start = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-3),
        //        end = DateTime.Now;
        //    var pagedList = await _dailyService.GetDailys(start, end, 1, 10);
        //    var result = new DailyViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
        //    {
        //        Start = start,
        //        End = end
        //    };

        //    return View(result);
        //}

        [HttpGet("paged")]
        public async Task<DailyViewModel> GetPagedList(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var pagedList = await _dailyService.GetDailys(start, end, pageIndex, pageSize);
            var result = new DailyViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
            {
                Start = start,
                End = end
            };

            return result;
        }
    }
}
