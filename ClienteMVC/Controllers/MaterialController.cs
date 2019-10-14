using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicioWCF;
using ServicioWCF.DTO;

namespace ClienteMVC.Controllers
{
    public class MaterialController : Controller
    {
        Service proxy = new Service();
        // GET: Material
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                return View(proxy.cargarMaterial());
            }
            else
            {
                return RedirectToAction("./Login");
            }
        }

        public ActionResult IndexSerie()
        {
            return View(proxy.cargarMaterialxTipo("S"));
        }

        public ActionResult IndexPelicula()
        {
            return View(proxy.cargarMaterialxTipo("P"));
        }

        // GET: Material/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Material/Create
        public ActionResult Create()
        {
            ViewBag.Paises = proxy.cargarPaises();
            ViewBag.Generos = proxy.cargarGenero();
            Session["Elenco"] = null;
            return View();
        }

        // POST: Material/Create
        [HttpPost]
        public ActionResult Create(DTOMaterial m)
        {
            try
            {
                // TODO: Add insert logic here
                if (proxy.addMaterial(m))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Msj = "Algo salio mal.";
                    return View();
                }
            }
            catch
            {
                ViewBag.Msj = "Algo salio mal.";
                ViewBag.Paises = proxy.cargarPaises();
                ViewBag.Generos = proxy.cargarGenero();
                return View();
            }
        }

        // GET: Material/Edit/5
        public ActionResult Edit(string id)
        {
            return View(proxy.cargarMaterial(id));
        }

        // POST: Material/Edit/5
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

        // GET: Material/Delete/5
        public ActionResult Delete(string id)
        {
            return View(proxy.traerMaterial(id));
        }

        // POST: Material/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                proxy.deleteMaterial(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
