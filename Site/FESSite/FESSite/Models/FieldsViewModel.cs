using PagedList;
using System.Web.Mvc;

namespace FESSite.Models
{
    public class FieldsViewModel
    {
        public IPagedList<Field> lsFields { get; set; }

        public ClaimLayoutType? SearchClaimLayout { get; set; }

        public RecordType? SearchRecordType { get; set; }

        public FormGrouping? SearchFormGrouping { get; set; }

        public SelectList slFileVersions { get; set; }

        public string SearchFileVersion { get; set; }

    }
}