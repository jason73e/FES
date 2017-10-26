using System;
using System.Collections.Generic;
using System.Linq;

namespace fes.Models
{
    public class ClaimHeader
    {
        public List<Field> lsFields;
        private string m_Source;
        private string m_FileVersion;
        private string m_DCN;
        private FESContext db = new FESContext();

        public string DCN { get { return m_DCN; } set { m_DCN = value; } }
        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }

        public string ClaimType
        {
            get
            {
                return Utility.GetClaimType(lsFields.Find(x => x.Name == "STD_BATCH_TYPE_CODE").Value);
            }
        }
        public ClaimLayoutType cltLayout { get { return Utility.GetCLT(lsFields.Find(x => x.Name == "STD_BATCH_TYPE_CODE").Value); } }

        public int NumberofDetails
        {
            get
            {
                return Convert.ToInt32(lsFields.Find(x => x.Name == "NUMBER_OF_DETAILS").Value);
            }
        }

        public PaperHeader ph;
        public List<ClaimDetail> lscd = new List<ClaimDetail>();

        public string ParseSource()
        {
            int iMaxPosition = 0;
            string sWorkingSource = Source;
            iMaxPosition = GetMaxPosition();
            if (Source.Length < iMaxPosition)
            {
                throw new Exception("File Layout Appears Invlaid.  ClaimHeader record appears too short.");
            }
            if (!Source.StartsWith("30"))
            {
                throw new Exception("File Layout Appears Invlaid.  ClaimHeader record appears does not start with 30.");
            }
            foreach (Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS + 1);
            }
            RemoveExtraFields();
            ph = new PaperHeader(sWorkingSource.Substring(iMaxPosition), FileVersion, cltLayout);
            DCN = ph.ClaimDCN;
            sWorkingSource = ph.ParseSource();
            for (int x = 0; x < NumberofDetails; x++)
            {
                ClaimDetail cd = new ClaimDetail(sWorkingSource, FileVersion, cltLayout);
                sWorkingSource = cd.ParseSource();
                lscd.Add(cd);
            }


            return sWorkingSource;

        }

        private void RemoveExtraFields()
        {
            List<Field> lsNewList = new List<Field>();
            string thisClaimType = ClaimType;
            switch (thisClaimType)
            {
                case "UB04 Inpatient":
                case "UB04 Outpatient":
                    foreach (Field f in lsFields)
                    {
                        if ((f.ClaimType == ClaimLayoutType.UB04))
                        {
                            lsNewList.Add(f);
                        }
                    }
                    lsFields = lsNewList;
                    break;
                case "Dental":
                    foreach (Field f in lsFields)
                    {
                        if ((f.ClaimType == ClaimLayoutType.Dental))
                        {
                            lsNewList.Add(f);
                        }
                    }
                    lsFields = lsNewList;
                    break;
                case "HCFA":
                    foreach (Field f in lsFields)
                    {
                        if ((f.ClaimType == ClaimLayoutType.HCFA))
                        {
                            lsNewList.Add(f);
                        }
                    }
                    lsFields = lsNewList;
                    break;
                case "Family Planning":
                    foreach (Field f in lsFields)
                    {
                        if ((f.ClaimType == ClaimLayoutType.FamilyPlanning))
                        {
                            lsNewList.Add(f);
                        }
                    }
                    lsFields = lsNewList;
                    break;
                case "UB04 Outpatient Crossover/Advantage":
                case "UB04 Inpatient Crossover/Advantage":
                    foreach (Field f in lsFields)
                    {
                        if (((f.ClaimType == ClaimLayoutType.UB04Advantage) || (f.ClaimType == ClaimLayoutType.UB04Crossover)))
                        {
                            lsNewList.Add(f);
                        }
                    }
                    lsFields = lsNewList;
                    break;
                case "HCFA Crossover/Advantage":
                    foreach (Field f in lsFields)
                    {
                        if (((f.ClaimType == ClaimLayoutType.HCFAAdvantage) || (f.ClaimType == ClaimLayoutType.HCFACrossover)))
                        {
                            lsNewList.Add(f);
                        }
                    }
                    lsFields = lsNewList;
                    break;
            }
        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }
        public ClaimHeader(string sSource, string sVersion)
        {
            Source = sSource;
            FileVersion = sVersion;
            if (!db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.ClaimHeader).Any())
            {
                throw new Exception("File Version is " + FileVersion + ".  There are no Claim Header Fields setup for this version.");
            }
            lsFields = db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.ClaimHeader).ToList();
            string s = ClaimType;

        }

        public ClaimHeader(int iFileID,int iRecordSequence, string sFileVersion,string sDCN)
        {
            DCN = sDCN;
            FileVersion = sFileVersion;
            lsFields = new List<Field>();
            var query = (from c in db.FileFieldValues
                         join f in db.Fields on c.FieldID equals f.FieldID
                         where c.FileID == iFileID
                         && c.recordType == RecordType.ClaimHeader
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