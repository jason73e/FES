﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FESSite.Models;
using PagedList;

namespace FESSite.Controllers
{
    public class FieldsController : Controller
    {
        private FESSiteContext db = new FESSiteContext();

        // GET: Fields
        public ActionResult Index(string sCurrentFileVersion, string SearchFileVersion, ClaimLayoutType? sCurrentClaimLayoutType, ClaimLayoutType? SearchClaimLayout,  int? page, int? PageSize)
        {
            TempData["MyFVM"] = null;
            if (SearchFileVersion != null)
            {
                page = 1;
            }
            else
            {
                SearchFileVersion = sCurrentFileVersion;
            }
            ViewBag.FileVersion = SearchFileVersion;

            if (SearchClaimLayout != null)
            {
                page = 1;
            }
            else
            {
                SearchClaimLayout = sCurrentClaimLayoutType;
            }
            ViewBag.ClaimLayout = SearchClaimLayout;

            var fields = db.Fields.Where(x=>x.FileVersion== SearchFileVersion).ToList();
            if(SearchClaimLayout!=null)
            {
                fields = fields.Where(x => x.ClaimType == SearchClaimLayout).ToList();
            }
            int DefaultPageSize = 10;
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;
            if (PageSize != null)
            {
                DefaultPageSize = (int)PageSize;
            }
            ViewBag.PageSize = DefaultPageSize;
            int pageNumber = (page ?? 1);
            ViewBag.Page = page;
            FieldsViewModel fvm = new FieldsViewModel();
            fvm.lsFields = fields.ToPagedList(pageNumber,DefaultPageSize);
            fvm.slFileVersions = Utility.GetFileVersions();
            fvm.SearchFileVersion = SearchFileVersion;
            fvm.SearchClaimLayout = SearchClaimLayout;
            TempData["MyFVM"] = fvm;
            return View(fvm);
        }

        // GET: Fields/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // GET: Fields/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FieldID,Name,Format,StartPOS,EndPOS,Description,DisplayName,PseudoCode,FormFieldName,FormFieldPosition,WebDEFieldName,Comments,FileVersion,RecordType,ClaimType,ts,Value")] Field field)
        {
            if (ModelState.IsValid)
            {
                db.Fields.Add(field);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(field);
        }

        // GET: Fields/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // POST: Fields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FieldID,Name,Format,StartPOS,EndPOS,Description,DisplayName,PseudoCode,FormFieldName,FormFieldPosition,WebDEFieldName,Comments,FileVersion,RecordType,ClaimType,ts,Value")] Field field)
        {
            if (ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(field);
        }

        // GET: Fields/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        // POST: Fields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Field field = db.Fields.Find(id);
            db.Fields.Remove(field);
            db.SaveChanges();
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
