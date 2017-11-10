using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using fes;
using System.Text;
using System.Threading.Tasks;
using fes.Models;
using System.Data.Entity;
using System.Data.SqlClient;

namespace FESBulkLoader
{
    public class BulkLoader
    {
        private FESContext db = new FESContext();

        public BulkLoader()
        {
        }
        public void Load()
        {
            DirectoryInfo d = new DirectoryInfo(ConfigurationManager.AppSettings["ProcessPath"]);
            foreach (FileInfo f in d.GetFiles())
            {
                try
                {
                    ClaimFile c = new ClaimFile();
                    c.Parsed = false;
                    c.Filename = f.Name;
                    c.FileSize = f.Length.ToString();
                    c.ts = DateTime.Now;
                    db.ClaimFiles.Add(c);
                    db.SaveChanges();
                    c = db.ClaimFiles.Where(x => x.Filename == f.Name).Single();
                    if (Parse(c.FileID, f.FullName))
                    {
                        f.Delete();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Remove()
        {
            try
            {
                int iPurgeHours = Convert.ToInt32(ConfigurationManager.AppSettings["Purge"]);
                iPurgeHours = iPurgeHours * -1;
                DateTime dtPurge = DateTime.Now.AddHours(iPurgeHours);
                List<ClaimFile> lsFiles = db.ClaimFiles.Where(x => x.ts < dtPurge).ToList();
                foreach(ClaimFile claimFile in lsFiles)
                {
                    db.Database.ExecuteSqlCommand("Delete FileFieldValues where FileID =" + claimFile.FileID.ToString());
                    db.Database.ExecuteSqlCommand("Delete ClaimFiles where FileID =" + claimFile.FileID.ToString());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private Boolean Parse(int? id, string sPath)
        {
            try
            {
                ParsedViewModel pvm = new ParsedViewModel();
                if (id == null)
                {
                    return false;
                }
                ClaimFile claimFile = db.ClaimFiles.Find(id);
                if (claimFile == null)
                {
                    return false;
                }
                string sSource = System.IO.File.ReadAllText(sPath);
                pvm.cf = claimFile;
                pvm.cf.fh = claimFile.Parse(sSource);
                SavePVMToDB(pvm);
                db = new FESContext();
                claimFile = db.ClaimFiles.Find(id);
                claimFile.Parsed = true;
                claimFile.DocType = pvm.cf.fh.lsdch[0].ClaimType;
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
                        throw ex;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void SavePVMToDB(ParsedViewModel pvm)
        {
            try
            {
                List<FileFieldValue> ls = new List<FileFieldValue>();
                int iRecordSeq = 0;
                int FileID = pvm.cf.FileID;
                foreach (Field f in pvm.cf.fh.lsFields)
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
                    foreach (ClaimDetail cd in ch.lscd)
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
                if (ls.Count > 0)
                {
                    using (SqlConnection sc = new SqlConnection(ConfigurationManager.ConnectionStrings["FESSiteContext"].ConnectionString))
                    {
                        sc.Open();
                        InsertFileFieldValues(sc, ls);
                        sc.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void InsertFileFieldValues(SqlConnection sqlConnection, IEnumerable<FileFieldValue> values)
        {
            var tableName = "FileFieldValues";
            var bufferSize = 5000;
            var inserter = new BulkInserter<FileFieldValue>(sqlConnection, tableName, bufferSize);
            inserter.Insert(values);
        }
    }
}
