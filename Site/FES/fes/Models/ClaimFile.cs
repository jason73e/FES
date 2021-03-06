﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace fes.Models
{
    public class ClaimFile
    {
        [Key]
        public int FileID { get; set; }
        [Required]
        public string Filename { get; set; }
        [Required]
        public string FileSize { get; set; }
        [Required]
        public bool Parsed { get; set; }

        public string DocType { get; set; }

        public DateTime ts { get; set; }

        public FileHeader fh;
        public ClaimFile()
        {
        }

        public FileHeader Parse(string sSource)
        {
            fh = new FileHeader(sSource);
            fh.ParseSource();
            return fh;
        }
    }
}