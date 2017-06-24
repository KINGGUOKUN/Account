using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Account.Service.Contract;
using Account.Models;
using Account.Entity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Account.Controllers
{
    [Route("[controller]")]
    public class ManifestController : Controller
    {
        private readonly IManifestService _manifestService;

        public ManifestController(IManifestService manifestService)
        {
            _manifestService = manifestService;
        }

        //public async Task<IActionResult> Index()
        //{
        //    DateTime start = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-3),
        //        end = DateTime.Now;
        //    var pagedList = await _manifestService.GetManifests(start, end, 1, 10);
        //    var result = new ManifestViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
        //    {
        //        Start = start,
        //        End = end
        //    };

        //    return View(result);
        //}

        [HttpGet("{ID}")]
        public Manifest GetManifestById(string ID)
        {
            return _manifestService.GetManifestById(ID);
        }

        [HttpGet("paged")]
        public async Task<ManifestViewModel> GetPagedList(DateTime start, DateTime end, int pageIndex, int pageSize)
        {
            var pagedList = await _manifestService.GetManifests(start, end, pageIndex, pageSize);
            var result = new ManifestViewModel(pagedList.PageIndex, pagedList.PageSize, pagedList.Count, pagedList.Items)
            {
                Start = start,
                End = end
            };

            return result;
        }

        [HttpPost("")]
        public IActionResult Add([FromBody]Manifest manifest)
        {
            manifest = _manifestService.AddManifest(manifest);

            return CreatedAtRoute(new { ID = manifest.ID }, manifest);
        }

        //public IActionResult Edit(string ID)
        //{
        //    var manifest = _manifestService.GetManifestById(ID);
        //    if (manifest == null)
        //    {
        //        return NotFound("指定消费明细已删除");
        //    }
        //    else
        //    {
        //        return View(manifest);
        //    }
        //}

        [HttpPut("")]
        public IActionResult Edit([FromBody]Manifest manifest)
        {
            _manifestService.UpdateManifest(manifest);

            return NoContent();
        }

        [HttpDelete("{ID}")]
        public IActionResult Delete(string ID)
        {
            _manifestService.DeleteManifest(ID);

            return RedirectToAction("Index");
        }
    }
}
