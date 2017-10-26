using System;
using System.Collections.Generic;
using System.Linq;

namespace fes.Models
{
    public class ClaimHeaderTableViewModel
    {
        private int m_iFileID;
        private string m_FileVersion;
        private string m_DCN;
        private string m_ClaimType;
        private ClaimLayoutType m_cltClaimType;
        private FESContext db = new FESContext();
        private int m_iRecordSequence;

        public string DCN { get { return m_DCN; } set { m_DCN = value; } }
        public string FileVersion { get { return m_FileVersion; } set { m_FileVersion = value; } }
        public ClaimLayoutType cltLayout { get { return m_cltClaimType; } set { m_cltClaimType = value; } }
        public int FileID { get { return m_iFileID; }set { m_iFileID = value; } }
        public string ClaimType { get { return m_ClaimType; } set { m_ClaimType = value; } }
        public int RecordSequence { get { return m_iRecordSequence; }set { m_iRecordSequence = value; } }

        public ClaimHeaderTableViewModel(int iFileid,string sFileVersion,string sDCN,string sClaimTypeCode,int iRecordSequence)
        {
            FileID = iFileid;
            FileVersion = sFileVersion;
            DCN = sDCN;
            RecordSequence = iRecordSequence;
            cltLayout = Utility.GetCLT(sClaimTypeCode);
            ClaimType = Utility.GetClaimType(sClaimTypeCode);
        }

    }
}