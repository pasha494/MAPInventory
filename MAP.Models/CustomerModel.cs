using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Inventory.Model
{

    public class CustomerModel
    {
        private int _CustomerId;
        public int CustomerId
        {
            get { return _CustomerId; }
            set { _CustomerId = value; }
        }

        private string _FirstName = string.Empty;
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName = string.Empty; 
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private bool _IsCustomer; 
        public bool IsCustomer
        {
            get { return _IsCustomer; }
            set { _IsCustomer = value; }
        }
         
        private string _EmailId = string.Empty; 
        public string EmailId
        {
            get { return _EmailId; }
            set { _EmailId = value; }
        }

        private int _MobileNo; 
        public int MobileNo
        {
            get { return _MobileNo; }
            set { _MobileNo = value; }
        }
         
        private string _PhoneNo = string.Empty; 
        public string PhoneNo
        {
            get { return _PhoneNo; }
            set { _PhoneNo = value; }
        }

        private string _PAN = string.Empty; 
        public string PAN
        {
            get { return _PAN; }
            set { _PAN = value; }
        }

        private string _GSTIN = string.Empty;
        public string GSTIN
        {
            get { return _GSTIN; }
            set { _GSTIN = value; }
        }

        private string _AdharNumber = string.Empty;
        public string AdharNumber
        {
            get { return _AdharNumber; }
            set { _AdharNumber = value; }
        }

        private string _Address = string.Empty;
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        } 

        private string _City = string.Empty;
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private int _PinCode; 
        public int PinCode
        {
            get { return _PinCode; }
            set { _PinCode = value; }
        } 

        private string _Notes = string.Empty;
        public string Notes
        {
            get { return _Notes; }
            set { _Notes = value; }
        }
         
        private string _Extra1 = string.Empty;
        public string Extra1
        {
            get { return _Extra1; }
            set { _Extra1 = value; }
        }

        private string _Extra2 = string.Empty;
        public string Extra2
        {
            get { return _Extra2; }
            set { _Extra2 = value; }
        }

        private string _Extra3 = string.Empty;
        public string Extra3
        {
            get { return _Extra3; }
            set { _Extra3 = value; }
        }

        private string _Extra4 = string.Empty;
        public string Extra4
        {
            get { return _Extra4; }
            set { _Extra4 = value; }
        }


        private string _Extra5 = string.Empty;
        public string Extra5
        {
            get { return _Extra5; }
            set { _Extra5 = value; }
        }
           
    }
}
