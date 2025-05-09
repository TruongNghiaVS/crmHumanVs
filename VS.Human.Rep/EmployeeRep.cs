﻿using Dapper;
using Microsoft.Extensions.Configuration;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace VS.Human.Rep
{
    public class EmployeeRep : RepositoryBase<Employee>, IEmployeeRep
    {

        public EmployeeRep(IConfiguration configuration)
            : base(configuration)
        {

            tableName = "Employees";
        }
        public async Task<Employee> Login(string userName, string password)
        {
            var modelCheck = new
            {
                userName,
                password
            };
            var result = await ExecuteSQL<Employee>("sp_emp_login",
              modelCheck);
            return result;
        }

        private async Task<bool> Update(Employee item)
        {
            var parameter = new
            {
                item.Id,
                item.FullName,
                item.NationalDate,
                item.NationalId,
                item.NationalPlace,
                item.Dob,
                item.Onboard,
                item.Phone,
                item.PositionCode,
                item.RoleCode,
                item.ManagerId,
                item.DepartmentCode,
                item.Email,
                item.CVLink,
                item.Noted,
                item.PermanentAddress,
                item.TemporaryAddress,
                item.DocumentStatus,
                item.Status,
                item.UpdatedBy,
                item.UpdateAt,
                item.IsActive
            };

            return await this.ExecuteSQL("sp_emp_update", parameter);
        }
        private async Task<bool> Add(Employee item)
        {
            item.CreateAt = DateTime.Now;
            item.UpdateAt = DateTime.Now;


            var parameter = new
            {
                item.FullName,
                item.NationalDate,
                item.NationalId,
                item.NationalPlace,
                item.Dob,
                item.Onboard,
                item.Phone,
                item.PositionCode,
                item.RoleCode,
                item.UserName,
                item.Pass,
                item.ManagerId,
                item.DepartmentCode,
                item.Email,
                item.CVLink,
                item.Noted,
                item.PermanentAddress,
                item.TemporaryAddress,
                item.DocumentStatus,
                item.Status,
                item.CreatedBy,
                item.UpdatedBy,
                item.CreateAt,
                item.UpdateAt,
                item.IsActive
            };

            return await this.ExecuteSQL("sp_emp_insert", parameter);
        }
        public async Task<Employee> CheckDuplicate(string email, string phone)
        {
            var parameter = new
            {
                email,

                phone
            };
            var result = await ExecuteSQL<Employee>("sp_Check_Duplicate", parameter);
            return result;
        }
        public async Task<bool> ChangePassword(string password, int id)
        {
            var parameter = new
            {
                password,
                id
            };

            return await this.ExecuteSQL("sp_emp_changePassword", parameter);
        }
        public async Task<bool> AddOrUpdate(Employee item)
        {
            if (item.Id > 0)
            {
                var itemUpdate = await GetById(item.Id);
                if (itemUpdate != null)
                {
                    itemUpdate.FullName = item.FullName;
                    itemUpdate.CreatedBy = item.CreatedBy;
                    itemUpdate.Id = item.Id;
                    itemUpdate.RoleCode = item.RoleCode;
                    itemUpdate.Dob = item.Dob;
                    itemUpdate.ManagerId = item.ManagerId;
                    itemUpdate.DepartmentCode = item.DepartmentCode;
                    itemUpdate.PositionCode = item.PositionCode;
                    itemUpdate.Email = item.Email;
                    itemUpdate.CVLink = item.CVLink;
                    itemUpdate.Phone = item.Phone;
                    itemUpdate.IsActive = item.IsActive;
                    itemUpdate.Noted = item.Noted;
                    itemUpdate.Onboard = item.Onboard;
                    itemUpdate.PermanentAddress = item.PermanentAddress;
                    itemUpdate.TemporaryAddress = item.TemporaryAddress;
                    itemUpdate.Noted = item.Noted;
                    itemUpdate.NationalId = item.NationalId;
                    itemUpdate.NationalDate = item.NationalDate;
                    itemUpdate.NationalPlace = item.NationalPlace;
                    itemUpdate.UpdatedBy = item.UpdatedBy;
                    itemUpdate.DocumentStatus = item.DocumentStatus;
                    itemUpdate.Status = item.Status;
                    return await Update(itemUpdate);
                }
            }
            return await Add(item);
        }

        public async Task<BaseList> GetAll(EmployeeRequest request)
        {
            var result = await this.GetBaseAll<EmployeeIndexModel>(request,
            new
            {
                request.From,
                request.To,
                request.Page,
                request.Token,
                request.GroupId,
                request.MemberId,
                request.IsDeleted,
                request.UserId,
                request.Limit,
                request.OrderBy


            });
            return result;
        }
        public async Task<BaseList> GetAllManager(int leadGroup = -1)
        {
            var result = await this.GetBaseAll<ManagerLeadIndex>(new BaseRequest { },
            new
            {
                groupLeadId = leadGroup
            }, sqlPro: "sp_group_getAllLead");
            return result;
        }

        public async Task<Account> GetByLineCode(string lineCode)
        {
            using (var con = GetConnection())
            {
                var sql = "select top 1  * from Employees d where d.LineCode = @LineCode  ";
                var result = await con.QuerySingleOrDefaultAsync<Account>(sql, new { LineCode = lineCode });
                return result;
            }
        }


        public async Task<bool> Delete(int id, bool reactive)
        {
            return await this.DeleteBase(id, tableDelete: "", delete: reactive == false ? 1 : 0);
        }


    }
}
