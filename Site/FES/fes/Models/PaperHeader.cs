using System;
using System.Collections.Generic;
using System.Linq;

namespace fes.Models
{
    public class PaperHeader
    {
        public List<Field> lsFields;
        private string m_Source;
        private string m_FileVersion;
        private FESContext db = new FESContext();

        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }
        public string ClaimDCN { get { return lsFields.Find(x => x.Name == "ENDORSEMENT NUMBER").Value; } }
        public ClaimLayoutType cltLayout;


        public string ParseSource()
        {
            int iMaxPosition = 0;
            iMaxPosition = GetMaxPosition();
            if (Source.Length < iMaxPosition)
            {
                throw new Exception("File Layout Appears Invlaid.  PaperHeader record appears too short.");
            }
            if (!Source.StartsWith("PH"))
            {
                throw new Exception("File Layout Appears Invlaid.  PaperHeader record does not start with PH.");
            }
            foreach (Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS + 1);
            }

            return Source.Substring(iMaxPosition);
        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }
        public PaperHeader(string sSource, string sVersion, ClaimLayoutType clt)
        {
            Source = sSource;
            FileVersion = sVersion;
            cltLayout = clt;
            if (!db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.PaperHeader && x.ClaimType == cltLayout).Any())
            {
                throw new Exception("File Version is " + FileVersion + ".  There are no Paper Header Fields setup for this version.");
            }
            lsFields = db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.PaperHeader && x.ClaimType == cltLayout).ToList();

        }
        public PaperHeader(int iFileID, int iRecordSequence,string sVersion, ClaimLayoutType clt)
        {
            FileVersion = sVersion;
            cltLayout = clt;
            lsFields = new List<Field>();
            var query = (from c in db.FileFieldValues
                         join f in db.Fields on c.FieldID equals f.FieldID
                         where c.FileID == iFileID
                         && c.recordType == RecordType.PaperHeader
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