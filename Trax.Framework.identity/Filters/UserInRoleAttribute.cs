using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Trax.Framework.identity.Identity;

namespace Trax.Framework.identity.Filters
{
    public class UserInRoleAttribute : ActionFilterAttribute, IActionFilter
    {
        public string Application { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else if (!string.IsNullOrEmpty(this.Application))
            {
                if (!ContentAuthorize.IsAccessAuthorize(this.Application, actionContext.ActionDescriptor.ControllerDescriptor.ControllerName, actionContext.ActionDescriptor.ActionName))
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
            else
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            base.OnActionExecuting(actionContext);
        }
    }
}
