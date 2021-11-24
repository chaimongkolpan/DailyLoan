using DailyLoan.Model.Entities.DailyLoan;
using DailyLoan.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyLoan.Repository.Interfaces
{
    public interface IPayRepo
    {
        #region getvalue
        int GetHouseIdByUserId(int uid);
        int GetCustomerLineIdByUserId(int uid);
        string GetIdcardByCustomerId(int cid);
        ManagementCustomer SearchCustomer(string idcard, string name);
        bool isExistContract(int cid);
        #endregion

        #region Customer
        Task<List<ManagementCustomer>> GetAllCustomer(int uid, int useraccess);
        ManagementCustomer GetCustomer(int cid);
        Task<bool> EditCustomer(Customer req);
        Task<bool> DeleteCustomer(int cid);
        #endregion

        #region Contract
        Task<List<ManagementContract>> GetAllContract(int uid, int useraccess);
        ManagementContract GetContract(int cid);
        Task<bool> EditContract(Contract req);
        Task<bool> DeleteContract(int cid);
        #endregion
    }
}
