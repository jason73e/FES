using System;
using System.Collections.Generic;
using System.Linq;

namespace FESSite.Models
{
    public class FileHeader
    {
        public List<ClaimHeader> lsdch;
        public List<Field> lsFields;
        private string m_Source;
        private FESSiteContext db = new FESSiteContext();

        public FileHeader(string sSource)
        {
            if(sSource.Length<6)
            {
                throw new Exception("File Layout Appears Invlaid.  FileHeader record appears too short.");
            }
            Source = sSource;
            string sSourceFileVersion = Source.Substring(3, 2);
            if(!db.Fields.Where(x=>x.FileVersion== sSourceFileVersion && x.RecordType == RecordType.FileHeader && x.ClaimType == ClaimLayoutType.All).Any())
            {
                throw new Exception("File Version is " + sSourceFileVersion +".  There are no FileHeader Fields setup for this version.");
            }
            lsFields = db.Fields.Where(x => x.FileVersion == sSourceFileVersion && x.RecordType== RecordType.FileHeader && x.ClaimType == ClaimLayoutType.All).ToList();

        }

        public int NumberofClaims
        {
            get
            {
                return Convert.ToInt32(lsFields.Find(x => x.Name == "NUMBER_OF_CLAIMS").Value);
            }
        }
        public string FileVersion
        {
            get
            {
                return lsFields.Find(x => x.Name == "VERSION").Value;
            }
        }

        public string Source { get { return m_Source; } set { m_Source = value; } }
        private void AddClaimHeader(ClaimHeader ch)
        {
            lsdch.Add(ch);
        }

        public void ParseSource()
        {
            int iMaxPosition = 0;
            iMaxPosition = GetMaxPosition();
            if(Source.Length<iMaxPosition)
            {
                throw new Exception("File Layout Appears Invlaid.  FileHeader record appears too short.");
            }
            foreach (Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS+1);
            }
            int iClaimsCount = NumberofClaims;
            int iClaimStartPos = GetMaxPosition();
            string sWorkingSource = Source.Substring(iClaimStartPos);
            for (int x=0; x<iClaimsCount; x++)
            {
                ClaimHeader ch = new ClaimHeader(sWorkingSource,FileVersion);
                sWorkingSource=ch.ParseSource();
                AddClaimHeader(ch);
            }

        }

        public int GetMaxPosition()
        {
            int retValue = 0;
            retValue = lsFields.Max(x => x.EndPOS);
            return retValue;
        }
    }
}