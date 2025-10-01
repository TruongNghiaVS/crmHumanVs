﻿using crmHuman.DisplayModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VS.Human.Business;
using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace crmHuman.Pages
{
    [Authorize]
    public class CandidateModel : BaseModel2
    {
        private readonly ILogger<CandidateModel> _logger;
        private readonly ICandidateBusiness _empBusiness;

        private readonly IEmpBusiness _iempl;
        public List<string> TableColumnTextAdmin { get; set; }
        public CandidateRequest RequestSearch { get; set; }
        public BaseList DataAll { get; set; }
        public List<DataMasterItem> DataMasterData { get; set; }
        private readonly ImasterDataBussiness _masterDataBussiness;
        public BaseList DataManager { get; set; }
        public int TotalRecord
        {

            get
            {
                return DataAll.Total;

            }
        }
        public CandidateModel(ILogger<CandidateModel> logger,
            ICandidateBusiness empBusiness,
            IEmpBusiness empBusiness1,
            ImasterDataBussiness imasterDataBussiness
            )
        {
            _logger = logger;
            _empBusiness = empBusiness;
            _masterDataBussiness = imasterDataBussiness;
            TitlePage = "Danh sách ứng viên";
            KeyPage = "Candidate";
            _iempl = empBusiness1;
            TableColumnText = new List<string>()
            {
                "STT","UserName","Họ tên", "Vị trí", "Bộ phận", "Loại tài khoản",
                "Người quản lý",
                "Trạng thái",
                "Trạng thái chứng từ"
               ,"Cập nhật gần nhất", "Người tạo","Thao tác"
            };

            TableColumnTextAdmin = new List<string>()
            {
                "STT","Họ tên", "Vị trí", "Bộ phận",
                 "Người quản lý","Trạng thái", "Trạng thái chứng từ",
                "Cập nhật gần nhất","Người tạo","Thao tác"
            };
            DataMasterData = new List<DataMasterItem>();

        }

        public async Task<IActionResult> OnPostAdd
            (CandidateAdd request)
        {
            var listEror = new List<object>();
            if (string.IsNullOrEmpty(request.Name))
            {
                var itemError = new
                {
                    name = "txtFullName",
                    Content = "Thiếu thông tin họ và tên"
                };
                listEror.Add(itemError);
            }

            if (string.IsNullOrEmpty(request.Phone))
            {
                var itemError = new
                {
                    name = "txtPhone",
                    Content = "Thiếu thông tin số điện thoại"
                };
                listEror.Add(itemError);
            }
            if (listEror.Count > 0)
            {
                return new JsonResult(listEror)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var result = true;
            if (request.Id < 0)
            {
                request.Status = 91;
                result = await _empBusiness.Add(request);

            }
            else
            {
                //result = await _empBusiness.Update(request);
            }
            var dataReponse = new
            {
                success = result,
            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }



        public async Task<ActionResult> OnGet([FromQuery] CandidateRequest request)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }
            GetInfoUser();
            if (UserData.RoleCode == "2")
            {
                return Redirect("/");
            }
            var temp = await _masterDataBussiness.GetAll(new CommonRequest()
            {

            });
            foreach (var item in temp.Data)
            {

                var tempItem = item as dynamic;

                var itemInsert = new DataMasterItem()
                {
                    Name = tempItem.Name,
                    TypeData = tempItem.TypeData,
                    Code = tempItem.Code,
                    ApplyFor = tempItem.ApplyFor,
                    IsActive = tempItem.IsActive
                };
                DataMasterData.Add(itemInsert);
            }
            DataManager = await _iempl.GetAllManager();
            return await GetAll(request);
        }

        public async Task<ActionResult> GetAll(CandidateRequest request2)
        {
            RequestSearch = request2;
            request2.UserId = UserData.UserId;
            if (UserData.RoleCode == "1")
            {
                request2.IsDeleted = true;
            }
            else
            {

            }
            {
                request2.IsDeleted = false;
            }

            DataAll = await _empBusiness.GetAll(request2);

            if (UserData.RoleCode == "6")
            {
                TableColumnText = new List<string>()
                    {
                        "STT","Họ tên","Tài khoản","Vai trò","Nhóm", "Ngày Onboard","Cập nhật gần nhất","Thao tác"
                    };

                TableColumnTextAdmin = new List<string>()
                    {
                        "STT","Họ tên","Tài khoản","Vai trò","Nhóm","Trạng thái", "Ngày Onboard","Cập nhật gần nhất","Thao tác"
                    };
            }
            return Page();
        }

        public virtual async Task<PartialViewResult> OnGetFormEdit(int id)

        {
            GetInfoUser();
            var resultView = new Candidate()
            {
                Id = id,

                IsActive = 1,
                Phone = "",
                Status = 1,
                CVLink = "",

                Deleted = false,
                Noted = ""

            };

            var temp = await _masterDataBussiness.GetAll(new CommonRequest()
            {

            });

            foreach (var item in temp.Data)
            {

                var tempItem = item as dynamic;

                var itemInsert = new DataMasterItem()
                {
                    Name = tempItem.Name,
                    TypeData = tempItem.TypeData,
                    Code = tempItem.Code,
                    IsActive = tempItem.IsActive
                };
                DataMasterData.Add(itemInsert);
            }

            DataManager = await _iempl.GetAllManager();

            var resultModel = new
            {
                DataManager,
                resultView,
                DataMasterData
            };


            if (id < 1)
            {
                return Partial("EditOrUpdateCandidate", resultModel);
            }
            if (id > 0)
            {
                resultView = await _empBusiness.GetById(id);
            }

            return Partial("editOrUpdateEmployee", resultView);
        }

        public virtual async Task<PartialViewResult> OnGetFormChangePassword(int id)

        {

            var resultView = new
            {
                Id = id
            };
            return Partial("formChangePassword", resultView);
        }

        public virtual async Task<PartialViewResult> OnGetFormImportCandidate()

        {
            var dataCandidate = await _empBusiness.GetAll(new CandidateRequest()
            {
                IsEmployee = true
            });
            var resultView = new
            {
                dataAll = dataCandidate.Data
            };
            return Partial("FormImportCandidate", resultView);
        }

        public async Task<IActionResult> OnPostDelete
      (int Id = -1)
        {

            var listEror = new List<object>();

            if (Id < 0)
            {
                var itemError = new
                {
                    name = "id",
                    Content = "Thiếu thông tin cần xoá"
                };
                listEror.Add(itemError);

            }
            if (listEror.Count > 0)
            {
                return new JsonResult(listEror)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var result = true;

            result = await _empBusiness.Delete(Id);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }


        public async Task<IActionResult> OnPostReactive
     (int Id = -1)
        {

            var listEror = new List<object>();

            if (Id < 0)
            {
                var itemError = new
                {
                    name = "id",
                    Content = "Thiếu thông tin cần xoá"
                };
                listEror.Add(itemError);

            }
            if (listEror.Count > 0)
            {
                return new JsonResult(listEror)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var result = true;

            result = await _empBusiness.Delete(Id, true);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }



    }
}
