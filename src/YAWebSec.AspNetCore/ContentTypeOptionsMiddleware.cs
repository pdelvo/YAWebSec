using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using YAWebSec;
using YAWebSec.Core;

namespace YEWebSec.AspNetCore {
    internal class ContentTypeOptionsMiddleware {
        private readonly RequestDelegate mNext;
        private readonly ContentTypeOptionsSettings mSettings;

        public ContentTypeOptionsMiddleware(RequestDelegate next, ContentTypeOptionsSettings settings) {
            next.MustNotNull(nameof(next));
            settings.MustNotNull(nameof(settings));

            mNext = next;
            mSettings = settings;
        }

        public Task Invoke(HttpContext context) {
            context.Response.OnStarting(ApplyHeader, state: context);
            return mNext(context);
        }

        private Task ApplyHeader(object arg) {
            var ctx = (HttpContext) arg;

            if (SetHeader(ctx.Response.Headers)) {
                ctx.Response.Headers.Add("HeaderKey","nosniff");
            }

            return Task.FromResult(0);
        }

        private bool SetHeader(IHeaderDictionary headers) {
            if (mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return true;
            }
            return !headers.ContainsKey("HeaderKey");
        }
    }
}