using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fes.Models
{
    public class ClaimDetail
    {
        public List<Field> lsFields;
        private string m_Source;
        private string m_FileVersion;
        private FESContext db = new FESContext();
        public ClaimLayoutType cltLayout;
        public string ClaimDetailNumber { get { return lsFields.Find(x => x.Name == "SEQUECNE NUMBER").Value; } }


        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }
        public PaperDetail pd;

        public string ParseSource()
        {
            int iMaxPosition = 0;
            string sWorkingSource = Source;
            iMaxPosition = GetMaxPosition();
            if (Source.Length < iMaxPosition)
            {
                throw new Exception("File Layout Appears Invlaid.  Claim Detail record appears too short.");
            }
            if (!Source.StartsWith("ID"))
            {
                throw new Exception("File Layout Appears Invlaid.  Claim Detail record appears does not start with ID.");
            }
            foreach (Field f in lsFields)
            {
                f.Value = sWorkingSource.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS + 1);
            }
            pd = new PaperDetail(sWorkingSource.Substring(iMaxPosition), FileVersion, cltLayout);
            sWorkingSource = pd.ParseSource();
            return sWorkingSource;

        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }

        public ClaimDetail(string sSource, string sVersion, ClaimLayoutType clt)
        {
            Source = sSource;
            cltLayout = clt;
            FileVersion = sVersion;
            if (!db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.ClaimDetail && x.ClaimType == cltLayout).Any())
            {
                throw new Exception("File Version is " + FileVersion + ".  There are no Claim Detail Fields setup for this version.");
            }
            lsFields = db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.ClaimDetail && x.ClaimType == cltLayout).ToList();

        }
        public ClaimDetail(int iFileID, int iRecordSequence, string sVersion, ClaimLayoutType clt)
        {
            cltLayout = clt;
            FileVersion = sVersion;
            lsFields = new List<Field>();
            var query = (from c in db.FileFieldValues
                         join f in db.Fields on c.FieldID equals f.FieldID
                         where c.FileID == iFileID
                         && c.recordType == RecordType.ClaimDetail
                         && c.recordSequence == iRecordSequence
                         select new
                         {
                             ClaimType = f.ClaimType,
                             Comments = f.Comments,
                             Description = f.Description,
                             DisplayName = f.DisplayName,
                             EndPOS = f.EndPOS,
                             FieldID = f.FieldID,
                             FileVersion = f.FileVersion,
                             Format = f.Format,
                             FormFieldName = f.FormFieldName,
                             FormFieldPosition = f.FormFieldPosition,
                             FormGroup = f.FormGroup,
                             IsDisplayed = f.IsDisplayed,
                             Name = f.Name,
                             PseudoCode = f.PseudoCode,
                             RecordType = f.RecordType,
                             StartPOS = f.StartPOS,
                             ts = c.ts,
                             Value = c.value,
                             WebDEFieldName = f.WebDEFieldName
                         });
            var vfields = query.ToList().Select(r => new Field
            {
                ClaimType = r.ClaimType,
                Comments = r.Comments,
                Description = r.Description,
                DisplayName = r.DisplayName,
                EndPOS = r.EndPOS,
                FieldID = r.FieldID,
                FileVersion = r.FileVersion,
                Format = r.Format,
                FormFieldName = r.FormFieldName,
                FormFieldPosition = r.FormFieldPosition,
                FormGroup = r.FormGroup,
                IsDisplayed = r.IsDisplayed,
                Name = r.Name,
                PseudoCode = r.PseudoCode,
                RecordType = r.RecordType,
                StartPOS = r.StartPOS,
                ts = r.ts,
                Value = r.Value,
                WebDEFieldName = r.WebDEFieldName
            }).ToList();
            lsFields.AddRange(vfields);
        }
    }
}