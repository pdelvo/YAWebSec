using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YAWebSec.Core;

namespace YAWebSec.Owin {
    using BuildFunc = Action<Func<IDictionary<string, object>, Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>>>;
    public static class BuildFuncExtensions {
        public static BuildFunc ContentTypeOptions(this BuildFunc builder, ContentTypeOptionsSettings settings) {
            builder.MustNotNull(nameof(builder));
            return builder;
        }    
    }
}