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
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using DurableTask.Core.History;
    using DurableTask.Emulator;
    using DurableTask.Test.Orchestrations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DispatcherMiddlewareTests
    {
        TaskHubWorker worker;
        TaskHubClient client;

        [TestInitialize]
        public async Task Initialize()
        {
            var service = new LocalOrchestrationService();
            this.worker = new TaskHubWorker(service);

            await this.worker
                .AddTaskOrchestrations(typeof(SimplestGreetingsOrchestration))
                .StartAsync();

            this.client = new TaskHubClient(service);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            await this.worker.StopAsync(true);
        }

        [TestMethod]
        public void DispatchMiddlewareContextBuiltInProperties()
        {
            TaskOrchestration orchestration = null;
            this.worker.AddOrchestrationDispatcherMiddleware((context, next) =>
            {
                orchestration = context.GetProperty<TaskOrchestration>();

                return next();
            });

            Assert.IsNotNull(orchestration);
        }
    }
}
