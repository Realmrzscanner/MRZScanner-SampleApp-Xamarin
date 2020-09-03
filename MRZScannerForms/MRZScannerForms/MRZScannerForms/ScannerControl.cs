using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MRZScannerForms
{
    public class ScannerControl : View
    {
        public ScannerControl() { }

        // Options: [ScannerTypeMrz, ScannerTypeDocImageId, ScannerTypeDocImagePassport]
        public static readonly BindableProperty ScannerTypeProperty = BindableProperty.Create("ScannerType", typeof(ScanType), typeof(ScannerControl), ScanType.Mrz);

        public ScanType ScannerType
        {
            get
            {
                return (ScanType)GetValue(ScannerTypeProperty);
            }
            set
            {
                SetValue(ScannerTypeProperty, value);
            }
        }

        // Enable/disable the ID document type
        public static readonly BindableProperty IDActiveProperty = BindableProperty.Create("IDActive", typeof(bool), typeof(ScannerControl), true);

        public bool IDActive
        {
            get
            {
                return (bool)GetValue(IDActiveProperty);
            }
            set
            {
                SetValue(IDActiveProperty, value);
            }
        }

        // Enable/disable the Passport document type
        public static readonly BindableProperty PassportActiveProperty = BindableProperty.Create("PassportActive", typeof(bool), typeof(ScannerControl), true);

        public bool PassportActive
        {
            get
            {
                return (bool)GetValue(PassportActiveProperty);
            }
            set
            {
                SetValue(PassportActiveProperty, value);
            }
        }

        // Enable/disable the Visa document type
        public static readonly BindableProperty VisaActiveProperty = BindableProperty.Create("VisaActive", typeof(bool), typeof(ScannerControl), true);

        public bool VisaActive
        {
            get
            {
                return (bool)GetValue(VisaActiveProperty);
            }
            set
            {
                SetValue(VisaActiveProperty, value);
            }
        }

        // Set the max CPU threads that the scanner can use
        public static readonly BindableProperty MaxThreadsProperty = BindableProperty.Create("MaxThreads", typeof(int), typeof(ScannerControl), 2);

        public int MaxThreads
        {
            get
            {
                return (int)GetValue(MaxThreadsProperty);
            }
            set
            {
                SetValue(MaxThreadsProperty, value);
            }
        }

        public static readonly BindableProperty LicenseKeyProperty = BindableProperty.Create("LicenseKey", typeof(string), typeof(ScannerControl), "licenseKey");

        public string LicenseKey
        {
            get
            {
                return (string)GetValue(LicenseKeyProperty);
            }
            set
            {
                SetValue(LicenseKeyProperty, value);
            }
        }


        public event EventHandler<ResultModel> ScanningFinished;
        public void OnScanningFinished(ResultModel args)
        {
            ScanningFinished?.Invoke(this, args);
        }

        public event EventHandler OnResumeScanning;
        public void ResumeScanning()
        {
            OnResumeScanning?.Invoke(this, null);
        }

        public event EventHandler ScannerDismissed;
        public void OnScannerDismissed()
        {
            ScannerDismissed?.Invoke(this, null);
        }

        public event EventHandler<LicenseResultType> LicenseResult;
        public void OnLicenseResult(LicenseResultType result)
        {
            LicenseResult?.Invoke(this, result);
        }
    }

    public class ResultModel
    {
        private string estIssuingDateReadable = "";
        private string expirationDateRaw = "";
        private string expirationDateReadable = "";
        private string expirationDateWithCheckDigit = "";
        private IList<string> givenNames;
        private string issuingCountry = "";
        private string masterCheckDigit = "";
        private string nationality = "";
        private IList<string> optionals;
        private string sex = "";
        private IList<string> surnames;
        private string estIssuingDateRaw = "";
        private string documentTypeReadable = "";
        private string documentTypeRaw = "";
        private string documentNumber = "";
        private string documentNumberWithCheckDigit = "";
        private string dobReadable = "";
        private string dobWithCheckDigit = "";
        private string dobRaw = "";
        private long dateScanned;
        private bool isExpired;
        private string fullName = "";
        private string error = null;
        private string rawResult = null;
        byte[] resultImage = null;
        byte[] portrait = null;
        byte[] signature = null;
        byte[] fullImage = null;

        public ResultModel() { }

        public ResultModel(string _documentTypeRaw, string _issuingCountry, string _masterCheckDigit, IList<string> _surnames, IList<string> _givenNames, string _documentNumber, string _documentNumberWithCheckDigit,
            string _nationality, string _dobRaw, string _sex, string _estIssuingDateRaw, string _expirationDateRaw, IList<string> _optionals, long _dateScanned, bool _isExpired,
            string _estIssuingDateReadable, string _expirationDateReadable, string _expirationDateWithCheckDigit, string _documentTypeReadable, string _dobReadable, string _dobWithCheckDigit, string _fullName, string _rawResult)
        {
            this.documentTypeRaw = _documentTypeRaw;
            this.issuingCountry = _issuingCountry;
            this.masterCheckDigit = _masterCheckDigit;
            this.surnames = _surnames;
            this.givenNames = _givenNames;
            this.documentNumber = _documentNumber;
            this.documentNumberWithCheckDigit = _documentNumberWithCheckDigit;
            this.nationality = _nationality;
            this.dobRaw = _dobRaw;
            this.sex = _sex;
            this.estIssuingDateRaw = _estIssuingDateRaw;
            this.expirationDateRaw = _expirationDateRaw;
            this.optionals = _optionals;
            this.dateScanned = _dateScanned;
            this.isExpired = _isExpired;

            this.estIssuingDateReadable = _estIssuingDateReadable;
            this.expirationDateReadable = _expirationDateReadable;
            this.expirationDateWithCheckDigit = _expirationDateWithCheckDigit;
            this.documentTypeReadable = _documentTypeReadable;
            this.dobReadable = _dobReadable;
            this.dobWithCheckDigit = _dobWithCheckDigit;
            this.fullName = _fullName;
            this.rawResult = _rawResult;
        }

        public ResultModel(string _error)
        {
            this.error = _error;
        }

        public string DocumentTypeRaw
        {
            get { return documentTypeRaw; }
            set { documentTypeRaw = value; }
        }

        public string IssuingCountry
        {
            get { return issuingCountry; }
            set { issuingCountry = value; }
        }

        public string MasterCheckDigit
        {
            get { return masterCheckDigit; }
            set { masterCheckDigit = value; }
        }

        public IList<string> Surnames
        {
            get { return surnames; }
            set { surnames = value; }
        }

        public IList<string> GivenNames
        {
            get { return givenNames; }
            set { givenNames = value; }
        }

        public string DocumentNumber
        {
            get { return documentNumber; }
            set { documentNumber = value; }
        }

        public string DocumentNumberWithCheckDigit
        {
            get { return documentNumberWithCheckDigit; }
            set { documentNumberWithCheckDigit = value; }
        }

        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        public string DobRaw
        {
            get { return dobRaw; }
            set { dobRaw = value; }
        }

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public string EstIssuingDateRaw
        {
            get { return estIssuingDateRaw; }
            set { estIssuingDateRaw = value; }
        }

        public string ExpirationDateRaw
        {
            get { return expirationDateRaw; }
            set { expirationDateRaw = value; }
        }

        public IList<string> Optionals
        {
            get { return optionals; }
            set { optionals = value; }
        }

        public long DateScanned
        {
            get { return dateScanned; }
            set { dateScanned = value; }
        }

        public bool IsExpired
        {
            get { return isExpired; }
            set { isExpired = value; }
        }


        public string EstIssuingDateReadable
        {
            get { return estIssuingDateReadable; }
            set { estIssuingDateReadable = value; }
        }

        public string ExpirationDateReadable
        {
            get { return expirationDateReadable; }
            set { expirationDateReadable = value; }
        }

        public string ExpirationDateWithCheckDigit
        {
            get { return expirationDateWithCheckDigit; }
            set { expirationDateWithCheckDigit = value; }
        }

        public string DocumentTypeReadable
        {
            get { return documentTypeReadable; }
            set { documentTypeReadable = value; }
        }

        public string DobReadable
        {
            get { return dobReadable; }
            set { dobReadable = value; }
        }

        public string DobWithCheckDigit
        {
            get { return dobWithCheckDigit; }
            set { dobWithCheckDigit = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        public string RawResult
        {
            get { return rawResult; }
            set { rawResult = value; }
        }

        public byte[] Portrait
        {
            get { return portrait; }
            set { portrait = value; }
        }

        public byte[] Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        public byte[] FullImage
        {
            get { return fullImage; }
            set { fullImage = value; }
        }

        public byte[] ResultImage
        {
            get { return resultImage; }
            set { resultImage = value; }
        }
    }

    public enum ScanType
    {
        Mrz,
        DocImageId,
        DocImagePassport
    }

    public enum LicenseResultType
    {
        Successful,
        Error_While_Parsing,
        Invalid_Licence,
        Invalid_Bundle_ID,
        Invalid_Device_Model,
        Licence_Expired,
        Invalid_Platform,
        Unknown
    }
}
