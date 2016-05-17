using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Owin;


namespace YAWebSec.Owin.AppBuilder {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;

    public static class AppBuilderExtensions {
        internal static BuildFunc UseOwin(this IAppBuilder builder) {
            return middleware => builder.Use(middleware(builder.Properties));
        }

        #region X-Content-Type-Options
        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response with <see cref="ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptionsHeader(this IAppBuilder builder) {
            builder.MustNotNull(nameof(builder));
            return ContentTypeOptionsHeader(builder, settings => settings.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet);
        }

        /// <summary>
        /// Adds the "X-Content-Type-Options" Header to the response.
        /// </summary>
        /// <param name="builder">The <see cref="IAppBuilder"/> instance.</param>
        /// <param name="configureSettings">Action to configure the settings-object.</param>
        /// <returns>The <see cref="IAppBuilder"/> instance.</returns>
        public static IAppBuilder ContentTypeOptionsHeader(this IAppBuilder builder, Action<ContentTypeOptionsSettings> configureSettings) {
            builder.MustNotNull(nameof(builder));
            configureSettings.MustNotNull(nameof(configureSettings));
            var settings = new ContentTypeOptionsSettings();
            configureSettings(settings);
            builder.UseOwin().ContentTypeOptions(settings);
            //TODO Add Middleware add here
            return builder;
        }
        #endregion
    }
}