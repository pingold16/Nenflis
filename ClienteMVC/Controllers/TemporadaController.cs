using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicioWCF;
using ServicioWCF.DTO;

namespace ClienteMVC.Controllers
{
    public class TemporadaController : Controller
    {
        Service proxy = new Service();
        // GET: Temporada
        public ActionResult Index()
        {
            return View(proxy.traerTemporadas());
        }

        // GET: Temporada/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Temporada/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Temporada/Create
        [HttpPost]
        public ActionResult Create(DTOTemporada t)
        {
            try
            {
                // TODO: Add insert logic here
                if (proxy.addTemporada(t))
                    return RedirectToAction("Index");

                ViewBag.Msj = "Algo salio mal.";
                return View(t);
            }
            catch
            {
                return View();
            }
        }

        // GET: Temporada/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Temporada/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Temporada/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Temporada/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
