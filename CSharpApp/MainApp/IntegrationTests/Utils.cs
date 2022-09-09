using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace MainApp.IntegrationTests
{
    public class Utils
    {

        public static Type GetTypeByFullName(string fullName, Type[] types)
        {
            foreach (Type type in types)
            {
                if (type.FullName.Equals(fullName))
                {
                    return type;
                }
            }
            return null;
        }

        public static Tuple<Type, MethodInfo> GetSetupMethod(Type[] types)
        {

            var typeToMethodInfo = new List<Tuple<Type, MethodInfo>>();
            foreach (Type type in types)
            {
                var methodInfos = type.GetMethods();

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    var setupAttribute = DoesMethodInfoHaveSetupAttribute(methodInfo);
                    if (setupAttribute != null)
                    {
                        var tuple = new Tuple<Type, MethodInfo>(type, methodInfo);
                        return tuple;
                    }

                }
            }
            return null;
        }
        public static Tuple<Type, MethodInfo> GetTearDownMethod(Type[] types)
        {

            var typeToMethodInfo = new List<Tuple<Type, MethodInfo>>();
            foreach (Type type in types)
            {
                var methodInfos = type.GetMethods();

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    var tearDownAttribute = DoesMethodInfoHaveTearDownAttribute(methodInfo);
                    if (tearDownAttribute != null)
                    {
                        var tuple = new Tuple<Type, MethodInfo>(type, methodInfo);
                        return tuple;
                    }

                }
            }
            return null;
        }

        public static List<Tuple<Type, MethodInfo>> GetTestMethodDictionarySafely(Type[] types)
        {

            var typeToMethodInfo = new List<Tuple<Type, MethodInfo>>();
            foreach (Type type in types)
            {
                var methodInfos = type.GetMethods();

                foreach (MethodInfo methodInfo in methodInfos)
                {
                    var testAttribute = DoesMethodInfoHaveTestAttribute(methodInfo);
                    if (testAttribute != null)
                    {
                        TestAttribute commandMethodAttribute = (TestAttribute)testAttribute;
                        var tuple = new Tuple<Type, MethodInfo>(type, methodInfo);
                        typeToMethodInfo.Add(tuple);
                    }

                }
            }
            return typeToMethodInfo;
        }

        private static object DoesMethodInfoHaveSetupAttribute(MethodInfo methodInfo)
        {
            var objectAttributes = methodInfo.GetCustomAttributes(true);
            foreach (object objectAttribute in objectAttributes)
            {
                if (ReferenceEquals(objectAttribute.GetType(), typeof(SetUpAttribute)))
                {
                    return objectAttribute;
                }
            }
            return null;
        }

        private static object DoesMethodInfoHaveTearDownAttribute(MethodInfo methodInfo)
        {
            var objectAttributes = methodInfo.GetCustomAttributes(true);
            foreach (object objectAttribute in objectAttributes)
            {
                if (ReferenceEquals(objectAttribute.GetType(), typeof(TearDownAttribute)))
                {
                    return objectAttribute;
                }
            }
            return null;
        }

        private static object DoesMethodInfoHaveTestAttribute(MethodInfo methodInfo)
        {
            var objectAttributes = methodInfo.GetCustomAttributes(true);
            foreach (object objectAttribute in objectAttributes)
            {
                if (ReferenceEquals(objectAttribute.GetType(), typeof(TestAttribute)))
                {
                    return objectAttribute;
                }
            }
            return null;
        }
    }
}