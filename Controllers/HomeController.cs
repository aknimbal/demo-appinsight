using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 
namespace demo_appinsight_linqsql.Controllers
{
    public class HomeController : Controller
    {
        public TelemetryHandler _ai= new TelemetryHandler();

        public ActionResult Index()
        {
            _ai.Pageview("System Trace");

            Trace.AutoFlush =true;
            Trace.TraceInformation("Informational telemetry using Trace");
            Trace.TraceError("Error telemetry using Trace");
            Trace.TraceWarning("Warning tlemetry using trace");
            
            return View();
           
        }

        public ActionResult About()
        {
            _ai.Pageview("Exception");

            ViewBag.Message = "";
            try
            {
                if (ViewBag.Message)
                { 
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.StackTrace);
            }
           
            return View();
        }

        public ActionResult Contact()
        {

            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Trace.TraceInformation("Informational telemetry using Trace with Telemetry client to see if it can pick same operation ID");

            _ai.Pageview("Telemetry Client");
            _ai.Info("Telemetry Client", "Informational method from Telemetry Client");
            _ai.Warn("Warn method from Telemetry Client");
            ViewBag.Message = "Telemetry Client";
            timer.Stop();
            _ai.Dependency("Total Time Taken","Contact method", "Calling both Trace and Telemetry Client", startTime, timer.Elapsed, true);
            
            var _metrics = new MetricTelemetry();
            _metrics.Name = "TTL for Contact method";
            _metrics.Sum = timer.ElapsedMilliseconds;
            _ai.CustomMetrics(_metrics);
         
            return View();
        }
    }
}