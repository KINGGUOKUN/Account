using Account.Common;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace Account.Filters
{
    /// <summary>
    /// 自定义异常处理过滤器
    /// </summary>
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exception = actionExecutedContext.Exception;

            if (exception is BusinessException)     //业务异常，一般不需记录日志，直接反馈错误信息至前端
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse((HttpStatusCode)exception.HResult, new HttpError(exception.Message));
            }
            else        //未处理异常如数据库访问出错、代码层面异常等，返回错误信息并记录日志
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, new HttpError(exception.Message));
                Log.Logger.Error("应用程序处理出错：", exception);
            }
        }
    }
}