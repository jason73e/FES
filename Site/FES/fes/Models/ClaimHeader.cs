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
        private FESContext db = new FESContext();

        public string Source { get { return m_Source; } set { m_Source = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }

        public string ClaimType
        {
            get
            {
                switch (lsFields.Find(x => x.Name == "STD_BATCH_TYPE_CODE").Value)
                {
                    case "001":
                        cltLayout = ClaimLayoutType.UB04;
                        return "UB04 Inpatient";

                    case "002":
                        cltLayout = ClaimLayoutType.UB04;
                        return "UB04 Outpatient";

                    case "003":
                        cltLayout = ClaimLayoutType.Dental;
                        return "Dental";

                    case "004":
                        cltLayout = ClaimLayoutType.HCFA;
                        return "HCFA";

                    case "005":
                        cltLayout = ClaimLayoutType.UB04;
                        return "UB04 Outpatient Crossover/Advantage";

                    case "006":
                        cltLayout = ClaimLayoutType.UB04;
                        return "UB04 Inpatient Crossover/Advantage";

                    case "007":
                        cltLayout = ClaimLayoutType.HCFA;
                        return "HCFA Crossover/Advantage";

                    case "008":
                        cltLayout = ClaimLayoutType.FamilyPlanning;
                        return "Family Planning";

                    default:
                        return "UNKNOWN";
                }
            }
        }
        public ClaimLayoutType cltLayout;

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
        }
    }
}