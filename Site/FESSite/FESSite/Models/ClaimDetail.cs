using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESSite.Models
{
    public class ClaimDetail
    {
        public List<Field> lsFields;
        private string m_Source;
        private string m_FileVersion;
        private FESSiteContext db = new FESSiteContext();
        public ClaimLayoutType cltLayout;

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
            pd = new PaperDetail(sWorkingSource.Substring(iMaxPosition), FileVersion,cltLayout);
            sWorkingSource = pd.ParseSource();
            return sWorkingSource;

        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }

        public ClaimDetail(string sSource, string sVersion , ClaimLayoutType clt)
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
    }
}