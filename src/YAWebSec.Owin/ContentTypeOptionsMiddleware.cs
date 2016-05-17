using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAWebSec.Core;

namespace YAWebSec.Owin {
    internal static class ContentTypeOptionsMiddleware {
        public static Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>> ContentTypeOptionsHeader(ContentTypeOptionsSettings settings) {
            return next =>
                env => {
                    var response = env.AsOwinContext().Response;
                    var state = new State<ContentTypeOptionsSettings>() {
                        Response = response,
                        Settings = settings
                    };
                    response.OnSendingHeaders(ApplyHeader, state);
                    return next(env);
                };
        }

        internal class State<T> {
            public IOwinResponse Response { get; set; }
            public T Settings { get; set; }
        }

        private static void ApplyHeader(State<ContentTypeOptionsSettings> obj) {
            var response = obj.Response;

            if (!SetHeader(obj.Settings, obj.Response.Headers)) {
                return;
            }
            response.Headers["TODO"] = "nosniff";
        }

        private static bool SetHeader(ContentTypeOptionsSettings settings, IHeaderDictionary headers) {
            if (settings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet && headers.ContainsKey("TODO")) {
                return false;
            }
            return true;
        }
    }
}