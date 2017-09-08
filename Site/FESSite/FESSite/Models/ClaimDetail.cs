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

        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }


        public void ParseSource()
        {
            int iMaxPosition = 0;
            iMaxPosition = GetMaxPosition();
            if (Source.Length < iMaxPosition)
            {
                return;
            }
            foreach (Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS + 1);
            }

        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }

        public ClaimDetail(string sSource, string sVersion)
        {
            Source = sSource;
            FileVersion = sVersion;
            if (!db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.ClaimDetail).Any())
            {
                throw new Exception("File Version is " + FileVersion + ".  There are no Claim Detail Fields setup for this version.");
            }
            lsFields = db.Fields.Where(x => x.FileVersion == FileVersion && x.RecordType == RecordType.ClaimDetail).ToList();

        }
    }
}