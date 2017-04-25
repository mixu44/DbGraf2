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
using MongoDB.Bson;

namespace DbGraf2.Controllers
{
    public class MeteorologiDatasController : Controller
    {
        // Our db context
        //private VillumResearchXMLEntities db = new VillumResearchXMLEntities();
        public IEnumerable<MeteorologiData> meteorologiData { get; set; }
        public IEnumerable<MercuryData> mercuryData { get; set; }
        public MeteorologiDatasController()
        {
            meteorologiData = GetMeteorologiDataFromMongoDB();
            mercuryData = GetMercuryDataFromMongoDB();

        }

        // The chart containing our data
        public static Chart Chart { get; set; }          

        // Index shows our chart
        public ActionResult Index()
        {
            // ViewModel containing data for the table.
            var viewModel = new ChartViewModel();

            // SQL
            //viewModel.Data = db.MeteorologiData.Take(100).ToList();

            // MONGODB
            viewModel.Data = meteorologiData;

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
            // SQL
            //var dbData = db.MeteorologiData.Take(100);

            // MONGODB
            var dbData = meteorologiData;


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

        //public ActionResult Mercury()
        //{
        //    var data = db.MercuryData.Take(100).ToList();
        //    return View(data);
        //}



        public IEnumerable<MeteorologiData> GetMeteorologiDataFromMongoDB()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("Meteorologi");
            IMongoCollection<MeteorologiData> collection = db.GetCollection<MeteorologiData>("meteorologi");

            List<MeteorologiData> data = new List<MeteorologiData>();

            var mongoCollection = collection.AsQueryable().Take(100);

            foreach (var item in mongoCollection)
            {
                data.Add(new MeteorologiData() {
                    StartDateTime = item.StartDateTime,
                    WindDirection = item.WindDirection,
                    WindSpeed = item.WindSpeed,
                    Temperature = item.Temperature,
                    Humidity = item.Humidity,
                    Radiation = item.Radiation,
                    Pressure = item.Pressure,
                });
            }

            return data;
        }

        public IEnumerable<MercuryData> GetMercuryDataFromMongoDB()
        {
            MongoClient client = new MongoClient("mongodb://localhost:27017");
            IMongoDatabase db = client.GetDatabase("Meteorologi");
            IMongoCollection<MercuryData> collection = db.GetCollection<MercuryData>("mercury");

            List<MercuryData> data = new List<MercuryData>();
            var mongoCollection = collection.AsQueryable().Take(100);

            foreach (var item in mongoCollection)
            {
                data.Add(new MercuryData
                {
                    DateTimeStart = item.DateTimeStart,
                    Hg = item.Hg,
                    unit = item.unit
                });
            }
            return data;
        }
    }
}
