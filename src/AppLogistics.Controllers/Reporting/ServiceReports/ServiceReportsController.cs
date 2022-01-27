using AppLogistics.Components.Mvc;
using AppLogistics.Objects;
using AppLogistics.Services;
using AppLogistics.Validators;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace AppLogistics.Controllers.Reporting
{
    [Area("Reporting")]
    public class ServiceReportsController : ValidatedController<IServiceReportValidator, IServiceReportService>
    {
        private readonly string serviceReportQueryKey = "ServiceReportQuery";

        public ServiceReportsController(IServiceReportValidator validator, IServiceReportService service)
            : base(validator, service)
        {
        }

        [HttpGet]
        public ViewResult Query()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query(ServiceReportQueryView serviceReportQuery)
        {
            if (!Validator.CanQuery(serviceReportQuery))
            {
                return View(serviceReportQuery);
            }

            TempData.Put(serviceReportQueryKey, serviceReportQuery);
            return RedirectToAction("QueryResult", "ServiceReports");
        }

        [HttpGet]
        public ActionResult QueryResult()
        {
            if (TempData[serviceReportQueryKey] != null)
            {
                try
                {
                    var query = TempData.Get<ServiceReportQueryView>(serviceReportQueryKey);

                    if (!Validator.CanQuery(query))
                    {
                        return View(query);
                    }

                    TempData.Keep(serviceReportQueryKey);
                    return NotEmptyView(Service.FilterByQuery(query));
                }
                catch (Exception)
                {
                    // log exception
                    return NotFoundView();
                }
            }
            else
            {
                Alerts.AddWarning("Please do your query from the reports menu");
                return RedirectToAction("Query");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            return NotEmptyView(Service.GetDetail(id));
        }

        [HttpGet]
        public ActionResult ExportExcel()
        {
            ServiceReportQueryView query = null;

            if (TempData["ServiceReportQuery"] != null)
            {
                try
                {
                    query = TempData.Get<ServiceReportQueryView>(serviceReportQueryKey);
                }
                catch (Exception)
                {
                    // log exception
                    return NotFoundView();
                }
            }

            if (query != null)
            {
                TempData.Keep(serviceReportQueryKey);

                var csvReportBytes = Service.GetExcelReport(query);

                var reportStream = new MemoryStream(csvReportBytes);

                return new FileStreamResult(reportStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

            return NotFoundView();
        }
    }
}
