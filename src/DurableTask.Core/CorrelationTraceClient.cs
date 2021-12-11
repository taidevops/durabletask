using System;
using System.Diagnostics;

namespace DurableTask.Core
{
    /// <summary>
    /// Delegate sending telemetry to the other side.
    /// Mainly send telemetry to the Durable Functions TelemetryClient
    /// </summary>
    public static class CorrelationTraceClient
    {
        const string DiagnosticSourceName = "DurableTask.Core";
        const string RequestTrackEvent = "RequestEvent";
        const string DependencyTrackEvent = "DependencyEvent";
        const string ExceptionEvent = "ExceptionEvent";
        static DiagnosticSource logger = new DiagnosticListener(DiagnosticSourceName);
        //static IDisposable applicationInsightsSubscription = null;
        static IDisposable listenerSubscription = null;

        /// <summary>
        /// Setup this class uses callbacks to enable send telemetry to the Application Insights.
        /// You need to call this method if you want to use this class. 
        /// </summary>
        /// <param name="trackRequestTelemetryAction">Action to send request telemetry using <see cref="Activity"></see></param>
        /// <param name="trackDependencyTelemetryAction">Action to send telemetry for <see cref="Activity"/></param>
        /// <param name="trackExceptionAction">Action to send telemetry for exception </param>
        public static void SetUp(
            Action<TraceContextBase> trackRequestTelemetryAction,
            Action<TraceContextBase> trackDependencyTelemetryAction,
            Action<Exception> trackExceptionAction)
        {
            listenerSubscription = DiagnosticListener.AllListeners.Subscribe();
        }
    }
}
