﻿using VS.Human.Item;
using VS.Human.Rep.Model;

namespace VS.Human.Rep
{
    public interface IEmployeeRep
    {
        Task<Employee> Login(string userName, string password);
        Task<bool> AddOrUpdate(Employee item);
        Task<bool> ChangePassword(string password, int id);

        Task<bool> Delete(int id, bool reactive =false);
        Task<Employee> GetById(int id);
        Task<Employee> CheckDuplicate(string email, string phone);
        Task<BaseList> GetAll(EmployeeRequest id);
        Task<BaseList> GetAllManager(int leadgroup = -1);

    }
}
