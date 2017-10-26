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
            //db.Database.ExecuteSqlCommand("Truncate Table ClaimFiles");
            //string _path = Server.MapPath("~/UploadedFiles");
            //DirectoryInfo di = new DirectoryInfo(_path);
            //foreach (FileInfo f in di.GetFiles())
            //{
                //ClaimFile c = new ClaimFile();
                //c.Filename = f.Name;
                //c.FileSize = f.Length.ToString();
                //db.ClaimFiles.Add(c);
                //db.SaveChanges();
            //}
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
                    FileInfo f = new FileInfo(_path);
                    ClaimFile c = new ClaimFile();
                    c.Parsed = false;
                    c.Filename = f.Name;
                    c.FileSize = f.Length.ToString();
                    db.ClaimFiles.Add(c);
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

        // ClaimInfo: ClaimFiles/ClaimInfo
        public ActionResult ClaimInfo(int FileID,int recordsequence,string FileVersion, ClaimLayoutType ClaimLayout, string DCN)
        {
            try
            {
                ParsedViewModel pvm = new ParsedViewModel();
                ClaimFile claimFile = db.ClaimFiles.Find(FileID);
                if (claimFile == null)
                {
                    return HttpNotFound();
                }
                int iFileID = FileID;
                pvm.cf = claimFile;
                pvm.cf.fh = new FileHeader(iFileID);
                pvm.cf.fh.lsdch = new List<ClaimHeader>();
                pvm.lsCHTVM = new List<ClaimHeaderTableViewModel>();
                ClaimHeader ch = new ClaimHeader(FileID, recordsequence,FileVersion,DCN);
                PaperHeader ph = new PaperHeader(FileID, recordsequence+1, FileVersion, ClaimLayout);
                ch.ph = ph;
                ch.lscd = new List<ClaimDetail>();
                for(int x=0;x<ch.NumberofDetails;x++)
                {
                    int iCDRecordSeq = recordsequence + 2 + (x * 2);
                    ClaimDetail cd = new ClaimDetail(FileID, iCDRecordSeq, FileVersion, ClaimLayout);
                    ch.lscd.Add(cd);
                }
                pvm.cf.fh.lsdch.Add(ch);
                return View(pvm);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ClaimFiles", "Index"));
            }
        }

        // Details: ClaimFiles/Details/5
        public ActionResult Details(int? id)
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
                int iFileID = (int)id;
                pvm.cf = claimFile;
                pvm.cf.fh = new FileHeader(iFileID);
                pvm.cf.fh.lsdch = new List<ClaimHeader>();
                pvm.lsCHTVM = new List<ClaimHeaderTableViewModel>();
                List<FileFieldValue> lsClaims = db.FileFieldValues.Where(x => x.FileID == id && x.recordType == RecordType.ClaimHeader).ToList();
                for(int i=0;i<pvm.cf.fh.NumberofClaims;i++)
                {
                    int iMinRecordSequence = lsClaims.Min(x => x.recordSequence);
                    List<FileFieldValue> lsClaim = lsClaims.Where(x => x.recordSequence == iMinRecordSequence).ToList();
                    
                    var ffvct = (from c in db.FileFieldValues
                                 join f in db.Fields on c.FieldID equals f.FieldID 
                                 where f.Name == "STD_BATCH_TYPE_CODE"
                                 && c.FileID == iFileID
                                 && c.recordType == RecordType.ClaimHeader
                                 && c.recordSequence == iMinRecordSequence
                                 select new { value = c.value });
                    string sClaimTypeCode = ffvct.First().value;
                    ClaimLayoutType clt = Utility.GetCLT(sClaimTypeCode);

                    Field fDCN = db.Fields.Where(x => x.RecordType == RecordType.PaperHeader && x.ClaimType == clt && x.FileVersion==pvm.cf.fh.FileVersion && x.Name.Trim().ToUpper() == "ENDORSEMENT NUMBER").Single();
                    FileFieldValue ffvDCN = db.FileFieldValues.Where(x => x.FieldID == fDCN.FieldID && x.FileID == id && x.recordSequence == iMinRecordSequence + 1).Single();
                    ClaimHeaderTableViewModel chtvm = new ClaimHeaderTableViewModel(iFileID, pvm.cf.fh.FileVersion,ffvDCN.value, sClaimTypeCode, iMinRecordSequence);
                    pvm.lsCHTVM.Add(chtvm);
                    foreach (FileFieldValue ffv in lsClaim)
                    {
                        lsClaims.Remove(ffv);
                    }
                }

                return View(pvm);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ClaimFiles", "Index"));
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
                pvm.cf.fh = claimFile.Parse(sSource);
                SavePVMToDB(pvm);
                db = new FESContext();
                claimFile = db.ClaimFiles.Find(id);
                claimFile.Parsed = true;
                if (claimFile != null)
                {
                    try
                    {
                        db.ClaimFiles.Attach(claimFile);
                        db.Entry(claimFile).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                return Request.IsAjaxRequest()
                    ? (ActionResult)PartialView("FileList", db.ClaimFiles.ToList())
                    : RedirectToAction("index");
                //return View(pvm);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "ClaimFiles", "Index"));
            }
        }

        private void SavePVMToDB(ParsedViewModel pvm)
        {
            try
            {
                List<FileFieldValue> ls = new List<FileFieldValue>();
                int iRecordSeq = 0;
                int FileID = pvm.cf.FileID;
                foreach(Field f in pvm.cf.fh.lsFields)
                {
                    FileFieldValue ffv = new FileFieldValue();
                    ffv.FileID = FileID;
                    ffv.FieldID = f.FieldID;
                    ffv.value = f.Value;
                    ffv.ts = DateTime.Now;
                    ffv.recordType = f.RecordType;
                    ffv.recordSequence = iRecordSeq;
                    ls.Add(ffv);
                }
                iRecordSeq++;
                foreach (ClaimHeader ch in pvm.cf.fh.lsdch)
                {

                    foreach (Field f in ch.lsFields)
                    {
                        FileFieldValue ffv = new FileFieldValue();
                        ffv.FileID = FileID;
                        ffv.FieldID = f.FieldID;
                        ffv.value = f.Value;
                        ffv.ts = DateTime.Now;
                        ffv.recordType = f.RecordType;
                        ffv.recordSequence = iRecordSeq;
                        ls.Add(ffv);
                    }
                    iRecordSeq++;
                    foreach (Field f in ch.ph.lsFields)
                    {
                        FileFieldValue ffv = new FileFieldValue();
                        ffv.FileID = FileID;
                        ffv.FieldID = f.FieldID;
                        ffv.value = f.Value;
                        ffv.ts = DateTime.Now;
                        ffv.recordType = f.RecordType;
                        ffv.recordSequence = iRecordSeq;
                        ls.Add(ffv);
                    }
                    iRecordSeq++;
                    foreach(ClaimDetail cd in ch.lscd)
                    {
                        foreach (Field f in cd.lsFields)
                        {
                            FileFieldValue ffv = new FileFieldValue();
                            ffv.FileID = FileID;
                            ffv.FieldID = f.FieldID;
                            ffv.value = f.Value;
                            ffv.ts = DateTime.Now;
                            ffv.recordType = f.RecordType;
                            ffv.recordSequence = iRecordSeq;
                            ls.Add(ffv);
                        }
                        iRecordSeq++;
                        foreach (Field f in cd.pd.lsFields)
                        {
                            FileFieldValue ffv = new FileFieldValue();
                            ffv.FileID = FileID;
                            ffv.FieldID = f.FieldID;
                            ffv.value = f.Value;
                            ffv.ts = DateTime.Now;
                            ffv.recordType = f.RecordType;
                            ffv.recordSequence = iRecordSeq;
                            ls.Add(ffv);
                        }
                        iRecordSeq++;
                    }
                }

                for (int i = 0; i < iRecordSeq; i++)
                {
                    db = new FESContext();
                    db.FileFieldValues.AddRange(ls.Where(x => x.recordSequence == i));
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
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
                List<FileFieldValue> lsValues = db.FileFieldValues.Where(x => x.FileID == id).ToList();
                db.FileFieldValues.RemoveRange(lsValues);
                db.ClaimFiles.Remove(claimFile);
                db.SaveChanges();
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
