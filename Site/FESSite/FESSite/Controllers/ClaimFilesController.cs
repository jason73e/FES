using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FESSite.Models;
using System.IO;

namespace FESSite.Controllers
{
    public class ClaimFilesController : Controller
    {
        private FESSiteContext db = new FESSiteContext();

        // GET: ClaimFiles
        public ActionResult Index()
        {
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
                    claimFile.Filename = _FileName;
                    claimFile.FileSize = file.ContentLength.ToString();
                    db.ClaimFiles.Add(claimFile);
                    db.SaveChanges();

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
            catch(Exception ex)
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
                db.ClaimFiles.Remove(claimFile);
                db.SaveChanges();
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
