using System;
using System.Collections.Generic;
using System.Text;
using DailyLoan.Model.Entities.DailyLoan;

namespace DailyLoan.Model
{
    public class ManagementUser : User
    {
        public string StatusText { get; set; }
        public string AccessText { get; set; }
        public string HouseText { get; set; }
        public string CustomerLineText { get; set; }
        public int CustomerLineId { get; set; }
    }
    public class ManagementCustomer : Customer
    {
        public string StatusText { get; set; }
        public string HouseText { get; set; }
        public int HouseId { get; set; }
        public string CustomerLineText { get; set; }
    }
    public class ManagementCustomerLine : CustomerLine
    {
        public string StatusText { get; set; }
        public string HouseText { get; set; }
    }
    public class ManagementHouse : House
    {
        public string StatusText { get; set; }
    }
    public class ManagementContract : Contract
    {
        public string StatusText { get; set; }
        //public string HouseText { get; set; }
        //public int HouseId { get; set; }
        //public string CustomerLineText { get; set; }
        //public int CustomerLineId { get; set; }
        //public string Firstname { get; set; }
        //public string Lastname { get; set; }
        //public string ShortAddress { get; set; }
        //public string GuaruntorName { get; set; }
        public string ApproverName { get; set; }
        public ManagementCustomer Customer { get; set; }
        public ManagementCustomer Guaruntor { get; set; }
    }
    public class ManagementConfig
    {
        public Dictionary<string,string> Configs { get; set; }
        public List<SpecialRate> SpecialRates { get; set; }
    }
}
