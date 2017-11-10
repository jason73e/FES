using fes.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fes.Models
{
    public static class Utility
    {
        public static SelectList GetFileVersions()
        {
            FESContext db = new FESContext();
            List<string> lsFileVersions = db.Fields.Select(x => x.FileVersion).Distinct().ToList();
            SelectList sl = new SelectList(lsFileVersions);
            db.Dispose();
            return (sl);
        }

        public static ClaimLayoutType GetCLT(string sClaimTypeCode)
        {
            switch (sClaimTypeCode)
            {
                case "001":
                    return ClaimLayoutType.UB04;
                case "002":
                    return ClaimLayoutType.UB04;
                case "003":
                    return ClaimLayoutType.Dental;
                case "004":
                    return ClaimLayoutType.HCFA;
                case "005":
                    return ClaimLayoutType.UB04;
                case "006":
                    return ClaimLayoutType.UB04;
                case "007":
                    return ClaimLayoutType.HCFA;
                case "008":
                    return ClaimLayoutType.FamilyPlanning;
                default:
                    return ClaimLayoutType.All;
            }

        }
        public static string GetClaimType(string sClaimTypeCode)
        {
            string ClaimType = string.Empty;
            switch (sClaimTypeCode)
            {
                case "001":
                    ClaimType = "UB04 Inpatient";
                    break;

                case "002":
                    ClaimType = "UB04 Outpatient";
                    break;
                case "003":
                    ClaimType = "Dental";
                    break;
                case "004":
                    ClaimType = "HCFA";
                    break;
                case "005":
                    ClaimType = "UB04 Outpatient Crossover/Advantage";
                    break;
                case "006":
                    ClaimType = "UB04 Inpatient Crossover/Advantage";
                    break;
                case "007":
                    ClaimType = "HCFA Crossover/Advantage";
                    break;
                case "008":
                    ClaimType = "Family Planning";
                    break;
                default:
                    ClaimType = "UNKNOWN";
                    break;
            }
            return ClaimType;
        }

        public static void SendProgress(string progressMessage, int progressCount, int totalItems)
        {
            //IN ORDER TO INVOKE SIGNALR FUNCTIONALITY DIRECTLY FROM SERVER SIDE WE MUST USE THIS
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();

            //CALCULATING PERCENTAGE BASED ON THE PARAMETERS SENT
            var percentage = (progressCount * 100) / totalItems;

            //PUSHING DATA TO ALL CLIENTS
            hubContext.Clients.All.AddProgress(progressMessage, percentage + "%");
        }

    }

}