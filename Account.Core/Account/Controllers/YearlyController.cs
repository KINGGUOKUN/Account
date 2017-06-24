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
    public class YearlyController : Controller
    {
        private readonly IYearlyService _yearlyService;

        public YearlyController(IYearlyService yearlyService)
        {
            _yearlyService = yearlyService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    int start = DateTime.Now.Year - 3,
        //        end = DateTime.Now.Year;
        //    var pagedList = await _yearlyService.GetYearlys(start, end, 1, 10);
        //    var result = new YearlyViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
        //    {
        //        Start = start,
        //        End = end
        //    };

        //    return View(result);
        //}

        [HttpGet("paged")]
        public async Task<YearlyViewModel> GetPagedList(int start, int end, int pageIndex, int pageSize)
        {
            var pagedList = await _yearlyService.GetYearlys(start, end, pageIndex, pageSize);
            var result = new YearlyViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
            {
                Start = start,
                End = end
            };

            return result;
        }
    }
}
