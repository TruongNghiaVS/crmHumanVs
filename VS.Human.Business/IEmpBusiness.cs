﻿using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace VS.Human.Business
{
    public interface IEmpBusiness
    {
        Task<Employee> Login(string userName, string password);
        Task<Employee> GetById(int Id);
        Task<Employee> CheckDuplicate(string email, string phone);
        Task<bool> Add(EmployeeAdd item);
        Task<bool> Update(EmployeeAdd item);
        Task<bool> ChangePassword(string password, int id);
        Task<bool> Delete(int id, bool reactive =false );


        Task<BaseList> GetAll(EmployeeRequest request);
    }



    public interface IBaseBusiness
    {

        Task<bool> Delete(int id);
    }
}