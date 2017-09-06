using System;
using System.Collections.Generic;
using System.Linq;

namespace FESSite.Models
{
    public class PaperHeader
    {
        public List<Field> lsFields;
        private string m_Source;
        public string Source { get { return m_Source; } set { m_Source = value; } }

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
        public PaperHeader()
        {
            lsFields = new List<Field>();
            Field f = new Field();
            f = new Field { Name = "RECORD_ID", Format = "TEXT 2", StartPOS = 1, EndPOS = 2, Description = "VALUE \"PH\"" };
            lsFields.Add(f);
            f = new Field { Name = "ENDORSEMENT_NUMBER", Format = "TEXT 15", StartPOS = 3, EndPOS = 17, Description = "Number assigned to paper claim by imaging" };
            lsFields.Add(f);
            f = new Field { Name = "BPD_STREET", Format = "TEXT 30", StartPOS = 18, EndPOS = 47, Description = "BILLING PROVIDER STREET" };
            lsFields.Add(f);
            f = new Field { Name = "BPD_CITY", Format = "TEXT 20", StartPOS = 48, EndPOS = 67, Description = "BILLING PROVIDER CITY " };
            lsFields.Add(f);
            f = new Field { Name = "BPD_STATE", Format = "TEXT 2", StartPOS = 68, EndPOS = 69, Description = "BILLING PROVIDER STATE" };
            lsFields.Add(f);
            f = new Field { Name = "BPD_ZIP_CODE", Format = "TEXT 10", StartPOS = 70, EndPOS = 79, Description = "BILLING PROVIDER ZIP CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_DATA_ENTRY_EOB_CODE", Format = "TEXT 5", StartPOS = 80, EndPOS = 84, Description = "EOB entered by data entry clerk" };
            lsFields.Add(f);
            f = new Field { Name = "FK_ATTACHMENT_CODE", Format = "TEXT 1", StartPOS = 85, EndPOS = 85, Description = "Code to identify claim attachments" };
            lsFields.Add(f);
            f = new Field { Name = "ATTACHMENT_DATE", Format = "TEXT 8", StartPOS = 86, EndPOS = 93, Description = "Date used to override late filing based on claim attachment" };
            lsFields.Add(f);
            f = new Field { Name = "FK_ATTACHMENT_DISP_CODE", Format = "TEXT 1", StartPOS = 94, EndPOS = 94, Description = "Disposition from other insurance based on claim attachment" };
            lsFields.Add(f);
            f = new Field { Name = "MCARE_TOTAL_BILLED_AMT", Format = "TEXT 11", StartPOS = 95, EndPOS = 105, Description = "Used to enter amount from claim form attached to Medicare" };
            lsFields.Add(f);
            f = new Field { Name = "SIGNATURE_INDICATOR", Format = "TEXT 1", StartPOS = 106, EndPOS = 106, Description = "SIGNATURE INDICATOR" };
            lsFields.Add(f);
            f = new Field { Name = "CILENT_DEATCH_INDICATOR", Format = "TEXT 1", StartPOS = 107, EndPOS = 107, Description = "Used when no date of death is submitted - valid values Y, N, space" };
            lsFields.Add(f);
            f = new Field { Name = "VIS_EYEGLASS_SIGNATURE_IND", Format = "TEXT 1", StartPOS = 108, EndPOS = 108, Description = "VISION EYEGLASS SIGNATURE INDICATOR" };
            lsFields.Add(f);
            f = new Field { Name = "CART_MEMO_NUMBER", Format = "TEXT 16", StartPOS = 109, EndPOS = 124, Description = "CARTS number required for claims entered from CARTS request" };
            lsFields.Add(f);
            f = new Field { Name = "T90_INDICATOR", Format = "TEXT 1", StartPOS = 125, EndPOS = 125, Description = "T90 indicator for claims removed from a batch" };
            lsFields.Add(f);
            f = new Field { Name = "KEYING_CLERK_ID", Format = "TEXT 8", StartPOS = 126, EndPOS = 133, Description = "KEYING CLERK ID" };
            lsFields.Add(f);

        }
    }
}