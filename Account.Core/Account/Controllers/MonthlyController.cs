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
    public class MonthlyController : Controller
    {
        private readonly IMonthlyService _monthlyService;

        public MonthlyController(IMonthlyService monthlyService)
        {
            _monthlyService = monthlyService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    string start = DateTime.Now.AddMonths(-3).ToString("yyyy-MM"),
        //        end = DateTime.Now.ToString("yyyy-MM");
        //    var pagedList = await _monthlyService.GetMonthlys(start, end, 1, 10);
        //    var result = new MonthlyViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
        //    {
        //        Start = start,
        //        End = end
        //    };

        //    return View(result);
        //}

        [HttpGet("paged")]
        public async Task<MonthlyViewModel> GetPagedList(string start, string end, int pageIndex, int pageSize)
        {
            var pagedList = await _monthlyService.GetMonthlys(start, end, pageIndex, pageSize);
            var result = new MonthlyViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
            {
                Start = start,
                End = end
            };

            return result;
        }
    }
}
