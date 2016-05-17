using System;

namespace YAWebSec.Core.Tests {
    internal class TestContext : IContext {
        public Func<string,bool> HeaderExistFunc { get; set; }
        public Action<string,string> OverrideHeaderValueAction { get; set; }
        public Action<string,string> AppendHeaderValueAction { get; set; }

        public bool HeaderExist(string headerName) {
            return HeaderExistFunc(headerName);
        }

        public void OverrideHeaderValue(string name, string value) {
            OverrideHeaderValueAction(name, value);
        }

        public void AppendHeaderValue(string name, string value) {
            AppendHeaderValueAction(name, value);
        }
    }
}