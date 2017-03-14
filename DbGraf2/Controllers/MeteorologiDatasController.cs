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

namespace DbGraf2.Controllers
{
    public class MeteorologiDatasController : Controller
    {
        private VillumResearchXMLEntities db = new VillumResearchXMLEntities();

        // GET: MeteorologiDatas
        public ActionResult Index()
        {
            return View(db.MeteorologiData.Take(500).ToList());
        }

        // GET: MeteorologiDatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
            if (meteorologiData == null)
            {
                return HttpNotFound();
            }
            return View(meteorologiData);
        }

        // GET: MeteorologiDatas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeteorologiDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,StartDateTime,WindDirection,WindSpeed,Temperature,Humidity,Radiation,Pressure")] MeteorologiData meteorologiData)
        {
            if (ModelState.IsValid)
            {
                db.MeteorologiData.Add(meteorologiData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(meteorologiData);
        }

        // GET: MeteorologiDatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
            if (meteorologiData == null)
            {
                return HttpNotFound();
            }
            return View(meteorologiData);
        }

        // POST: MeteorologiDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,StartDateTime,WindDirection,WindSpeed,Temperature,Humidity,Radiation,Pressure")] MeteorologiData meteorologiData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meteorologiData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(meteorologiData);
        }

        // GET: MeteorologiDatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
            if (meteorologiData == null)
            {
                return HttpNotFound();
            }
            return View(meteorologiData);
        }

        // POST: MeteorologiDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MeteorologiData meteorologiData = db.MeteorologiData.Find(id);
            db.MeteorologiData.Remove(meteorologiData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CreateLineChartActionResult()
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

            var myChart = new Chart(width: 800, height: 400)
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

            var finalechart = myChart.GetBytes("png");


            return File(finalechart, "image/bytes");
        }

        public ActionResult ChartboolActionResult()
        {
            //var HentData = db.MeteorologiData.Take(500).ToString();

            //var dataChart = new Chart(width: 800, height: 400);
            //bool windSpeedY;
            //bool windDirectionY;
            //bool humidityY;
            //bool temperatureY;
            //bool dateTimeY;
            //bool radiationY;
            //bool pressureY;

            //foreach (var v in HentData)
            //{
            //}
            //if (windSpeedY)
            //{
            //    dataChart.AddSeries(yValues:  )
            //}
            


            //dataChart.AddSeries(xValue: data)
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
