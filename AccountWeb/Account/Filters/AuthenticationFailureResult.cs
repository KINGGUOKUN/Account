using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Account.Filters
{
    /// <summary>
    /// 认证失败结果生成器
    /// </summary>
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        /// <summary>
        /// 认证失败描述信息
        /// </summary>
        public string ReasonPhrase { get; private set; }

        /// <summary>
        /// Http请求消息
        /// </summary>
        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, this.ReasonPhrase));
        }
    }
}