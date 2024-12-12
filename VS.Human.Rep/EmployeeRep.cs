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
                item.LineCode,

                item.FullName,
                item.Phone,
                item.Dob,
                item.ColorCode,

                item.UpdatedBy,

                item.UpdateAt,
                item.RoleCode,
                item.Noted,
                item.IsActive
            };

            return await this.ExecuteSQL("sp_emp_update", parameter);
        }
        private async Task<bool> Add(Employee item)
        {
            var itemInsert = new Employee()
            {
                FullName = item.FullName,
                IsActive = 1,
                Noted = item.Noted,
                Phone = item.Phone,
                UserName = item.UserName,
                RoleCode = item.RoleCode,
                Dob = item.Dob,
                Pass = item.Pass,
                CreatedBy = item.CreatedBy,
                UpdatedBy = item.UpdatedBy,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                Onboard = item.Onboard,
                LineCode = item.LineCode,
                ColorCode = item.ColorCode

            };
            var parameter = new
            {
                itemInsert.Onboard,
                itemInsert.LineCode,
                itemInsert.UserName,
                itemInsert.Pass,
                itemInsert.FullName,
                itemInsert.Phone,
                itemInsert.Dob,
                itemInsert.CreatedBy,
                itemInsert.UpdatedBy,
                itemInsert.CreateAt,
                itemInsert.UpdateAt,
                itemInsert.RoleCode,
                itemInsert.ColorCode,
                item.Noted,
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
                    itemUpdate.UpdatedBy = item.UpdatedBy;
                    itemUpdate.RoleCode = item.RoleCode;
                    itemUpdate.Dob = item.Dob;
                    itemUpdate.Phone = item.Phone;
                    itemUpdate.IsActive = item.IsActive;
                    itemUpdate.Noted = item.Noted;
                    itemUpdate.ColorCode = item.ColorCode;

                    itemUpdate.Onboard = item.Onboard;
                    itemUpdate.LineCode = item.LineCode;
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



        public async Task<bool> Delete(int id, bool reactive)
        {
            return await this.DeleteBase(id, tableDelete: "", delete: reactive == false ? 1 : 0);
        }


    }
}
