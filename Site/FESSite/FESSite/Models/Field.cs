using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESSite.Models
{
    public class Field
    {
        private string m_Name;
        private string m_Format;
        private int m_StartPOS;
        private int m_EndPOS;
        private string m_Description;
        private string m_PseudoCode;
        private string m_FormFieldName;
        private string m_FormFieldPosition;
        private string m_WebDEFieldName;
        private string m_Comments;
        private string m_Value;

        public string Name { get { return m_Name; } set { m_Name = value; } }
        public string Format { get { return m_Format; } set { m_Format = value; } }
        public int StartPOS { get { return m_StartPOS; } set { m_StartPOS = value; } }
        public int EndPOS { get { return m_EndPOS; } set { m_EndPOS = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public string PseudoCode { get { return m_PseudoCode; } set { m_PseudoCode = value; } }
        public string FormFieldName { get { return m_FormFieldName; } set { m_FormFieldName = value; } }
        public string FormFieldPosition { get { return m_FormFieldPosition; } set { m_FormFieldPosition = value; } }
        public string WebDEFieldName { get { return m_WebDEFieldName; } set { m_WebDEFieldName = value; } }
        public string Comments { get { return m_Comments; } set { m_Comments = value; } }
        public string Value { get { return m_Value; } set { m_Value = value; } }

        public Field()
        {

        }

        public Field(string sName, string sFormat, int istart, int iEnd)
        {
            this.Name = sName;
            this.Format = sFormat;
            this.StartPOS = istart;
            this.EndPOS = iEnd;
        }
    }
}