﻿using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace VS.Human.Rep
{
    public class RepositoryBaseNoTModel
    {
        protected IDbConnection _connection { get; set; }
        protected readonly IConfiguration _configuration;
        protected int offset;
        protected string tableName = "Employees";
        protected string sqlGetALl = "sp_Employee_getAll";
        public RepositoryBaseNoTModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(configuration.GetConnectionString("stringConnect7"));
        }
        protected IDbConnection GetConnection()
        {
            var con = new SqlConnection(_configuration.GetConnectionString("stringConnect7"));
            con.Open();
            return con;
        }
        public async Task<bool> Delete(int id)
        {
            return await DeleteBase(id, tableDelete: tableName);
        }
        protected DynamicParameters AddOutputParam(string name, DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            p.Add(name, dbType: type, direction: ParameterDirection.Output);
            return p;
        }
        protected DynamicParameters GetParams<T>(T model, string[] ignoreKey = null, string outputParam = "", DbType type = DbType.Int32)
        {
            var p = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(outputParam))
                p.Add(outputParam, dbType: type, direction: ParameterDirection.Output);
            var properties = model.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var key = prop.Name;
                var value = prop.GetValue(model);
                if (ignoreKey != null && ignoreKey.Contains(key))
                    continue;
                if (!string.IsNullOrWhiteSpace(outputParam))
                {
                    if (key.ToLower() == outputParam.ToLower())
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrWhiteSpace(key))
                    p.Add(key, value);
            }
            return p;
        }
        protected void ProcessInputPaging(ref int page, ref int limit, out int offset)
        {
            page = page <= 0 ? 1 : page;
            if (limit <= 0)
                limit = 20;
            if (limit > 1000)
                limit = 100;
            offset = (page - 1) * limit;
        }

        public async Task<bool> ExecuteSQL(string sql = "",
            object parameter = null,
            CommandType commandType = CommandType.StoredProcedure)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            if (parameter == null)
            {
                parameter = new DynamicParameters();
            }

            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.ExecuteAsync(sql, param: parameter, commandType: commandType);

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;

            }

        }


        public async Task<List<TIndexModel>> ExecuteSQL<TIndexModel>(string sql = "",
            object parameter = null,
            CommandType commandType = CommandType.StoredProcedure)
            where TIndexModel : BaseIndexModel

        {
            if (string.IsNullOrEmpty(sql))
            {
                return new List<TIndexModel>();
            }
            if (parameter == null)
            {
                parameter = new DynamicParameters();
            }

            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QueryAsync<TIndexModel>(sql, param: parameter, commandType: commandType);

                    if (result.Any())
                    {
                        return result.ToList();
                    }
                    return new List<TIndexModel>();
                }
            }
            catch (Exception e)
            {
                return new List<TIndexModel>();

            }



        }


        public async Task<TResult> GetData<TResult, TRequest, TindexModel>(
            TRequest request,
            string sql
            ) where TResult : BaseList, new()
            where TRequest : BaseRequest, new()
            where TindexModel : BaseIndexModel, new()
        {
            int page = request.Page;
            int limit = request.Limit;
            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<TindexModel>(sql,
                    request, commandType: CommandType.StoredProcedure);

                    var fistElement = result.FirstOrDefault();
                    var totalRecord = 0;
                    if (fistElement != null)
                    {

                        totalRecord = 10;
                    }

                    var reponse = new TResult()
                    {
                        Total = totalRecord,
                        Data = result
                    };

                    return reponse;
                }
            }
            catch (Exception e)
            {
                return new TResult();
            }
        }


        public async Task<T> ExecuteSQL<T>(string sql = "",
            object parameter = null)
            where T : BaseModel, new()

        {
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            if (parameter == null)
            {
                parameter = new DynamicParameters();
            }

            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.QuerySingleOrDefaultAsync<T>(sql, param: parameter);

                    if (result == null)
                    {
                        return new T()
                        {
                            Id = -1
                        };
                    }
                    return result;
                }
            }
            catch (Exception e)
            {

                return new T()
                {
                    Id = -1
                };

            }

        }
        public async Task<bool> ExecuteSQL(string sql = "",
         object parameter = null)

        {
            if (string.IsNullOrEmpty(sql))
            {
                return false;
            }
            if (parameter == null)
            {
                return false;
            }

            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.ExecuteAsync(sql, param: parameter);

                    if (result == null)
                    {
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception e)
            {

                return false;

            }

        }

        public async Task<bool> DeleteBase(int id, int delete = 1)
        {
            var _baseTable = tableName;

            var sql = "UPDATE " + "[" + _baseTable + "]" + " SET Deleted= @del  WHERE Id = @id";
            return await ExecuteSQL(sql, new
            {
                Id = id,
                del = delete
            });
        }
        public async Task<bool> DeleteBase(int id, int delete = 1, string tableDelete = "")
        {
            var _baseTable = tableName;
            if (!string.IsNullOrEmpty(tableDelete))
            {
                tableName = tableDelete;
            }
            var sql = "UPDATE " + "[" + _baseTable + "]" + " SET Deleted= @del  WHERE Id = @id";
            return await ExecuteSQL(sql, new
            {
                Id = id,
                del = delete
            });
        }


        public async Task<BaseList> GetBaseAll<TIndexModel>(
             BaseRequest request,
             dynamic parameter,
             string sqlPro = ""


            ) where
            TIndexModel : BaseIndexModel
        {
            int page = request.Page;
            int limit = request.Limit;


            if (parameter == null)
            {
                parameter = new
                {
                    request.From,
                    request.To,
                    request.Limit,
                    request.Page,
                    request.Token,
                    request.OrderBy,
                    request.UserId

                };
            }
            else
            {
                //parameter.From = request.From;
                //parameter.To = request.To;
                //parameter.Limit = request.Limit;
                //parameter.Page = request.Page;
            }



            try
            {
                using (var con = GetConnection())
                {
                    var sqlGet = sqlGetALl;
                    if (!string.IsNullOrEmpty(sqlPro))
                    {
                        sqlGet = sqlPro;
                    }
                    var result = await con.QueryAsync<TIndexModel>(sqlGet,
                        parameter as object, commandType: CommandType.StoredProcedure);

                    var fistElement = result.FirstOrDefault();
                    var totalRecord = 0;
                    if (fistElement != null)
                    {
                        totalRecord = fistElement.TotalRecord;
                    }

                    var reponse = new BaseList()
                    {
                        Total = totalRecord,

                        Data = result
                    };


                    return reponse;
                }
            }
            catch (Exception e)
            {
                return new BaseList()
                {
                    Data = null,
                    Total = 0,
                };
            }
        }
    }
}