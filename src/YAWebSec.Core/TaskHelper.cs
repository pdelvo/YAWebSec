using System.Threading.Tasks;

namespace YAWebSec.Core {
    /// <summary>
    /// Facade for Task specific conditional compilings (when available).
    /// </summary>
    internal static class TaskHelper {
        public static Task Completed() {
#if NET46
            return Task.FromResult(true);
#endif
            return Task.FromResult(0);
        }
    }
}