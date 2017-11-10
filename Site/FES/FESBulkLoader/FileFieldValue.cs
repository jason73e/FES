using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fes.Models
{
    public class FileFieldValue
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int FileID { get; set; }

        [Required]
        public int FieldID { get; set; }
        [Required]
        public string value { get; set; }

        [Required]
        public DateTime ts { get; set; }

        [Required]
        public RecordType recordType { get; set; }

        [Required]
        public int recordSequence { get; set; }


    }
}