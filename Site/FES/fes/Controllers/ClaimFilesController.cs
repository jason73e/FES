using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using fes.Models;
using System.IO;

namespace fes.Controllers
{
    [Authorize]
    public class ClaimFilesController : Controller
    {
        private FESContext db = new FESContext();

        // GET: ClaimFiles
        public ActionResult Index()
        {
            db.Database.ExecuteSqlCommand("Truncate Table ClaimFiles");
            string _path = Server.MapPath("~/UploadedFiles");
            DirectoryInfo di = new DirectoryInfo(_path);
            foreach (FileInfo f in di.GetFiles())
            {
                ClaimFile c = new ClaimFile();
                c.Filename = f.Name;
                c.FileSize = f.Length.ToString();
                db.ClaimFiles.Add(c);
                db.SaveChanges();
            }
            return View(db.ClaimFiles.ToList());
        }

        // GET: ClaimFiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClaimFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file)
        {
            ClaimFile claimFile = new ClaimFile();
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return View();
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }

        }

        // Parse: ClaimFiles/Parse/5
        public ActionResult Parse(int? id)
        {
            try
            {
                ParsedViewModel pvm = new ParsedViewModel();
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ClaimFile claimFile = db.ClaimFiles.Find(id);
                if (claimFile == null)
                {
                    return HttpNotFound();
                }
                string _FileName = claimFile.Filename;
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                string sSource = System.IO.File.ReadAllText(_path);
                pvm.cf = claimFile;
                pvm.fh = claimFile.Parse(sSource);

                return View(pvm);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ClaimFiles", "Index"));
            }
        }


        // GET: ClaimFiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimFile claimFile = db.ClaimFiles.Find(id);
            if (claimFile == null)
            {
                return HttpNotFound();
            }
            return View(claimFile);
        }

        // POST: ClaimFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                ClaimFile claimFile = db.ClaimFiles.Find(id);
                string _FileName = claimFile.Filename;
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                System.IO.File.Delete(_path);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ClaimFiles", "Index"));
            }
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
