using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using YAWebSec;
using YAWebSec.Core;

namespace YEWebSec.AspNetCore {
    public static class ApplicationBuilderExtension {
        #region X-Content-Type-Options
        /// <summary>
        /// Adds the "X-Content-Type-Options" header with value "nosniff" to the response.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder builder) {
            return UseContentTypeOptions(builder, options => options.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
        }

        /// <summary>
        /// Adds the "X-Content-Type-Options" header with the configured settings.
        /// </summary>
        /// <param name="builder">The IApplicationBuilder instance.</param>
        /// <param name="configureSettings">The action to set the settings.</param>
        /// <returns>The IApplicationBuilder instance.</returns>
        public static IApplicationBuilder UseContentTypeOptions(this IApplicationBuilder builder, Action<ContentTypeOptionsSettings> configureSettings) {
            builder.MustNotNull(nameof(builder));
            configureSettings.MustNotNull(nameof(configureSettings));

            var settings = new ContentTypeOptionsSettings();
            configureSettings(settings);

            var cto = new ContentTypeOptions(settings);
            builder.Use(async (context, func) => {
                context.Response.OnStarting(innerCtx => cto.ApplyHeader(innerCtx), context.AsInternalCtx());
                await func();
            });

            builder.UseMiddleware<ContentTypeOptionsMiddleware>(settings);
            return builder;
        }
        #endregion



        private static void OnStarting<T>(this HttpResponse source, Func<T, Task> callback, T context) {
            source.OnStarting(ctx => callback(context), context);
        }

        private static AspNetCoreContext AsInternalCtx(this HttpContext source) {
            return new AspNetCoreContext(source);
        }
    }

    
}