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

namespace DbGraf2.Controllers
{
    public class MeteorologiDatasController : Controller
    {
        private VillumResearchXMLEntities db = new VillumResearchXMLEntities();

        public static Chart Chart { get; set; }          

        // GET: MeteorologiDatas
        public ActionResult Index()
        {
            var viewModel = new ChartViewModel();

            viewModel.Data = db.MeteorologiData.Take(500).ToList();
            return View(viewModel);
        }

        //// GET: MeteorologiDatas/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
        //    if (meteorologiData == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(meteorologiData);
        //}

        //// GET: MeteorologiDatas/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: MeteorologiDatas/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,StartDateTime,WindDirection,WindSpeed,Temperature,Humidity,Radiation,Pressure")] MeteorologiData meteorologiData)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.MeteorologiData.Add(meteorologiData);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(meteorologiData);
        //}

        //// GET: MeteorologiDatas/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
        //    if (meteorologiData == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(meteorologiData);
        //}

        //// POST: MeteorologiDatas/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,StartDateTime,WindDirection,WindSpeed,Temperature,Humidity,Radiation,Pressure")] MeteorologiData meteorologiData)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(meteorologiData).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(meteorologiData);
        //}

        //// GET: MeteorologiDatas/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
        //    if (meteorologiData == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(meteorologiData);
        //}

        //// POST: MeteorologiDatas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
        //    db.MeteorologiData.Remove(meteorologiData);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult CreateLineChartActionResult()
        {
            
            var finalechart = Chart.GetBytes("png");
            return File(finalechart, "image/bytes");
        }

        public void CreateChart()
        {
            var dataHent = db.MeteorologiData.Take(500);
            var data1 = new List<DateTime?>();
            var data2 = new List<double?>();
            var data3 = new List<double?>();

            foreach (var data in dataHent)
            {
                data1.Add(data.StartDateTime);
                data2.Add(data.WindSpeed);
                data3.Add(data.Humidity);

            }

            Chart = new Chart(width: 800, height: 400)
                .AddTitle("Data")
                .AddSeries(
                    name: "WindsSpeeds",
                    chartType: "Line",
                    xValue: data1.ToArray(),
                    yValues: data2.ToArray()
                    )

                .AddSeries(
                    name: "Temperature",
                    chartType: "Line",
                    xValue: data1.ToArray(),
                    yValues: data3.ToArray());
        }

        [HttpPost, ActionName("ChartBool")]
        public ActionResult ChartboolActionResult(ChartViewModel viewModel)
        {
            var HentData = db.MeteorologiData.Take(500);

             Chart = new Chart(width: 800, height: 400);

            var DataWindSpeed = new List<double?>();
            var DataWindDirection = new List<double?>();
            var DataHumidity = new List<double?>();
            var DataTemperature = new List<double?>();
            var DataPressure = new List<double?>();
            var DataRadiation = new List<double?>();
            var DataDateTime = new List<DateTime?>();

            foreach (var data in HentData)
            {
                DataWindSpeed.Add(data.WindSpeed);
                DataWindDirection.Add(data.WindDirection);
                DataHumidity.Add(data.Humidity);
                DataTemperature.Add(data.Temperature);
                DataPressure.Add(data.Pressure);
                DataRadiation.Add(data.Radiation);
                DataDateTime.Add(data.StartDateTime);



            }
            if (viewModel.ShowWindSpeed)
            {
                Chart.AddSeries(xValue: DataDateTime, yValues: DataWindSpeed.ToArray());
               
            }
            if (viewModel.ShowWindDirection)
            {
                Chart.AddSeries(xValue: DataDateTime, yValues: DataWindDirection.ToArray());
            }
            if (viewModel.ShowHumidity)
            {
                Chart.AddSeries(xValue: DataDateTime, yValues: DataHumidity.ToArray());
            }
            if (viewModel.ShowTemperature)
            {
                Chart.AddSeries(xValue: DataDateTime, yValues: DataTemperature.ToArray());
            }
            if (viewModel.ShowPressure)
            {
                Chart.AddSeries(xValue: DataDateTime, yValues: DataPressure.ToArray());
            }
            if (viewModel.ShowRadiation)
            {
                Chart.AddSeries(xValue: DataDateTime, yValues: DataRadiation.ToArray());
            }


            return RedirectToAction("Index");



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
