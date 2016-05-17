using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.TestHost;
using Xunit;
using YAWebSec.Core;
using YEWebSec.AspNetCore;

namespace YAWebSec.AspNetCore.Tests {
    public class ContentTypeOptionsMiddlewareTests {
        private TestServer mServer;
        private HttpClient mClient;
        private Action<HttpContext> mModifyRun;
        private Action<ContentTypeOptionsSettings> mSetSettings = settings => {};


        private void Arrange() {
            mServer = TestServer.Create(app => {
                app.UseContentTypeOptions(mSetSettings);
                app.Run(async ctx => {
                    ctx.Response.StatusCode = 200;
                    mModifyRun?.Invoke(ctx);
                    await ctx.Response.WriteAsync("Hello World!");
                });
            });
            mClient = mServer.CreateClient();
        }

        [Fact]
        public async Task When_adding_with_default_settings_it_should_add_the_header() {
            Arrange();
            var result = await mClient.GetAsync("/");
            result.XContentTypeOptions().Should().Be("nosniff");
        }

        [Fact]
        public async Task When_adding_header_with_do_not_override_and_header_already_exist_it_should_not_be_overridden() {
            mModifyRun = ctx => ctx.Response.Headers.Add(ContentTypeOptions.XContentTypeOptions, "invalidvalue");
            mSetSettings = settings => settings.HeaderHandling = ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet;
            Arrange();
            var result = await mClient.GetAsync("/");
            result.XContentTypeOptions().Should().Be("invalidvalue");
        }
    }

    internal static class HeaderHelper {
        public static string XContentTypeOptions(this HttpResponseMessage source) {
            return source.Headers.GetValues(ContentTypeOptions.XContentTypeOptions).Single();
        }
    }
}