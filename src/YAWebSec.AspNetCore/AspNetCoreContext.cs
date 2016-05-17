using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Headers;
using YAWebSec.Core;

namespace YEWebSec.AspNetCore {
    internal class AspNetCoreContext : IContext {
        private readonly HttpContext mContext;
        private IHeaderDictionary Headers => mContext.Response.Headers;
        private ResponseHeaders TypedHeaders => mContext.Response.GetTypedHeaders();

        public AspNetCoreContext(HttpContext context) {
            mContext = context;
        }

        public void AppendHeaderValue(string name, string value) => TypedHeaders.Append(name, value);

        public bool HeaderExist(string headerName) => Headers.ContainsKey(headerName);

        public void OverrideHeaderValue(string name, string value) {
            if (Headers.ContainsKey(name)) {
                Headers.Remove(name);
            }
            
            TypedHeaders.Set(name, value);
        }
    }
}