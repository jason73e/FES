using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FESSite.Models
{
    public enum ClaimLayoutType
    {
        All=1,UB04=2,Dental=3,HCFA=4, UB04Crossover=5, UB04Advantage=6, HCFAAdvantage=7,FamilyPlanning=8, HCFACrossover = 9
    }

    public enum RecordType
    {
        FileHeader=1,ClaimHeader=2,PaperHeader=3,ClaimDetail=4,PaperDetail=5
    }
    public class Field
    {
        private string m_Value;

        [Key]
        public int FieldID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Format { get; set; }
        [Required]
        public int StartPOS { get; set; }
        [Required]
        public int EndPOS { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string DisplayName { get; set; }
        public string PseudoCode { get; set; }
        public string FormFieldName { get; set; }
        public string FormFieldPosition { get; set; }
        public string WebDEFieldName { get; set; }
        public string Comments { get; set; }
        [Required]
        public string FileVersion { get; set; }
        [Required]
        public RecordType RecordType { get; set; }
        [Required]
        public ClaimLayoutType ClaimType { get; set; }
        [Required]
        public DateTime ts { get; set; }
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