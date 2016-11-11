using Account.BLL;
using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Account.Controllers
{
    /// <summary>
    /// 日消费明细控制器
    /// </summary>
    [RoutePrefix("api/Manifests")]
    public class ManifestsController : ApiController
    {
        #region Private Fields

        private readonly ManifestBLL _bll = new ManifestBLL();

        #endregion

        [Route("")]
        public IEnumerable<Manifest> GetManifest(DateTime start, DateTime end)
        {
            return _bll.GetManifest(start, end);
        }

        /// <summary>
        /// 添加消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public HttpResponseMessage AddManifest(Manifest manifest)
        {
            manifest = _bll.AddManifest(manifest);
            var response = Request.CreateResponse<Manifest>(HttpStatusCode.Created, manifest);
            response.Headers.Location = new Uri(Url.Link("DefaultApi", 
                new
                {
                    controller = "Manifests",
                    id = manifest.ID
                }));

            return response;
        }

        /// <summary>
        /// 更新消费明细
        /// </summary>
        /// <param name="manifest"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public HttpResponseMessage UpdateManifest(Manifest manifest)
        {
            bool result = _bll.UpdateManifest(manifest);
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "更新成功");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "更新出错");
            }
        }

        [HttpDelete]
        [Route("{ID}")]
        public HttpResponseMessage DeleteManifest(string ID)
        {
            bool result = _bll.DeleteManifest(ID);
            if (result)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "删除成功");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "删除出错");
            }
        }
    }
}
