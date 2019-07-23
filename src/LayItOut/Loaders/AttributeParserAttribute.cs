using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LayItOut.Loaders
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AttributeParserAttribute : Attribute
    {
        private static Func<string, object> CreateDelegate(MethodInfo parseMethod)
        {
            Func<MethodInfo, Func<string, object>> func = CreateDelegate<object>;
            return (Func<string, object>)func.Method.GetGenericMethodDefinition()
                .MakeGenericMethod(parseMethod.ReturnType).Invoke(null, new object[] { parseMethod });
        }

        private static Func<string, object> CreateDelegate<T>(MethodInfo parseMethod)
        {
            var func = (Func<string, T>)parseMethod.CreateDelegate(typeof(Func<string, T>));
            return x => func(x);
        }

        internal static IEnumerable<(Type type, Func<string, object> method)> FindParseMethods(Type type) =>
            type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.GetCustomAttribute<AttributeParserAttribute>() != null)
                .Select(methodInfo => (methodInfo.ReturnType, CreateDelegate(methodInfo)));
    }
}
