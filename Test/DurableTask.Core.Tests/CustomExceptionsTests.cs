//  ----------------------------------------------------------------------------------
//  Copyright Microsoft Corporation
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//  http://www.apache.org/licenses/LICENSE-2.0
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//  ----------------------------------------------------------------------------------

namespace DurableTask.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CustomExceptionsTests
    {
        List<Type> customExceptions;
        const string DefaultExceptionsNamespace = "DurableTask.Core.Exceptions";
        const string DefaultExceptionMessage = "Test Message";

        [TestInitialize]
        public void Initialize()
        {
            // Get all exceptions from the DurableTask.Core that are public
            this.customExceptions = typeof(TaskHubWorker).Assembly.GetTypes().Where(_ => typeof(Exception).IsAssignableFrom(_) && _.IsPublic).ToList();
        }

        [TestMethod]
        public void CustomExceptionNamespace()
        {
            this.customExceptions.ForEach(_ =>
            {
                Assert.AreEqual(DefaultExceptionsNamespace, _.Namespace, "All custom exception must be defined in the '{DefaultExceptionsNamespace}' namespace");
            });
        }

        [TestMethod]
        public void CustomExceptionDefaultConstructors()
        {
            this.customExceptions.ForEach(_ =>
            {
                // Get the default constructor
                ConstructorInfo constructor = _.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new Type[0], null);
                Assert.IsNotNull(constructor, $"Default constructor .ctor() for exception '{_.FullName}' does not exist");

                // Create an instance to make sure no exception is raised
                constructor.Invoke(new object[0]);

                // Get the constructor with a single string parameter
                constructor = _.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] { typeof(string) }, null);
                Assert.IsNotNull(constructor, $"Default constructor .ctor(string) for exception '{_.FullName}' does not exist");

                // Create an instance to make sure no exception is raised
                var exception = (Exception)constructor.Invoke(new object[] { DefaultExceptionMessage });
                Assert.AreEqual(DefaultExceptionMessage, exception.Message);

                // Get the constructor with a single string parameter and a inner exception
                constructor = _.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new[] { typeof(string), typeof(Exception) }, null);
                Assert.IsNotNull(constructor, $"Default constructor .ctor(string, Exception) for exception '{_.FullName}' does not exist");

                // Create an instance to make sure no exception is raised
                var timeOutException = new TimeoutException();
                exception = (Exception)constructor.Invoke(new object[] { DefaultExceptionMessage, timeOutException });
                Assert.AreSame(timeOutException, exception.InnerException, $"Inner exception for exception '{_.FullName}' was not set properly");
            });
        }


    }
}
