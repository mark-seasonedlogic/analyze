using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;


namespace BBIReporting
{
    public class Restaurant
    {
        private string _id; //ex: BFG0507 
        private string _fullName;
        private string _fullAddress;
        private string _phoneNumber;
        private string _emailAddress;
        private string _mp;
        private string _jvp;
        private string _rvp;
        private bool _isOpen;
        private Dictionary<string,AndroidDevice> _devices = new Dictionary<string,AndroidDevice>();
        public string Id { get { return _id; } set { _id = value; } }
        public string FullName { get { return _fullName; } set { _fullName = value; } }
        public string PhoneNumber { get { return _phoneNumber; } set { _phoneNumber = value; } }
        public string EmailAddress { get { return _emailAddress; } set { _emailAddress = value; } }
        public string Mp { get { return _mp; } set { _mp = value; } }
        public string Jvp { get { return _jvp; } set { _jvp = value; } }
        public string Rvp { get { return _rvp; } set { _rvp = value; } }
        public string FullAddress { get { return _fullAddress; } set { _fullAddress = value; } }
        public bool IsOpen { get { return _isOpen; } set { _isOpen  = value; } }
        public Dictionary<string,AndroidDevice> Devices { get {  return _devices; } set { _devices = value; } }
        
        public static string GetIdFromFullName(string fullName)
        {
            string id = String.Empty;
            if (!String.IsNullOrEmpty(fullName) && fullName.Length > 8)
            {
                id = String.Format("{0}{1}", fullName.Substring(5, 3), fullName.Substring(0, 4));
            }
            return id;
        }

        public  enum Fields
{
CONCEPT_RSTRNT_CD,
CORPORATE_ABBR,
CORPORATE_NAME,
LGCY_CONCEPT_CD,
CONCEPT_TYPE,
PRFT_CNTR_CMPNY_CD,
PRFT_CNTR_GRP,
CONCEPT_CD,
RSTRNT_NBR,
RSTRNT_COMMON_NAME,
RSTRNT_LNG_NAME,
RSTRNT_LEGAL_NAME,
FINCL_SYSTEM_CST_CNTR_CD,
SRVC_TYP_CD,
SRVC_TYP_NAME,
OWNERSHIP_TYP_NAME,
JVP_CD,
ROCK_JVP_ID,
PLND_OPN_DT,
PRE_OPN_CLOSED_FLG,
PAY_CYCLE_CD,
LIQR_TAX_PCT,
SEPARATE_LIQR_DPST_FLG,
ADI_CD,
ADI_DESC,
ADI_OVRRD_PCT,
ADI_ADVERTISING_RATE,
ACCTING_FEE_PCT,
SUPERVISORY_FEE_PCT,
INSRNC_FEE_PCT,
ROYALTY_FEE_PCT,
EXCESS_RENT_MGT_FEE_PCT,
MKTNG_FEE_INCM_PCT,
CMPNY_ROYALTY_FEE_PCT,
SRVC_FEE_PCT,
AGENCY_FEE_PCT,
ADVERTISING_FEE_PCT,
RECORD_SLS_AMT,
RECORD_SLS_WK_BEGIN_DT,
CASH_DPST_ACCT_NBR,
LIQR_DPST_ACCT_NBR,
POS_SFTWARE_CD,
BANK_ACCT_NBR,
PAY_GRP_CD,
PAY_GRP_NAME,
BOH_SFTWARE_CD,
AMERICAN_EXPRESS_DPST_ACCT_NBR,
VISA_DPST_ACCT_NBR,
DISCOVER_DPST_ACCT_NBR,
DISCOVER_DISC_OFFSET_ACCT,
DINERS_DPST_ACCT_NBR,
IMPREST_CASH_ACCT_NBR,
OWNERSHIP_ORG_CD,
VACTN_PCT,
SELLS_GFT_CARDS_FLG,
SELLS_MRCHNDS_FLG,
RECORD_TO_GO_SLS_AMT,
RECORD_TO_GO_SLS_WK_BEGIN_DT,
ALT_OWNERSHIP_TYP_NAME,
NATL_PCT,
LCL_TAX_ACCT_NBR,
OTHR_TAX_ACCT_NBR,
LCL_MIN_WAGE_AMT,
LCL_MIN_TIPPED_WAGE_AMT,
USES_CRUNCH_TIME_FLG,
COMP6_UNIT_FOOD_CST_PCT,
FOOD_PRC_TIER_CD,
LIQR_PRC_TIER_CD,
MENU_VRSN_CD,
MENU_VRSN_DESC,
DAYS_OPN_FOR_LNCH_CNT,
OPN_FOR_LNCH_FLG,
DSGNATED_MKT_AREA_CD,
DSGNATED_MKT_AREA_NAME,
LBR_ADJSTMNT_HRS_AMT,
INITAL_ACTV_STATUS_DT,
ACTV_STATUS_FLG,
FINAL_ACTV_STATUS_DT,
OPN_TO_THE_PUBLIC_DT,
CLOSED_TO_THE_PUBLIC_DT,
ACQUISITION_DT,
MANAGING_PARTNER_NAME,
KNAPP_RGN_CD,
KNAPP_RGN_DESC,
ELCTRNC_INVOICE_SYSTEM_CD,
HUMAN_RESRCS_SYSTEM_CD,
DFLT_CRNCY_CD,
INV_SYSTEM_CD,
INV_SYSTEM_NAME,
COMP12_ACCT_NBR,
BONUS_SRC_IND,
AP_INVOICE_SEQ_NBR,
RGN_ID_NBR,
RGN_NAME,
PRESIDENT_CD,
PRESIDENT_NAME,
RVP_CD,
ROCK_RVP_ID,
RVP_CMPNY_CD,
RVP_LONG_NAME,
JVP_CMPNY_CD,
JVP_GRP_ABBR,
JVP_GRP_DESC,
JVP_AREA_GL_CASH_ACCT_NBR_CD,
JVP_UTM_NET_CASH_GL_ACCT_NBR_C,
JVP_SPCL_EVNT_CASH_GL_ACCT_NBR,
JVP_AP_POSTING_CMPNY_CD,
JVP_AP_DEBIT_POSTING_CMPNY_CD,
ROCK_UNIT_ID_KEY,
UPDT_DT,
LIQR_PRC_TIER_NAME,
FOOD_PRC_TIER_NAME,
HUMAN_RESRCS_SYSTEM_NAME,
BOH_SFTWARE_NAME,
PAY_CYCLE_NAME,
JVP_POSITION_NAME,
JVP_COMMENT_TXT,
JVP_CNTCT_NAME,
POS_LVL_LOAD_DT,
LCL_TAX_PCT,
FEDERAL_MIN_WAGE_AMT,
FEDERAL_TIPPED_MIN_WAGE_AMT,
MIN_WAGE_AMT,
LCL_MIN_WAGE_ALWD_FLG,
ACCTS_PAYABLE_DEBIT_POSTING_CD,
OWNERSHIP_TYP_CD,
INVCE_PSTNG_CMPNY_CD,
JVP_INDVL_ID_NBR,
RVP_INDVL_ID_NBR,
MANAGING_PARTNER_INDVL_ID_NBR,
COLLCTN_TIME_MONDAY,
COLLCTN_TIME_TUESDAY,
COLLCTN_TIME_WEDNESDAY,
COLLCTN_TIME_THURSDAY,
COLLCTN_TIME_FRIDAY,
COLLCTN_TIME_SATURDAY,
COLLCTN_TIME_SUNDAY,
POS_LVL_TABLE_CODE,
LAST_REMDL_DT,
REMDLED_FLG,
LAST_RELOCATED_DT,
RELOCATED_FLG,
OPN_MON_THRU_THURS_LNCH_FLG,
MON_THRU_THURS_LNCH_STRT_DT,
MON_THRU_THURS_LNCH_END_DT,
OPN_FRIDAY_LNCH_FLG,
FRIDAY_LNCH_STRT_DT,
FRIDAY_LNCH_END_DT,
OPN_SATURDAY_LNCH_FLG,
SATURDAY_LNCH_STRT_DT,
SATURDAY_LNCH_END_DT,
OPN_SUNDAY_BRUNCH_FLG,
SUNDAY_BRUNCH_STRT_DT,
SUNDAY_BRUNCH_END_DT,
IMPREST_PYMT_CLRG_ACCT_NBR_CD,
FINTECH_PYMT_CLRG_ACCT_NBR_CD,
IMPREST_PYMT_CMPNY_CD,
FINTECH_PYMT_CMPNY_CD,
TIPS_CLRNG_ACCT,
WINE_CNTRY_FLG,
SUNDAY_STRT_LNCH_DT,
SUNDAY_END_LNCH_DT,
DSTRCT_NBR,
SPCL_EVNT_INTRNL_ORDR_NBR_CD,
SPCL_EVNT_CUST_NBR_CD,
COMP1_INTRNL_ORDR_NBR_CD,
JVP_EMPLOYEE_ID,
JVP_INDVL_NAME,
JVP_EMAIL_ADDR,
RVP_EMPLOYEE_ID,
RVP_INDVL_NAME,
RVP_EMAIL_ADDR,
TIME_ZN_ABBR,
TIME_ZN_NAME,
CONCEPT_RSTRNT_NBR,
INCLUSIVE_TAX_FLG,
MP_POSITION_CD,
OPN_SUNDAY_LNCH_FLG,
POS_LVL_NAME,
ST_MIN_TIPPED_WAGE_AMT,
ST_MIN_WAGE_AMT,
ST_TIPPED_WAGE_OVRRD_AMT,
CST_CNTR_CTGRY_CD,
CST_CNTR_CTGRY,
JVP_PRFT_CNTR_CD,
RVP_PRFT_CNTR_CD,
ADDR_LINE1_TXT,
ADDR_LINE2_TXT,
CITY_NAME,
COUNTY_NAME,
STATE_CD,
STATE_NAME,
POSTAL_CODE,
COUNTRY,
COUNTRY_NAME,
STORE_PHONE_NO,
STORE_EMAIL_ADDR,
TELEPHONE_CNTRY_CD,
TELEPHONE_AREA_CD,
TELEPHONE_EXCHNG_CD,
TELEPHONE_LINE_CD,
PERSON_RESPONSIBLE,
PCTR_START_DATE,
CIM_DATE,
RIA_DATE,
RIA_FLAG,
EIPP_FLAG,
CIM_FLAG,
POSI_FLAG,
RMS_FLAG,
IS_VALIDATED,
LAT_NBR,
LONG_NBR,
MANUAL_INVOICE_SYSTEM_CD,
LGCY_RVP_ID,
PAY_GROUP_TABLE_CODE,
CAPEX_INTRNL_ORDR_NBR_CD,
IS_DELETED,
TIME_ZN_RGN_NAME,
OPN_FOR_LNCH,
OPN_FOR_LNCH_DAYS_CNT,
SUN_OPN_TIME,
MON_OPN_TIME,
TUE_OPN_TIME,
WED_OPN_TIME,
THU_OPN_TIME,
FRI_OPN_TIME,
SAT_OPN_TIME,
SUN_CLOSE_TIME,
MON_CLOSE_TIME,
TUE_CLOSE_TIME,
WED_CLOSE_TIME,
THU_CLOSE_TIME,
FRI_CLOSE_TIME,
SAT_CLOSE_TIME,
DIRECTIONS_TXT,
DIRECTIONS2_TXT,
MARKTNG_MGR_NAME,
RVP_ADMIN_EMAIL,
OPN_FOR_WKEND_LUNCH_FLG,
FAX_NUMBER,
FACEBOOK_URL_TXT,
TAKEAWAY_VENDOR_TXT,
TAKEAWAY_VENDOR_URL_TXT,
IS_TMP_CLOSED_FLG,
GREETING_TXT,
GREETING_IMAGE_URL_TXT,
HAS_CALLAHEAD_FLG,
HAS_CATERING_FLG,
HAS_DELIVERY_FLG,
HAS_DELIVERY_WINE_FLG,
HAS_DINE_REWARDS_FLG,
HAS_GLUTEN_FREE_FLG,
HAS_LARGE_PARTY_FLG,
HAS_PRIVATE_DINING_FLG,
HAS_RESERVATIONS_FLG,
HAS_TAKEAWAY_FLG,
HAS_TAKEAWAY_WINE_FLG,
HAS_WIFI_FLG,
IS_SHOWN_ON_WEBSITE_FLG,
DISPLAY_NAME,
POS_IP_ADDRESS_TXT,
PFG_DC_NBR,
HAS_DINNER_DELIVERY_FLG,
HAS_LUNCH_DELIVERY_FLG,
DLVY_START_DT,
DLVY_END_DT,
TBL_MGMT_SYS_CD,
PVT_DNG_EMAIL_ADDR_TEXT,
CP_NAME,
CP_EMAIL_ADDR_TEXT,
BD_NAME,
BD_EMAIL_ADDR_TEXT
}        public Restaurant()
        {

        }
        public Restaurant(string id, string fullName, string fullAddress, string phoneNumber, string emailAddress, string mp, string jvp, string rvp, bool isOpen=true)
        {
            _id = id;
            _fullName = fullName;
            _fullAddress = fullAddress;
            _phoneNumber = phoneNumber;
            _emailAddress = emailAddress;
            _mp = mp;
            _jvp = jvp;
            _rvp = rvp;
            _isOpen = isOpen;
        }
    public static Dictionary<string, Restaurant> GetMasterRestaurantList(string filename, ILogger<Restaurant> log)
{
    Dictionary<string, Restaurant> masterRestaurants = new Dictionary<string, Restaurant>();
    Encoding encoding = Encoding.UTF8;

    // Check if the file exists
    if (!File.Exists(filename))
    {
        log.LogError($"Master restaurant directory \"{filename}\" does not exist.");
        return masterRestaurants;
    }

    // Read and process the CSV file
    using (var reader = new StreamReader(filename, encoding))
    {
        int lineNum = 0;
        string[] headers = { "" };
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            // Split the line using commas (CSV format), handling quoted fields
            string[] fields = SplitCsvLine(line);
            if (lineNum == 0)
            {
                headers = fields;
                lineNum++;
                continue;
            }
            lineNum++;

            if (fields.Length < headers.Length)
            {
                log.LogError("More fields found in line than expected ({0}).\nExpected Number of Fields: {1}", fields.Length, headers.Length);
                continue;
            }

            // Log debug information
            for (int fieldNum = 0; fieldNum < fields.Length; fieldNum++)
            {
                if (fieldNum >= headers.Length)
                {
                    log.LogError("Unexpected number of fields.\nField Number: {0}\nExpected Number of Fields: {1}", fieldNum, headers.Length);
                }
                else
                {
                    log.LogDebug("Header: {0}\nValue: {1}", headers[fieldNum], fields[fieldNum]);
                }
            }

            if (String.IsNullOrEmpty(fields[(int)Restaurant.Fields.RSTRNT_LNG_NAME]))
            {
                log.LogDebug("Restaurant master missing name!");
                continue;
            }

            DateTime closedDate = DateTime.Today;
            string currRestClosedDate = fields[(int)Restaurant.Fields.CLOSED_TO_THE_PUBLIC_DT];
            string dateFormat = "M/d/yyyy h:mm:ss tt";
            CultureInfo culture = CultureInfo.InvariantCulture;

            try
            {
                if (!String.IsNullOrEmpty(currRestClosedDate))
                {
                    closedDate = DateTime.ParseExact(currRestClosedDate, dateFormat, culture);
                }
                else
                {
                    log.LogError("Closed date for {0} is invalid: {1}", fields[(int)Restaurant.Fields.RSTRNT_LNG_NAME], currRestClosedDate);
                    closedDate = DateTime.Parse("01/01/9999");
                }
            }
            catch (Exception dtEx)
            {
                log.LogError("Exception {0}\nDate String {1}", dtEx.Message, currRestClosedDate);
            }

            if (closedDate < DateTime.Today)
            {
                log.LogDebug("Found {0} with Closed Date {1}", fields[(int)Restaurant.Fields.RSTRNT_LEGAL_NAME], currRestClosedDate);
            }

            try
            {
                Restaurant currRestaurant = new Restaurant
                {
                    Id = String.Format("{0}{1}", fields[(int)Restaurant.Fields.RSTRNT_LEGAL_NAME].Substring(5, 3), fields[(int)Restaurant.Fields.RSTRNT_LEGAL_NAME].Substring(0, 4)),
                    Mp = fields[(int)Restaurant.Fields.MANAGING_PARTNER_NAME],
                    Jvp = fields[(int)Restaurant.Fields.JVP_CNTCT_NAME],
                    FullName = fields[(int)Restaurant.Fields.RSTRNT_LNG_NAME],
                    Rvp = fields[(int)Restaurant.Fields.RVP_INDVL_NAME],
                    FullAddress = String.Format("{0},{1},{2} {3}",
                        fields[(int)Restaurant.Fields.ADDR_LINE1_TXT],
                        fields[(int)Restaurant.Fields.CITY_NAME],
                        fields[(int)Restaurant.Fields.STATE_NAME],
                        fields[(int)Restaurant.Fields.POSTAL_CODE]),
                    IsOpen = closedDate >= DateTime.Today
                };

                log.LogDebug("Including restaurant {1} with closed Date for {0}", currRestaurant.FullName, closedDate);
                masterRestaurants.Add(currRestaurant.Id, currRestaurant);
            }
            catch (Exception ex)
            {
                log.LogError("Exception processing restaurant: {0}\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }

    return masterRestaurants;
}

/// <summary>
/// A helper function to split CSV lines, handling quoted fields.
/// </summary>
/// <param name="line">The CSV line to split.</param>
/// <returns>An array of fields in the CSV line.</returns>
private static string[] SplitCsvLine(string line)
{
    var result = new List<string>();
    var currentField = new StringBuilder();
    bool inQuotes = false;

    for (int i = 0; i < line.Length; i++)
    {
        char currentChar = line[i];

        if (currentChar == '"')
        {
            // Toggle quoted state
            if (inQuotes && i + 1 < line.Length && line[i + 1] == '"') // Escaped quote
            {
                currentField.Append('"');
                i++;
            }
            else
            {
                inQuotes = !inQuotes;
            }
        }
        else if (currentChar == ',' && !inQuotes)
        {
            // End of field
            result.Add(currentField.ToString());
            currentField.Clear();
        }
        else
        {
            // Add normal character to the field
            currentField.Append(currentChar);
        }
    }

    // Add the last field
    result.Add(currentField.ToString());

    return result.ToArray();
}

    }
   }
