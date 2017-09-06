using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FESSite.Models
{
    public class ClaimDetail
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

        public ClaimDetail()
        {
            lsFields = new List<Field>();
            Field f = new Field();
            f = new Field { Name = "RECORD_ID", Format = "TEXT 2", StartPOS = 1, EndPOS = 2, Description = "VALUE \"ID\"" };
            lsFields.Add(f);
            f = new Field { Name = "SEQUECNE_NUMBER", Format = "TEXT 2", StartPOS = 3, EndPOS = 4, Description = "Service line Sequence Number" };
            lsFields.Add(f);
            f = new Field { Name = "FROM_DATE_OF_SERVICE", Format = "TEXT 8", StartPOS = 5, EndPOS = 12, Description = "FROM DATE OF SERVICE" };
            lsFields.Add(f);
            f = new Field { Name = "THROUGH_DATE_OF_SERVICE", Format = "TEXT 8", StartPOS = 13, EndPOS = 20, Description = "THROUGH DATE OF SERVICE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_PROCEDURE_CODE", Format = "TEXT 7", StartPOS = 21, EndPOS = 27, Description = "PROCEDURE CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_MODIFIER_CODE_1", Format = "TEXT 2", StartPOS = 28, EndPOS = 29, Description = "MODIFIER CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_MODIFIER_CODE_2", Format = "TEXT 2", StartPOS = 30, EndPOS = 31, Description = "MODIFIER CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_MODIFIER_CODE_3", Format = "TEXT 2", StartPOS = 32, EndPOS = 33, Description = "MODIFIER CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_MODIFIER_CODE_4", Format = "TEXT 2", StartPOS = 34, EndPOS = 35, Description = "MODIFIER CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_MODIFIER_CODE_5", Format = "TEXT 2", StartPOS = 36, EndPOS = 37, Description = "MODIFIER CODE" };
            lsFields.Add(f);
            f = new Field { Name = "FK_PLACE_OF_SERVICE", Format = "TEXT 2", StartPOS = 38, EndPOS = 39, Description = "PLACE OF SERVICE " };
            lsFields.Add(f);
            f = new Field { Name = "FK_TYPE_OF_SERVICE", Format = "TEXT 2", StartPOS = 40, EndPOS = 41, Description = "TYPE OF SERVICE" };
            lsFields.Add(f);
            f = new Field { Name = "DIAGNOSIS_POINTER", Format = "TEXT 2", StartPOS = 42, EndPOS = 43, Description = "DIAGNOSIS POINTER" };
            lsFields.Add(f);
            f = new Field { Name = "MEDICAID_CIDC_EOB_CODE", Format = "TEXT 5", StartPOS = 44, EndPOS = 48, Description = "EOB Medicaid used to disposition the claim detail" };
            lsFields.Add(f);
            f = new Field { Name = "DENT_TOOTH_NUMBER", Format = "TEXT 2", StartPOS = 49, EndPOS = 50, Description = "DENTAL TOOTH NUMBER" };
            lsFields.Add(f);
            f = new Field { Name = "DENT_TOOTH_SURFACE_CODE", Format = "TEXT 5", StartPOS = 51, EndPOS = 55, Description = "DENTAL TOOTH SURFACE CODE" };
            lsFields.Add(f);
            f = new Field { Name = "ENC_HMO_EOB_CODE", Format = "TEXT 5", StartPOS = 56, EndPOS = 60, Description = "ENCOUNTER HMO EOB CODE" };
            lsFields.Add(f);
            f = new Field { Name = "ENC_HMO_CLAIM_STATUS", Format = "TEXT 1", StartPOS = 61, EndPOS = 61, Description = "ENCOUNTER HMO CLAIM STATUS " };
            lsFields.Add(f);
            f = new Field { Name = "MC_MEDICARE_PAYMENT_IND", Format = "TEXT 1", StartPOS = 62, EndPOS = 62, Description = "MEDICARE PAYMENT INDICATOR" };
            lsFields.Add(f);
            f = new Field { Name = "PPD_ALTERNATE_ID", Format = "TEXT 13", StartPOS = 63, EndPOS = 75, Description = "PRERFORMING PROVIDER ALTERNATE ID" };
            lsFields.Add(f);
            f = new Field { Name = "SEQUENCE_NUMBER_HIPAA", Format = "TEXT 3", StartPOS = 76, EndPOS = 78, Description = "Service line Sequence Number" };
            lsFields.Add(f);
            f = new Field { Name = "ANESTHESIA_MINUTES_HIPAA", Format = "TEXT 15", StartPOS = 79, EndPOS = 93, Description = "ANESTHESIA MINUTES" };
            lsFields.Add(f);
            f = new Field { Name = "BILLED_QUANTITY_HIPAA", Format = "TEXT 15", StartPOS = 94, EndPOS = 108, Description = "BILLED QUANTITY" };
            lsFields.Add(f);
            f = new Field { Name = "BILLED_AMT_HIPAA", Format = "TEXT 12", StartPOS = 109, EndPOS = 120, Description = "BILLED AMOUNT" };
            lsFields.Add(f);
            f = new Field { Name = "MC_PAID_AMT_HIPAA", Format = "TEXT 12", StartPOS = 121, EndPOS = 132, Description = "MEDICARE PAID AMOUNT" };
            lsFields.Add(f);
            f = new Field { Name = "MC_COINS_AMT", Format = "TEXT 12", StartPOS = 133, EndPOS = 144, Description = "MEDICARE COINSURANCE AMOUNT" };
            lsFields.Add(f);
            f = new Field { Name = "MC_MEDICARE_ALLOWED_AMT_HIPAA", Format = "TEXT 12", StartPOS = 145, EndPOS = 156, Description = "MEDICARE ALLOWED AMOUNT" };
            lsFields.Add(f);
            f = new Field { Name = "UB92_UNIT_PRICE_HIPAA", Format = "TEXT 12", StartPOS = 157, EndPOS = 168, Description = "UB92 UNIT PRICE" };
            lsFields.Add(f);
            f = new Field { Name = "MC_REGULAR_DEDUCTIBLE_HIPAA", Format = "TEXT 12", StartPOS = 169, EndPOS = 180, Description = "MEDICARE REGULAR DEDUCTIBLE" };
            lsFields.Add(f);
            f = new Field { Name = "MC_BLOOD_DEDUCTIBLE_HIPAA", Format = "TEXT 12", StartPOS = 181, EndPOS = 192, Description = "MEDICARE BLOOD DEDUCTABLE" };
            lsFields.Add(f);
            f = new Field { Name = "UB92_CHARGES_NOT_COVERED_HIPAA", Format = "TEXT 12", StartPOS = 193, EndPOS = 204, Description = "UB92 CHARGES NOT COVERED" };
            lsFields.Add(f);
            f = new Field { Name = "STD_DTL_LINE_ITEM_CONTROL_NUM_HIPAA", Format = "TEXT 30", StartPOS = 205, EndPOS = 234, Description = "DETAIL LINE ITEM CONTROL NUMBER" };
            lsFields.Add(f);
            f = new Field { Name = "STD_FK_DTL_CONDITION_CD_1_HIPAA", Format = "TEXT 2", StartPOS = 235, EndPOS = 236, Description = "DETAIL CONDITION CODE" };
            lsFields.Add(f);
            f = new Field { Name = "STD_FK_DTL_CONDITION_CD_2_HIPAA", Format = "TEXT 2", StartPOS = 237, EndPOS = 238, Description = "DETAIL CONDITION CODE" };
            lsFields.Add(f);
            f = new Field { Name = "STD_FK_DTL_CONDITION_CD_3_HIPAA", Format = "TEXT 2", StartPOS = 239, EndPOS = 240, Description = "DETAIL CONDITION CODE" };
            lsFields.Add(f);
            f = new Field { Name = "PPD_PERF_PROV_TAXONOMY_CD_HIPAA", Format = "TEXT 10", StartPOS = 241, EndPOS = 250, Description = "PERFORMING PROVIDER TAXONOMY CODE" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_PERF_NPI_API_PROV_ID", Format = "TEXT 10", StartPOS = 251, EndPOS = 260, Description = "PERFORMING PROVIDER NPI/API/PROVIDER ID" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_PERF_PROV_XWALK_ADDR_1", Format = "TEXT 30", StartPOS = 261, EndPOS = 290, Description = "PERFORMING PROVIDER ADDRESS" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_PERF_PROV_XWALK_CITY", Format = "TEXT 20", StartPOS = 291, EndPOS = 310, Description = "PERFORMING PROVIDER CITY" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_PERF_PROV_XWALK_STATE", Format = "TEXT 2", StartPOS = 311, EndPOS = 312, Description = "PERFORMING PROVIDER STATE" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_PERF_PROV_XWALK_ZIPCODE", Format = "TEXT 10", StartPOS = 313, EndPOS = 322, Description = "PERFORMING PROVIDER ZIPCODE" };
            lsFields.Add(f);
            f = new Field { Name = "STD_UFK_NDC_CD", Format = "TEXT 11", StartPOS = 323, EndPOS = 333, Description = "NDC CODE" };
            lsFields.Add(f);
            f = new Field { Name = "STD_UFK_NDC_CD_MOD", Format = "TEXT 2", StartPOS = 334, EndPOS = 335, Description = "NDC CODE MODIFIER" };
            lsFields.Add(f);
            f = new Field { Name = "STD_NDC_QUANTITY", Format = "TEXT 12", StartPOS = 336, EndPOS = 347, Description = "NDC QUANTITY" };
            lsFields.Add(f);
            f = new Field { Name = "STD_UFK_PROC_REV_CD", Format = "TEXT 7", StartPOS = 348, EndPOS = 354, Description = "Revenue code used at the service line" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_AMB_PICKUP_ZIP", Format = "TEXT 5", StartPOS = 355, EndPOS = 359, Description = "Ambulance pickup zip" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_AMB_PICKUP_ZIP_EXT", Format = "TEXT 4", StartPOS = 360, EndPOS = 363, Description = "Ambulance pickup zip extension" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_AMB_DROPOFF_ZIP", Format = "TEXT 5", StartPOS = 364, EndPOS = 368, Description = "Ambulance pickup drop off zip " };
            lsFields.Add(f);
            f = new Field { Name = "ICD_AMB_DROPOFF_ZIP_EXT", Format = "TEXT 4", StartPOS = 369, EndPOS = 372, Description = "Ambulance pickup drop off zip extension" };
            lsFields.Add(f);
            f = new Field { Name = "ICD_OBS_ANESTHESIA_ADD_UNITS", Format = "TEXT 3", StartPOS = 373, EndPOS = 375, Description = "Anesthesia additional units" };
            lsFields.Add(f);
        }
    }
}