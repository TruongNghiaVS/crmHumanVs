using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep;
using VS.Human.Rep.Model;

namespace VS.Human.Business.Imp
{
    public class SourceMarkettingItem
    {
        public DateTime? DateAdd { get; set; }
        public string NameCandidate { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailCan { get; set; }
        public string LinkDocument { get; set; }
        public string JobCand { get; set; }
        public string NotedIn { get; set; }
        public string SourceMarketting { get; set; }
    }
    public class CandidateBusiness : BaseBusiness, ICandidateBusiness
    {
        public CandidateBusiness(IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, httpContextAccessor)
        {

        }
        public async Task<bool> AddOrUpdate(CandidateAdd item)
        {
            item.CreatedBy = GetUserId();
            item.UpdatedBy = item.CreatedBy;
            return await _unitOfWork.CandidateRep.AddOrUpdate(item);
        }


        private string ReadvalueStringExcel(ExcelWorksheet excelworksheet, int row, int col)
        {
            var cellRange = excelworksheet.Cells[row, col];
            if (cellRange != null)
            {

                if (cellRange.Value != null)
                {
                    return cellRange.Value.ToString();
                }

            }
            return "";

        }


        private DateTime? ReadvalueDateExcel(ExcelWorksheet excelworksheet, int row, int col)
        {
            try
            {
                var cellRange = excelworksheet.Cells[row, col];
                if (cellRange != null)
                {

                    if (cellRange.Text != null)
                    {
                        try
                        {
                            var dtp = DateTime.Parse(cellRange.Text.ToString());
                            return dtp;
                            return DateTime.ParseExact(cellRange.Text.Trim(), "dd/MM/yyyy", null);

                        }
                        catch (Exception)
                        {

                            try
                            {
                                return DateTime.ParseExact(cellRange.Text.Trim(), "MM/dd/yyyy", null);
                            }
                            catch (Exception)
                            {

                                try
                                {
                                    var dtp = DateTime.Parse(cellRange.Text.ToString());
                                    return dtp;
                                }
                                catch (Exception)
                                {
                                    try
                                    {
                                        return DateTime.ParseExact(cellRange.Text.Trim(), "dd/MM/yyyy", null);
                                    }
                                    catch (Exception)
                                    {

                                        return null;
                                    }
                                    return null;
                                }

                                return null;
                            }


                            return null;
                        }

                    }

                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }



        }



        private DateTime? ReadvalueDateExcel2(ExcelWorksheet excelworksheet, int row, int col)
        {
            try
            {
                var cellRange = excelworksheet.Cells[row, col];
                if (cellRange != null)
                {

                    if (cellRange.Text != null)
                    {
                        try
                        {
                            var dtp = DateTime.Parse(cellRange.Text.ToString());
                            return dtp;
                            return DateTime.ParseExact(cellRange.Text.Trim(), "dd/MM/yyyy", null);

                        }
                        catch (Exception)
                        {
                            try
                            {
                                return DateTime.ParseExact(cellRange.Text.Trim(), "MM/dd/yyyy", null);

                            }
                            catch (Exception)
                            {
                                try
                                {
                                    var dtp = DateTime.Parse(cellRange.Text.ToString());
                                    return dtp;
                                    return null;
                                }
                                catch (Exception)
                                {

                                    return null;
                                }

                                return null;
                            }

                            return null;
                        }

                    }

                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }


        }

        private float ReadvaluefloatExcel(ExcelWorksheet excelworksheet, int row, int col)
        {
            try
            {
                var cellRange = excelworksheet.Cells[row, col];
                if (cellRange == null)
                {
                    return 0;
                }

                if (cellRange.Value == null)

                {

                    return 0;
                }
                var valueCell = cellRange.Value.ToString();

                float b1 = 0;
                if (!float.TryParse(valueCell, out b1))
                {

                }
                return b1;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        private int ReadvalueintExcel(ExcelWorksheet excelworksheet, int row, int col)
        {
            try
            {
                var cellRange = excelworksheet.Cells[row, col];
                if (cellRange == null)
                {
                    return 0;
                }

                if (cellRange.Value == null)

                {

                    return 0;
                }
                var valueCell = cellRange.Value.ToString();

                int b1 = 0;
                if (!int.TryParse(valueCell, out b1))
                {

                }
                return b1;
            }
            catch (Exception)
            {

                return 0;
            }

        }

        private string? ReadvaluestringExcelWidthNull(ExcelWorksheet excelworksheet, int row, int col)
        {
            try
            {
                var cellRange = excelworksheet.Cells[row, col];
                if (cellRange == null)
                {
                    return null;
                }

                if (cellRange.Value == null)

                {

                    return null;
                }
                var valueCell = cellRange.Value.ToString();


                return valueCell;
            }
            catch (Exception)
            {

                return "";
            }

        }


        public async Task<bool> ImportSource(IFormFile item)
        {

            var fileHandler = item;
            if (fileHandler == null)
            {
                return false;
            }
            var listInput = new List<SourceMarkettingItem>();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            await using (MemoryStream ms = new MemoryStream())
            {
                await fileHandler.CopyToAsync(ms);


                using (ExcelPackage package = new ExcelPackage(ms))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                    int totalRows = workSheet.Dimension.Rows;
                    var k = 0;
                    for (int i = 2; i <= totalRows; i++)
                    {
                        k = i;


                        if (i < 1)
                        {
                            continue;
                        }
                        try
                        {
                            var dateAdd = ReadvalueDateExcel(workSheet, i, 2);
                            var nameCandidate = ReadvaluestringExcelWidthNull(workSheet, i, 3);
                            var phoneNumber = ReadvaluestringExcelWidthNull(workSheet, i, 4);
                            var emailCan = ReadvaluestringExcelWidthNull(workSheet, i, 5);
                            var linkDocument = ReadvaluestringExcelWidthNull(workSheet, i, 6);
                            var jobCand = ReadvaluestringExcelWidthNull(workSheet, i, 7);
                            var notedIn = ReadvaluestringExcelWidthNull(workSheet, i, 8);
                            var sourceMarketting = ReadvaluestringExcelWidthNull(workSheet, i, 9);

                            var itemAdd = new SourceMarkettingItem
                            {
                                DateAdd = dateAdd,
                                NameCandidate = nameCandidate,
                                PhoneNumber = phoneNumber,
                                EmailCan = emailCan,
                                LinkDocument = linkDocument,
                                JobCand = jobCand,
                                NotedIn = notedIn,
                                SourceMarketting = sourceMarketting

                            };
                            listInput.Add(itemAdd);
                        }
                        catch (Exception)
                        {

                            return false;
                        }

                    }

                }
            }

            foreach (var item1 in listInput)
            {
                var iteminsert = item1;
                var temp = iteminsert.SourceMarketting;
                int? sourcetemp = 1;
                try
                {
                    sourcetemp = int.Parse(temp);
                }
                catch (Exception)
                {
                    sourcetemp = 1;
                }
                var candidate = new Candidate()
                {
                    Name = iteminsert.NameCandidate,
                    Email = iteminsert.EmailCan,
                    Phone = iteminsert.PhoneNumber,
                    IsActive = 1,
                    AvatarLink = "",
                    ShortDes = iteminsert.LinkDocument,
                    CVLink = "",
                    Noted = iteminsert.NotedIn,
                    CreatedBy = GetUserId(),
                    UpdatedBy = GetUserId(),
                    Source = int.Parse(iteminsert.SourceMarketting),
                    Dob = null,
                    Status = 0
                };
                var jobItem = await _unitOfWork.JobItemRep.GetById(int.Parse(iteminsert.JobCand));
                if (jobItem == null)
                {
                    continue;
                }
                var orderInsert = new Order()
                {
                    ShortDes = iteminsert.LinkDocument,
                    JobId = jobItem.Id,
                    Assignee = -1,
                    Source = sourcetemp,
                    CandidateId = candidate.Id
                };
                var objectInsert = new
                {
                    iteminsert.NameCandidate,
                    iteminsert.LinkDocument,
                    iteminsert.DateAdd,
                    iteminsert.PhoneNumber,
                    iteminsert.EmailCan,
                    iteminsert.SourceMarketting,
                    iteminsert.NotedIn,
                    orderInsert.JobId,
                    orderInsert.Assignee,
                    createBy = GetUserId(),
                };
                await _unitOfWork.OrderRep.ImportSourMarketting(objectInsert);

            }
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            return await _unitOfWork.CandidateRep.Delete(id);
        }

        public async Task<BaseList> GetAll(CandidateRequest request)
        {
            return await _unitOfWork.CandidateRep.GetAll(request);
        }

        public async Task<Candidate> GetById(int id)
        {
            return await _unitOfWork.CandidateRep.GetById(id);
        }
        public async Task<bool> AddCandidateWidthOrder(CandidateOrderAdd item)
        {
            item.CreateBy = GetUserId();

            return await _unitOfWork.CandidateRep.AddCandidateWidthOrder(item);
        }


    }
}
