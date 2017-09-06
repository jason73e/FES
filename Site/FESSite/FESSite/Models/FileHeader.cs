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

        public FileHeader()
        {
            lsdch = new List<ClaimHeader>();
            lsFields = new List<Field>();
            Field f = new Field { Name = "NUMBER_OF_CLAIMS", Format = "TEXT 3", StartPOS = 1, EndPOS = 3, Description = "Number of claim in the file." };
            lsFields.Add(f);
            f = new Field { Name = "Version", Format = "TEXT 2", StartPOS = 4, EndPOS = 5, Description = "Current Version"};
            lsFields.Add(f);

        }
        public int NumberofClaims
        {
            get
            {
                return Convert.ToInt32(lsFields.Find(x => x.Name == "NUMBER_OF_CLAIMS").Value);
            }
        }
        public string Version
        {
            get
            {
                return lsFields.Find(x => x.Name == "Version").Value;
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
                return;
            }
            foreach(Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS+1);
            }
            int iClaimsCount = NumberofClaims;
            int iClaimStartPos = GetMaxPosition();
            string sWorkingSource = Source.Substring(iClaimStartPos);
            for (int x=0; x<iClaimsCount; x++)
            {
                ClaimHeader ch = new ClaimHeader();
                ch.Source = sWorkingSource;
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