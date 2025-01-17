﻿using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using VS.Human.Rep.Model;

namespace VS.Human.Rep
{
    public class ReportTalkTimeRepository : RepositoryBaseNoTModel, IReportTalkTimeRepository
    {

        public ReportTalkTimeRepository(IConfiguration configuration)
            : base(configuration)
        {

            tableName = "MasterData";
            sqlGetALl = "sp_masterData_getAll";
        }




        public async Task<int> Add(ReportTalkTime entity)
        {
            entity.CreateAt = DateTime.Now;
            entity.UpdateAt = DateTime.Now;
            var par = GetParams(entity, new string[] {
                nameof(entity.UpdatedBy),
                nameof(entity.UpdateAt),
                nameof(entity.Id),
                nameof(entity.CreateAt),
                nameof(entity.Deleted),
                nameof(entity.Lastapp),
                nameof(entity.LastData)
            }, "Id");

            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.ExecuteAsync("sp_ReportTalkTime_Insert2", par, commandType: CommandType.StoredProcedure);
                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;

            }

        }
        public async Task<bool> CheckDuplicate(string code)
        {
            using (var con = GetConnection())
            {
                var sql = "SELECT * FROM " + tableName + " WHERE code = @Code";
                var result = await con.QuerySingleOrDefaultAsync<ImpactHistoryIndexModel>(sql, new { Code = code });
                if (result == null)
                {
                    return false;
                }
                return true;
            }
        }

        public async Task<int> DeleteAll()
        {
            using (var con = GetConnection())
            {
                var sql = "delete from  " + tableName + " WHERE 1 =1";
                var result = await con.QuerySingleOrDefaultAsync(sql);

                return 1;
            }
        }

        public async Task<int> UpdateFileDeleted(string filePath)
        {
            using (var con = GetConnection())
            {
                var sql = "update ReportTalkTime set deleteFile = 1 where  FileRecording = @filePath  ";
                var result = await con.QueryAsync(sql, new
                {
                    filePath
                }, commandType: CommandType.Text);

                return 1;
            }
        }




        public async Task<int> DeleteAllRangeFromTo(DateTime from, DateTime to)
        {
            using (var con = GetConnection())
            {
                var sql = "delete from  " + tableName + " WHERE CallDate >= @from and CallDate <= @to  ";
                var result = await con.QuerySingleOrDefaultAsync(sql, new
                {
                    from,
                    to
                });

                return 1;
            }
        }

        public async Task<DateTime?> GetMaxLinked(string type)
        {
            using (var con = GetConnection())
            {
                var sql = "select max(CallDate) from  " + tableName + " WHERE 1 =1 and sourceCall = " + type + "";
                var result = await con.QueryFirstAsync<DateTime?>(sql);
                if (result == null)
                {
                    return null;
                }
                return result;
            }
        }
        public async Task<List<ReportTalkTimeIndexModel>> GetAllDeleted()
        {
            ReportTalkTimeRequest request = new ReportTalkTimeRequest()
            {
                Limit = 100
            };
            int page = request.Page;
            int limit = request.Limit;

            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<ReportTalkTimeIndexModel>("sp_ReportTalkTime_getAllNotDeleteFile", new
                    {
                        request.Token,
                        request.From,
                        request.To,

                        request.Limit,
                        request.Page,
                        request.OrderBy
                    }, commandType: CommandType.StoredProcedure);
                    return result.ToList();
                }
            }
            catch (Exception e)
            {
                return new List<ReportTalkTimeIndexModel>();
            }
        }
        public async Task<ReportTalkTimeReponse> GetALl(ReportTalkTimeRequest request)
        {
            int page = request.Page;
            int limit = request.Limit;

            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryAsync<ReportTalkTimeIndexModel>("sp_ReportTalkTime_getAllAutoCall", new
                    {
                        request.Token,
                        request.From,
                        request.To,

                        request.Limit,
                        request.Page,
                        request.OrderBy
                    }, commandType: CommandType.StoredProcedure);

                    var fistElement = result.FirstOrDefault();
                    var totalRecord = 0;
                    if (fistElement != null)
                    {
                        totalRecord = fistElement.TotalRecord;
                    }
                    var reponse = new ReportTalkTimeReponse()
                    {
                        Total = totalRecord,

                        Data = result
                    };
                    return reponse;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<int> Update(ReportTalkTime entity)
        {
            entity.CreateAt = DateTime.Now;
            entity.UpdateAt = DateTime.Now;
            var par = GetParams(entity, new string[] {
                nameof(entity.UpdateAt),
                nameof(entity.CreateAt),
                nameof(entity.Deleted),
                nameof(entity.CreatedBy)
            });
            try
            {
                using (var _con = GetConnection())
                {
                    var result = await _con.ExecuteAsync("sp_ReportTalkTime_Update", par, commandType: CommandType.StoredProcedure);

                    return 1;
                }
            }
            catch (Exception e)
            {
                return 0;

            }
        }

        public async Task<IEnumerable<ReportQuerryNewTaltimeIndex>> HandlelFileRecording(HandlelFileRecordingRequest request)
        {
            int page = request.Page;
            int limit = request.Limit;

            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetMysqlConnection2())
                {
                    var sqlQuerry = "SELECT d.cnum AS 'LineCode', '1' as SourceCall ,  d.dst AS  'PhoneLog', d.linkedid AS 'Linkedid', d.calldate,  d.disposition, d.billsec AS 'DurationBill', d.duration AS 'Duration', d.recordingfile AS 'FileRecording'  \r\n\r\nFROM cdr d WHERE  d.lastapp = 'Dial'\r\n\r\n  AND d.calldate >= @timeFrom and d.calldate <= @timeTo and d.lastapp = 'Dial' ";
                    var result = await con.QueryAsync<ReportQuerryNewTaltimeIndex>(sqlQuerry, new
                    {
                        request.Token,
                        request.From,
                        request.To,
                        request.TimeFrom,
                        request.TimeTo,
                        request.Linked,

                        request.Limit,
                        request.Page,
                        request.OrderBy
                    });
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public async Task<IEnumerable<ReportQuerryNewTaltimeIndex>> ProcessingCal(HandlelFileRecordingRequest request)
        {
            int page = request.Page;
            int limit = request.Limit;
            request.From = DateTime.Now.AddDays(-4);
            request.To = DateTime.Now.AddDays(1);
            request.TimeFrom = request.From;
            request.TimeTo = request.To;
            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetMysqlConnection3())
                {

                    if (request.Linked.HasValue)
                    {

                    }
                    else
                    {
                        request.Linked = DateTime.UtcNow.AddDays(-5);
                    }
                    var sqlQuerry = "SELECT d.cnum AS 'LineCode', '2' as SourceCall,  d.dst AS  'PhoneLog', d.linkedid AS 'Linkedid', d.calldate,  d.disposition, d.billsec AS 'DurationBill', d.duration AS 'Duration', d.recordingfile AS 'FileRecording'  FROM cdr d WHERE   d.calldate >= @timeFrom and d.calldate <= @timeTo and d.lastapp = 'Dial'  ORDER BY d.calldate  desc";
                    var result = await con.QueryAsync<ReportQuerryNewTaltimeIndex>(sqlQuerry, new
                    {
                        request.Token,
                        request.From,
                        request.To,
                        request.TimeFrom,
                        request.TimeTo,
                        request.Linked,
                        request.Limit,
                        request.Page,
                        request.OrderBy
                    });
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<IEnumerable<ReportQuerryNewTaltimeIndex>> HandlelFileRecordingServeAutoCall(HandlelFileRecordingRequest request)
        {
            int page = request.Page;
            int limit = request.Limit;

            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetMysqlConnection3())
                {
                    if (request.Linked.HasValue)
                    {

                    }
                    else
                    {
                        request.Linked = DateTime.UtcNow.AddDays(-5);
                    }
                    var sqlQuerry = "SELECT d.cnum AS 'LineCode', '3' as SourceCall,  d.userfield AS  'PhoneLog', d.lastapp,  d.linkedid AS 'Linkedid', d.calldate,  d.disposition, d.billsec AS 'DurationBill', d.duration AS 'Duration', d.recordingfile AS 'FileRecording'  FROM cdr d WHERE   d.calldate >= @timeFrom and d.calldate <= @timeTo  and d.lastapp = 'Dial'  AND  d.dcontext ='outgoing_calls'  AND d.disposition = 'ANSWERED' ";
                    var result = await con.QueryAsync<ReportQuerryNewTaltimeIndex>(sqlQuerry, new
                    {
                        request.Token,
                        request.From,
                        request.To,
                        request.TimeFrom,
                        request.TimeTo,
                        request.Linked,
                        request.Limit,
                        request.Page,
                        request.OrderBy
                    });
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<IEnumerable<ReportQuerryNewTaltimeIndex>> HandlelFileRecordingServe3(HandlelFileRecordingRequest request)
        {
            int page = request.Page;
            int limit = request.Limit;

            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetMysqlConnection4())
                {
                    if (request.Linked.HasValue)
                    {

                    }
                    else
                    {
                        request.Linked = DateTime.Now.AddDays(-5);
                    }
                    var sqlQuerry = "SELECT d.cnum AS 'LineCode', '3' as SourceCall,  d.dst AS  'PhoneLog', d.linkedid AS 'Linkedid', d.calldate,  d.disposition, d.billsec AS 'DurationBill', d.duration AS 'Duration', d.recordingfile AS 'FileRecording'  FROM cdr d WHERE   d.calldate >= @timeFrom and d.calldate <= @timeTo and d.lastapp = 'Dial'";

                    var result = await con.QueryAsync<ReportQuerryNewTaltimeIndex>(sqlQuerry, new
                    {
                        request.Token,
                        request.From,
                        request.To,
                        request.TimeFrom,
                        request.TimeTo,
                        request.Linked,
                        request.Limit,
                        request.Page,
                        request.OrderBy
                    });
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<IEnumerable<ReportQuerryNewTaltimeIndex>> HandlelFileRecordingServe4(HandlelFileRecordingRequest request)
        {
            int page = request.Page;
            int limit = request.Limit;

            ProcessInputPaging(ref page, ref limit, out offset);
            try
            {
                using (var con = GetMysqlConnection5())
                {
                    if (request.Linked.HasValue)
                    {

                    }
                    else
                    {
                        request.Linked = DateTime.UtcNow.AddDays(-5);
                    }
                    var sqlQuerry = "SELECT d.cnum AS 'LineCode', '4' as SourceCall,  d.dst AS  'PhoneLog', d.linkedid AS 'Linkedid', d.calldate,  d.disposition, d.billsec AS 'DurationBill', d.duration AS 'Duration', d.recordingfile AS 'FileRecording'  FROM cdr d WHERE  d.calldate >= @timeFrom and d.calldate <= @timeTo and d.lastapp = 'Dial'"; ;
                    var result = await con.QueryAsync<ReportQuerryNewTaltimeIndex>(sqlQuerry, new
                    {
                        request.Token,
                        request.From,
                        request.To,
                        request.TimeFrom,
                        request.TimeTo,
                        request.Linked,
                        request.Limit,
                        request.Page,
                        request.OrderBy
                    });
                    return result;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }



        public async Task<ReportQuerryRecordingFileIndex> GetInfomationRecording(string likiedId)
        {

            try
            {
                using (var con = GetMysqlConnection3())
                {
                    var sqlQuerry = "SELECT * FROM recording_files d WHERE d.`file` LIKE '%" + likiedId + "%'";
                    var result = await con.QueryAsync<ReportQuerryRecordingFileIndex>(sqlQuerry, new
                    {

                    });

                    var fistElement = result.FirstOrDefault();
                    var totalRecord = 0;
                    if (fistElement != null)
                    {
                        totalRecord = fistElement.TotalRecord;
                    }
                    return fistElement;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        protected IDbConnection GetMysqlConnection2()
        {
            var con = new MySqlConnection(_configuration.GetConnectionString("mysqlStringConnect"));
            con.Open();
            return con;
        }


        protected IDbConnection GetMysqlConnection3()
        {
            var con = new MySqlConnection(_configuration.GetConnectionString("mysqlStringConnect3"));
            con.Open();
            return con;
        }

        protected IDbConnection GetMysqlConnection4()
        {
            var con = new MySqlConnection(_configuration.GetConnectionString("mysqlStringConnect4"));
            con.Open();
            return con;
        }

        protected IDbConnection GetMysqlConnection5()
        {
            var con = new MySqlConnection(_configuration.GetConnectionString("mysqlStringConnect6"));
            con.Open();
            return con;
        }







    }
}
