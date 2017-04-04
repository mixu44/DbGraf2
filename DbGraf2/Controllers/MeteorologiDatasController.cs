using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DbGraf2.Models;
using DbGraf2.viewmodels;
using MongoDB.Driver;

namespace DbGraf2.Controllers
{
    public class MeteorologiDatasController : Controller
    {
        // Our db context
        private VillumResearchXMLEntities db = new VillumResearchXMLEntities();
        

        // The chart containing our data
        public static Chart Chart { get; set; }          

        // Index shows our chart
        public ActionResult Index()
        {
            // ViewModel containing data for the table.
            var viewModel = new ChartViewModel();

            // Get table data from the db.
            viewModel.Data = db.MeteorologiData.Take(100).ToList();

            // Show the data in the view
            return View(viewModel);
        }

        public ActionResult GetChart()
        {
            var finalechart = Chart.GetBytes("png");
            return File(finalechart, "image/bytes");
        }

        // This method creates and set our chart.
        [HttpPost]
        public ActionResult PopulateChart(ChartViewModel viewModel)
        {
            // Get data for our chart
            var dbData = db.MeteorologiData.Take(100);

            // Instantiate our chart
            Chart = new Chart(width: 800, height: 400);

            // Prepare lists to contain the different data
            var DataWindSpeed = new List<double?>();
            var DataWindDirection = new List<double?>();
            var DataHumidity = new List<double?>();
            var DataTemperature = new List<double?>();
            var DataPressure = new List<double?>();
            var DataRadiation = new List<double?>();
            var DataDateTime = new List<DateTime?>();

            // Populate the lists with our db data
            foreach (var data in dbData)
            {
                DataWindSpeed.Add(data.WindSpeed);
                DataWindDirection.Add(data.WindDirection);
                DataHumidity.Add(data.Humidity);
                DataTemperature.Add(data.Temperature);
                DataPressure.Add(data.Pressure);
                DataRadiation.Add(data.Radiation);
                DataDateTime.Add(data.StartDateTime);
            }

            // We add data to the chart according to checkboxes checked off
            if (viewModel.ShowWindSpeed)
                Chart.AddSeries(xValue: DataDateTime, yValues: DataWindSpeed.ToArray());
            if (viewModel.ShowWindDirection)
                Chart.AddSeries(xValue: DataDateTime, yValues: DataWindDirection.ToArray());
            if (viewModel.ShowHumidity)
                Chart.AddSeries(xValue: DataDateTime, yValues: DataHumidity.ToArray());
            if (viewModel.ShowTemperature)
                Chart.AddSeries(xValue: DataDateTime, yValues: DataTemperature.ToArray());
            if (viewModel.ShowPressure)
                Chart.AddSeries(xValue: DataDateTime, yValues: DataPressure.ToArray());
            if (viewModel.ShowRadiation)
                Chart.AddSeries(xValue: DataDateTime, yValues: DataRadiation.ToArray());

            // The chart has been created and populated with data, so we return to the index view
            return RedirectToAction("Index");
        }

        public ActionResult Mercury()
        {
            var data = db.MercuryData.Take(100).ToList();
            return View(data);
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
