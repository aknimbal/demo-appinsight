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
            //pageview
            _ai.Pageview("System Trace");

            //use trace method to write to application insight
            Trace.AutoFlush =true;
            Trace.TraceInformation("Informational telemetry using Trace");
            Trace.TraceError("Error telemetry using Trace");
            Trace.TraceWarning("Warning tlemetry using trace");
            
            return View();
           
        }

        public ActionResult About()
        {
            //LinqtoSQL Example
            LinqtoSQLDataContext nw = new LinqtoSQLDataContext();

            var empQuery =
                   from emp in nw.Employees
                   where emp.Id == 1
                   select emp.Name;
            
            foreach(var _emp in empQuery)
            {
                //capture SQL statement of LinqToSQL
                Trace.TraceInformation(string.Format("{0}", empQuery.ToString()));
            }

            //PageView
            _ai.Pageview("Exception");

            ViewBag.Message = "";

            //capture type conversion error
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
            //use of Trace and Telemetryclient methods

            var startTime = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            Trace.TraceInformation("Informational telemetry using Trace with Telemetry client to see if it can pick same operation ID");

            _ai.Pageview("Telemetry Client");
            _ai.Info("Telemetry Client", "Informational method from Telemetry Client");
            _ai.Warn("Warn method from Telemetry Client");
            ViewBag.Message = "Telemetry Client";
            timer.Stop();
            _ai.Dependency("Total Time Taken","Contact method", "Calling both Trace and Telemetry Client", startTime, timer.Elapsed, true);

            //custom metrics
            var _metrics = new MetricTelemetry();
            _metrics.Name = "TTL for Contact method";
            _metrics.Sum = timer.ElapsedMilliseconds;
           
            _ai.CustomMetrics(_metrics);
         
            return View();
        }
    }
}