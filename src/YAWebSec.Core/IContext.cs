namespace YAWebSec.Core {
    internal interface IContext {
        bool HeaderExist(string headerName);
        void OverrideHeaderValue(string name, string value);
        void AppendHeaderValue(string name, string value);
    }
}