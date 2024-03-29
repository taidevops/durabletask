﻿namespace DurableTask.Core
{
    using System;
    using System.Dynamic;
    using System.Reflection;

    /// <summary>
    /// Helper class for getting name information from types and instances
    /// </summary>
    public static class NameVersionHelper
    {
        internal static string GetDefaultMethodName(MethodInfo methodInfo, bool useFullyQualifiedMethodNames)
        {
            string methodName = methodInfo.Name;
            if (useFullyQualifiedMethodNames && methodInfo.DeclaringType != null)
            {
                methodName = GetFullyQualifiedMethodName(methodInfo.DeclaringType.Name, methodInfo.Name);
            }

            return methodName;
        }

        /// <summary>
        /// Gets the default name of an Object instance without using fully qualified names
        /// </summary>
        /// <param name="obj">Object to get the name for</param>
        /// <returns>Name of the object instance's type</returns>
        public static string GetDefaultName(object obj)
        {
            return GetDefaultName(obj, false);
        }

        /// <summary>
        /// Gets the default name of an Object instance using reflection
        /// </summary>
        /// <param name="obj">Object to get the name for</param>
        /// <param name="useFullyQualifiedMethodNames">Boolean indicating whether to use fully qualified names or not</param>
        /// <returns>Name of the object instance's type</returns>
        public static string GetDefaultName(object obj, bool useFullyQualifiedMethodNames)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            string name;
            Type type;
            MethodInfo methodInfo;
            InvokeMemberBinder binder;
            if ((type = obj as Type) != null)
            {
                name = type.ToString();
            }
            else if ((methodInfo = obj as MethodInfo) != null)
            {
                name = GetDefaultMethodName(methodInfo, useFullyQualifiedMethodNames);
            }
            else if ((binder = obj as InvokeMemberBinder) != null)
            {
                name = binder.Name;
            }
            else
            {
                name = obj.GetType().ToString();
            }

            return name;
        }

        /// <summary>
        /// Gets the default version for an object instance's type
        /// </summary>
        /// <param name="obj">Object to get the version for</param>
        /// <returns>The version as string</returns>
        public static string GetDefaultVersion(object obj)
        {
            return string.Empty;
        }

        internal static string GetFullyQualifiedMethodName(string declaringType, string methodName)
        {
            if (string.IsNullOrWhiteSpace(declaringType))
            {
                return methodName;
            }

            return declaringType + "." + methodName;
        }
    }
}
