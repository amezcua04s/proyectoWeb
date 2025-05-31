using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace clinicaApp.Controllers
{
    public class ExpedienteController : Controller
    {
        // GET: ExpedienteController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ExpedienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ExpedienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExpedienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExpedienteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ExpedienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ExpedienteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ExpedienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
