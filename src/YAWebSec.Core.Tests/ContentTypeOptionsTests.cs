using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;


namespace YAWebSec.Core.Tests {
    public class ContentTypeOptionsTests {
        private static readonly Action<string, string> IoExceptionThrower = (a, b) => { throw new InvalidOperationException(); };

        [Fact]
        public async Task When_header_already_exist_it_should_not_be_added_or_overriden() {
            var cto = CreateCto(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet);
            var ctx = new TestContext {
                HeaderExistFunc = _ => true,
                OverrideHeaderValueAction = IoExceptionThrower,
                AppendHeaderValueAction = IoExceptionThrower
            };

            await cto.ApplyHeader(ctx);
        }

        [Theory]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet)]
        [InlineData(ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet)]
        public async Task When_header_does_not_exist_it_should_be_added(ContentTypeOptionsSettings.HeaderControl handling) {
            var cto = CreateCto(handling);
            bool called = false;
            var ctx = new TestContext {
                HeaderExistFunc = _ => false,
                OverrideHeaderValueAction = (a, b) => called = true,
                AppendHeaderValueAction = (a, b) => called = true
            };
            await cto.ApplyHeader(ctx);
            called.Should().BeTrue("Either Override or Append header should be called");
        }

        private static ContentTypeOptions CreateCto(ContentTypeOptionsSettings.HeaderControl headerHandling) {
            return new ContentTypeOptions(new ContentTypeOptionsSettings {
                HeaderHandling = headerHandling
            });
        }
    }
}
