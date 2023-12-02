using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FunctionalMVC5.Models;

namespace FunctionalMVC5.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [OutputCache(Duration = 30 )]
        [AllowAnonymous]
        [Route("students")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Students.ToListAsync());
        }

        [HttpGet]
        [Route("student/{id:int}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpGet]
        [Route("create-new-student")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("create-new-student")]
        [HandleError(ExceptionType = typeof(NullReferenceException), View = "Error")]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Email,CPF,Active")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.SignupDate = DateTime.Now;
                db.Students.Add(student);
                await db.SaveChangesAsync();
                TempData["Message"] = $"Student: {student.Name} successfully registered";
                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpGet]
        [Route("edit-student/{id:int}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Message = "Não esqueça que essa ação é Irreversivel";
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Route("edit-student")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Email,CPF,Active")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.Entry(student).Property(e=> e.SignupDate).IsModified = false;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        [HttpGet]
        [Route("delete-student/{id:int}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = await db.Students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [HttpPost]
        [ActionName("Delete")]
        [Route("delete-student/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Student student = await db.Students.FindAsync(id);
            db.Students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
