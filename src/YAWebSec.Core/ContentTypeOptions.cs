using System;
using System.Threading.Tasks;

namespace YAWebSec.Core {
    internal class ContentTypeOptions  {
        internal const string XContentTypeOptions = "X-Content-Type-Options";
        internal const string XContentTypeOptionsValue = "nosniff";
        private readonly ContentTypeOptionsSettings mSettings;

        public ContentTypeOptions(ContentTypeOptionsSettings settings) {
            mSettings = settings;
        }

        public Task ApplyHeader(IContext context) {
            if (!SetHeader(context.HeaderExist)) {
                return TaskHelper.Completed();
            }

            Action<string, string> action;
            switch (mSettings.HeaderHandling) {
                case ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet:
                    action = context.OverrideHeaderValue;
                    break;
                case ContentTypeOptionsSettings.HeaderControl.IgnoreIfHeaderAlreadySet:
                    action = context.AppendHeaderValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown enum-value '{mSettings.HeaderHandling}' of enum ${typeof(ContentTypeOptionsSettings.HeaderControl).FullName}");
            }
            action(XContentTypeOptions, XContentTypeOptionsValue);
            return TaskHelper.Completed();
        }

        private bool SetHeader(Func<string, bool> headerExist) {
            if (mSettings.HeaderHandling == ContentTypeOptionsSettings.HeaderControl.OverwriteIfHeaderAlreadySet) {
                return true;
            }
            return !headerExist(XContentTypeOptions);
        }
    }
}