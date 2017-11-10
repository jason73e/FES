using System.Collections.Generic;
using System.Linq;

namespace fes.Models
{
    public static class Utility
    {

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


    }

}