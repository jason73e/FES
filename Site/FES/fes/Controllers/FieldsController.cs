using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using fes.Models;
using PagedList;

namespace fes.Controllers
{
    [Authorize]
    public class FieldsController : Controller
    {
        private FESContext db = new FESContext();

        // GET: Fields
        public ActionResult Index(string SearchName, string SearchFileVersion, ClaimLayoutType? SearchClaimLayout, RecordType? SearchRecordType, FormGrouping? SearchFormGrouping, int? page, int? PageSize, string sortOption)
        {
            TempData["MyFVM"] = null;
            ViewBag.FileVersion = SearchFileVersion;

            ViewBag.SearchName = SearchName;

            ViewBag.ClaimLayout = SearchClaimLayout;

            ViewBag.RecordType = SearchRecordType;

            ViewBag.FormGrouping = SearchFormGrouping;

            ViewBag.sortOption = sortOption;

            List<Field> fields = new List<Field>();
            if (!(String.IsNullOrEmpty(SearchName) && String.IsNullOrEmpty(SearchFileVersion) && SearchClaimLayout == null && SearchRecordType == null && SearchFormGrouping == null))
            {
                fields = db.Fields.ToList();

                if (!String.IsNullOrEmpty(SearchName))
                {
                    fields = fields.Where(x => x.DisplayName.ToLower().Contains(SearchName.ToLower()) || x.Name.ToLower().Contains(SearchName.ToLower())).ToList();
                }

                if (!String.IsNullOrEmpty(SearchFileVersion))
                {
                    fields = fields.Where(x => x.FileVersion == SearchFileVersion).ToList();
                }

                if (SearchClaimLayout != null)
                {
                    fields = fields.Where(x => x.ClaimType == SearchClaimLayout).ToList();
                }
                if (SearchRecordType != null)
                {
                    fields = fields.Where(x => x.RecordType == SearchRecordType).ToList();
                }
                if (SearchFormGrouping != null)
                {
                    fields = fields.Where(x => x.FormGroup == SearchFormGrouping).ToList();
                }
                switch (sortOption)
                {
                    case "name_acs":
                        fields = fields.OrderBy(p => p.Name).ToList();
                        break;
                    case "name_desc":
                        fields = fields.OrderByDescending(p => p.Name).ToList();
                        break;
                    case "description_acs":
                        fields = fields.OrderBy(p => p.Description).ToList();
                        break;
                    case "description_desc":
                        fields = fields.OrderByDescending(p => p.Description).ToList();
                        break;
                    case "displayname_acs":
                        fields = fields.OrderBy(p => p.DisplayName).ToList();
                        break;
                    case "displayname_desc":
                        fields = fields.OrderByDescending(p => p.DisplayName).ToList();
                        break;
                    case "pseudocode_acs":
                        fields = fields.OrderBy(p => p.PseudoCode).ToList();
                        break;
                    case "pseudocode_desc":
                        fields = fields.OrderByDescending(p => p.PseudoCode).ToList();
                        break;
                    case "fileversion_acs":
                        fields = fields.OrderBy(p => p.FileVersion).ToList();
                        break;
                    case "fileversion_desc":
                        fields = fields.OrderByDescending(p => p.FileVersion).ToList();
                        break;
                    case "recordtype_acs":
                        fields = fields.OrderBy(p => p.RecordType).ToList();
                        break;
                    case "recordtype_desc":
                        fields = fields.OrderByDescending(p => p.RecordType).ToList();
                        break;
                    case "claimtype_acs":
                        fields = fields.OrderBy(p => p.ClaimType).ToList();
                        break;
                    case "claimtype_desc":
                        fields = fields.OrderByDescending(p => p.ClaimType).ToList();
                        break;
                    case "formgroup_acs":
                        fields = fields.OrderBy(p => p.FormGroup).ToList();
                        break;
                    case "formgroup_desc":
                        fields = fields.OrderByDescending(p => p.FormGroup).ToList();
                        break;
                    default:
                        fields = fields.OrderBy(p => p.FieldID).ToList();
                        break;

                }
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
            fvm.lsFields = fields.ToPagedList(pageNumber, DefaultPageSize);
            fvm.slFileVersions = Utility.GetFileVersions();
            fvm.SearchFileVersion = SearchFileVersion;
            fvm.SearchClaimLayout = SearchClaimLayout;
            TempData["MyFVM"] = fvm;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("FieldList", fvm.lsFields)
                : View(fvm);
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
        public ActionResult Create([Bind(Include = "FieldID,Name,Format,StartPOS,EndPOS,Description,DisplayName,PseudoCode,FormFieldName,FormFieldPosition,WebDEFieldName,Comments,FileVersion,RecordType,ClaimType,IsDisplayed,FormGroup")] Field field)
        {
            if (ModelState.IsValid)
            {
                field.ts = DateTime.Now;
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
        public ActionResult Edit([Bind(Include = "FieldID,Name,Format,StartPOS,EndPOS,Description,DisplayName,PseudoCode,FormFieldName,FormFieldPosition,WebDEFieldName,Comments,FileVersion,RecordType,ClaimType,IsDisplayed,FormGroup")] Field field)
        {
            if (ModelState.IsValid)
            {
                field.ts = DateTime.Now;
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
