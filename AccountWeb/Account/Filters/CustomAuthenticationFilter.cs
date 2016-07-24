using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Account.Filters
{
    /// <summary>
    /// 自定义认证
    /// </summary>
    public class CustomAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public virtual bool AllowMultiple
        {
            get { return false; }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var principal = await this.AuthenticateAsync(context.Request);
            if (principal == null)
            {
                context.Request.Headers.GetCookies().Clear();
                context.ErrorResult = new AuthenticationFailureResult("未授权请求", context.Request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        private Task<IPrincipal> AuthenticateAsync(HttpRequestMessage request)
        {
            //return Task.Run<IPrincipal>(() =>
            //    {
            //        CookieHeaderValue cookieMobile = request.Headers.GetCookies("clientMobile").FirstOrDefault(),
            //            cookieToken = request.Headers.GetCookies("clientToken").FirstOrDefault();
            //        if (cookieMobile == null || cookieToken == null
            //            || string.IsNullOrWhiteSpace(cookieMobile["clientMobile"].Value)
            //            || string.IsNullOrWhiteSpace(cookieToken["clientToken"].Value))
            //        {
            //            return null;
            //        }

            //        string mobile = cookieMobile["clientMobile"].Value,
            //            token = cookieToken["clientToken"].Value;
            //        ClientDTO client = null;
            //        using (ICache cache = ObjectContainer.Current.Resolve<ICacheFactory>().CreateCache())
            //        {
            //            client = cache.Get<ClientDTO>(RedisTables.CLIENT, mobile);
            //        }
            //        if (client != null && string.Equals(token, Md5Helper.MD5(string.Format("{0}{1}", mobile, client.MsgCode), 32), StringComparison.Ordinal))
            //        {
            //            IEnumerable<Claim> claims = new List<Claim>()
            //            {
            //                new Claim(ClaimTypes.Name, mobile)
            //            };
            //            var identity = new ClaimsIdentity("LoanCookie");
            //            identity.AddClaims(claims);
            //            return new ClaimsPrincipal(identity);
            //        }

            //        return null;
            //    });

            var tcs = new TaskCompletionSource<IPrincipal>();
            tcs.SetResult(null);

            return tcs.Task;
        }
    }
}