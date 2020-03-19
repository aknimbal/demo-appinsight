using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace demo_appinsight_linqsql.Controllers
{
   public class TelemetryHandler : Controller
    {
        private TelemetryClient _appInsightsClient;

        public TelemetryHandler()
        {
            _appInsightsClient = new TelemetryClient();
            //_appInsightsClient.InstrumentationKey=""
           // _appInsightsClient.InstrumentationKey = ConfigManager.Insights_Key;
        }

         ~TelemetryHandler()
        {
            _appInsightsClient.Flush();
        }

        public void Pageview(string pagename)
        {
     
            _appInsightsClient.TrackPageView(pagename);
            
        }

        public void CustomMetrics(MetricTelemetry  mymetrics)
        {
            _appInsightsClient.TrackMetric(mymetrics);
        }

        public void Info(string callingmethod,string message)
        {
            var properties = new Dictionary<string, string>();

            properties.Add("message", message);
            properties.Add("callingmethod", callingmethod);
            properties.Add("Time Captured", DateTime.Now.ToString());

            _appInsightsClient.TrackEvent("Info", properties);
        }

        public void Dependency(string Dependencytype,string DependencyCall, string message, DateTime startTime, TimeSpan ElapsedTime, bool isSuccess)
        {
            _appInsightsClient.TrackDependency(Dependencytype, DependencyCall, message, startTime, ElapsedTime, isSuccess); 
        }

        public void Warn(string message)
        {
            var properties = new Dictionary<string, string> { { "message", message } };
            _appInsightsClient.TrackEvent("Warn", properties);
            
        }

        public void Debug(string message)
        {
            var properties = new Dictionary<string, string> { { "message", message } };
            _appInsightsClient.TrackEvent("Debug", properties);
            
        }

        public void Error(string message, Exception ex)
        {
            var properties = new Dictionary<string, string> { { "message", message } };
            _appInsightsClient.TrackException(ex, properties);
            
        }

        public void Error(string message)
        {
            var properties = new Dictionary<string, string> { { "message", message } };
            Exception ex = new Exception(message);
            _appInsightsClient.TrackException(ex, properties);
            
        }

        public void Error(Exception ex)
        {
            _appInsightsClient.TrackException(ex);
            
        }
    }
}