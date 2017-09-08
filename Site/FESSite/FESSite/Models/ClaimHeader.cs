using System;
using System.Collections.Generic;
using System.Linq;

namespace FESSite.Models
{
    public class ClaimHeader
    {
        public List<Field> lsFields;
        private string m_Source;
        private string m_FileVersion;
        private FESSiteContext db = new FESSiteContext();

        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }

        public string ClaimType
        {
            get
            {
                switch(lsFields.Find(x => x.Name == "STD_BATCH_TYPE_CODE").Value)
                {
                    case "001":
                        return "UB04 Inpatient";

                    case "002":
                        return "UB04 Outpatient";

                    case "003":
                        return "Dental";

                    case "004":
                        return "HCFA";

                    case "005":
                        return "UB04 Outpatient Crossover/Advantage";

                    case "006":
                        return "UB04 Inpatient Crossover/Advantage";

                    case "007":
                        return "HCFA Advantage/Crossover";

                    case "008":
                        return "Family Planning";

                    default:
                        return "UNKNOWN";
                }
            }
        }

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
            int iWorkingPosition = 0;
            iMaxPosition = GetMaxPosition();
            iWorkingPosition = iMaxPosition;
            if (Source.Length < iMaxPosition)
            {
                throw new Exception("File Layout Appears Invlaid.  ClaimHeader record appears too short.");
            }
            if(!Source.StartsWith("30"))
            {
                throw new Exception("File Layout Appears Invlaid.  ClaimHeader record appears does not start with 30.");
            }
            foreach (Field f in lsFields)
            {
                f.Value = Source.Substring(f.StartPOS - 1, f.EndPOS - f.StartPOS + 1);
            }
            ph = new PaperHeader(Source.Substring(iWorkingPosition), FileVersion);
            ph.ParseSource();


            return Source.Substring(iWorkingPosition);

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
        }
    }
}