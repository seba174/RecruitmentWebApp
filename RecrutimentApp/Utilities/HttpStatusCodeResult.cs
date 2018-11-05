using Microsoft.AspNetCore.Mvc;

namespace RecrutimentApp.Utilities
{
    public class HttpStatusCodeResult : ActionResult
    {
        private object badRequest;

        public HttpStatusCodeResult(object badRequest) => this.badRequest = badRequest;
    }
}
