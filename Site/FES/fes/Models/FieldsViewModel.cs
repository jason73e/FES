﻿using PagedList;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace fes.Models
{
    public class FieldsViewModel
    {
        public IPagedList<Field> lsFields { get; set; }

        public ClaimLayoutType? SearchClaimLayout { get; set; }

        public RecordType? SearchRecordType { get; set; }

        public FormGrouping? SearchFormGrouping { get; set; }

        public SelectList slFileVersions { get; set; }

        public string SearchFileVersion { get; set; }

        public string SearchName { get; set; }
    }
}