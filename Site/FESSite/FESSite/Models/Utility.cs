using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FESSite.Models
{
    public static class Utility
    {
        public static SelectList GetFileVersions()
        {
            FESSiteContext db = new FESSiteContext();
            List<string> lsFileVersions = db.Fields.Select(x => x.FileVersion).Distinct().ToList();
            SelectList sl = new SelectList(lsFileVersions);
            db.Dispose();
            return (sl);
        }
    }
}