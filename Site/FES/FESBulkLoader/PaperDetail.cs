using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fes.Models
{
    public class PaperDetail
    {
        public List<Field> lsFields;
        private string m_Source;
        private string m_FileVersion;
        private FESContext db = new FESContext();
        public ClaimLayoutType cltLayout;

        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }

        public string ParseSource()
        {
            int iMaxPosition = 0;
            string sWorkingSource = Source;
            iMaxPosition = GetMaxPosition();
            if (Source.Length < iMaxPosition)
            {
                throw new Exception("File Layout Appears Invlaid.  Paper Detail record appears too short.");
            }
            if (!Source.StartsWith("PD"))
            {
                throw new Exception("File Layout Appears Invlaid.  Paper Detail record appears does not start with PD.");
            }
            foreach (Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS + 1);
            }
            return sWorkingSource.Substring(iMaxPosition);
        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }
        public PaperDetail(string sSource, string sVersion, ClaimLayoutType clt)
        {
            Source = sSource;
            cltLayout = clt;
            FileVersion = sVersion;
            if (!db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.PaperDetail && x.ClaimType == cltLayout).Any())
            {
                throw new Exception("File Version is " + FileVersion + ".  There are no Paper Detail Fields setup for this version.");
            }
            lsFields = db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.PaperDetail && x.ClaimType == cltLayout).ToList();
        }
    }
}