using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LayItOut.Loaders
{
    /// <summary>
    /// Can be used on public static methods that accepts string and return target type or task of target type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AttributeParserAttribute : Attribute
    {
        private static (Type type, Func<string, Task<object>> method) CreateDelegate(MethodInfo parseMethod)
        {
            if (IsAsync(parseMethod))
            {
                var returnType = parseMethod.ReturnType.GenericTypeArguments[0];

                Func<MethodInfo, Func<string, Task<object>>> func = CreateAsyncDelegate<object>;
                var method = (Func<string, Task<object>>)func.Method.GetGenericMethodDefinition()
                    .MakeGenericMethod(returnType).Invoke(null, new object[] { parseMethod });

                return (returnType, method);
            }
            else
            {
                Func<MethodInfo, Func<string, Task<object>>> func = CreateSyncDelegate<object>;
                var method = (Func<string, Task<object>>)func.Method.GetGenericMethodDefinition()
                    .MakeGenericMethod(parseMethod.ReturnType).Invoke(null, new object[] { parseMethod });

                return (parseMethod.ReturnType, method);
            }
        }

        private static Func<string, Task<object>> CreateSyncDelegate<T>(MethodInfo parseMethod)
        {
            var func = (Func<string, T>)parseMethod.CreateDelegate(typeof(Func<string, T>));
            return x => Task.FromResult((object)func(x));
        }

        private static Func<string, Task<object>> CreateAsyncDelegate<T>(MethodInfo parseMethod)
        {
            var asyncFunc = (Func<string, Task<T>>)parseMethod.CreateDelegate(typeof(Func<string, Task<T>>));
            return async x => await asyncFunc(x);
        }

        internal static IEnumerable<(Type type, Func<string, Task<object>> method)> FindParseMethods(Type type) =>
            type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.GetCustomAttribute<AttributeParserAttribute>() != null)
                .Select(CreateDelegate);

        private static bool IsAsync(MethodInfo methodInfo)
        {
            return methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}
