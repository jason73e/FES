using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESSite.Models
{
    public class PaperDetail
    {
        public List<Field> lsFields;
        private string m_Source;
        public string Source { get { return m_Source; } set { m_Source = value; } }

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
        public PaperDetail()
        {
            lsFields = new List<Field>();
            Field f = new Field();
            f = new Field { Name = "RECORD_ID", Format = "TEXT 2", StartPOS = 1, EndPOS = 2, Description = "value \"PD\"" };
            lsFields.Add(f);
            f = new Field { Name = "FK_DATA_ENTRY_EOB_EOPS_CODE", Format = "TEXT 5", StartPOS = 3, EndPOS = 7, Description = "EOB entered by data entry clerk" };
            lsFields.Add(f);
        }
    }
}