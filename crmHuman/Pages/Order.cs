using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VS.Human.Business;
using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace crmHuman.Pages
{
    [Authorize]
    public class OrderModel : BaseModel2
    {
        private readonly ILogger<OrderModel> _logger;
        private readonly IOrderBussiness _orderBussiness;
        private readonly IJobItemBusiness _jobItemBusiness;
        private readonly ICandidateBusiness _candidateBusiness;
        private readonly ImasterDataBussiness _masterDataBussiness;
        private readonly IPartnerBusiness _partnerBusiness;
        public OrderRequest RequestSearch { get; set; }
        public BaseList DataAll { get; set; }
        public BaseList JobList { get; set; }



        public int TotalRecord
        {

            get
            {
                return DataAll.Total;

            }
        }
        public OrderModel(ILogger<OrderModel> logger,
            IOrderBussiness empBusiness,
            IJobItemBusiness jobItemBusiness,
            ICandidateBusiness candidateBusiness,
            IPartnerBusiness partnerBusiness,
            ImasterDataBussiness masterDataBussiness
            )
        {
            _logger = logger;
            _orderBussiness = empBusiness;
            _jobItemBusiness = jobItemBusiness;
            TitlePage = "Danh sách đơn hàng";
            KeyPage = "Order";
            _candidateBusiness = candidateBusiness;
            _partnerBusiness = partnerBusiness;
            _masterDataBussiness = masterDataBussiness;

            TableColumnText = new List<string>()
            {
                "STT","Mã","Ứng cử viên","Vị trí",
                "Đối tác/dự án","Trạng thái","Người tạo",
                "Xử lý bởi",
                "Ngày nhận",
                "Cập nhật gần nhất","Thao tác"
            };
        }


        public async Task<IActionResult> OnPostAddInfo(OrderInfoItemAdd request)
        {
            var listEror = new List<object>();
            if (listEror.Count > 0)
            {
                return new JsonResult(listEror)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            request.Source = 0;
            request.Isapply = 0;

            var result = await _orderBussiness.AddOrUpdateInfoOrder(request);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }


        public async Task<IActionResult> OnPostChangeStatusApply(ApplyToStatus request)
        {
            if (request.OrderId.HasValue == false)
            {
                var errorMessage = new
                {
                    Message = "không có đối tượng",
                    success = true
                };
                return new JsonResult(errorMessage)
                {
                    StatusCode = StatusCodes.Status200OK
                };
            }
            var orderInfo = await _orderBussiness.GetInfo(request.OrderId.Value);
            var candidateInfo = await _candidateBusiness.GetById(orderInfo.CandidateId.Value);
            if (string.IsNullOrEmpty(candidateInfo.Phone) || candidateInfo.Phone.Length != 10)
            {
                return new JsonResult(new
                {
                    Message = " Số điện thoại không hợp lệ, cập nhật lại",
                    success = false
                })
                {
                    StatusCode = StatusCodes.Status200OK

                };

            }
            if (string.IsNullOrEmpty(orderInfo.CVLink))
            {
                return new JsonResult(new
                {
                    Message = "Yêu cầu cập nhật lại file CV",
                    success = false
                })
                {
                    StatusCode = StatusCodes.Status200OK

                };
            }


            if (candidateInfo.Dob.HasValue == false)
            {
                return new JsonResult(new
                {
                    Message = "Yêu cầu nhập ngày sinh",
                    success = false
                })
                {
                    StatusCode = StatusCodes.Status200OK

                };

            }
            if (
                string.IsNullOrEmpty(orderInfo.Regional) ||
                string.IsNullOrEmpty(orderInfo.SchoolName) ||
                string.IsNullOrEmpty(orderInfo.Introduction) ||
                orderInfo.Experience < 1 ||
                orderInfo.RankLevel < 1 ||
               orderInfo.Gender < 1)
            {
                return new JsonResult(new
                {
                    Message = "Thông tin đang còn thiếu, vui lòng cập nhật danh sách ứng viên các mục trong mục thông tin thêm",
                    success = false
                })
                {
                    StatusCode = StatusCodes.Status200OK

                };
            }
            var result = await _orderBussiness.ChangeStatusApply(request);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }

        public async Task<IActionResult> OnPostAdd(OrderItemAdd request)
        {
            var listEror = new List<object>();
            if (request.CandidateId < 1)
            {
                var itemError = new
                {

                    name = "cbcandidateId",
                    Content = "Thiếu thông tin ứng cử viên"
                };
                listEror.Add(itemError);

            }
            if (request.JobId < 1)
            {
                var itemError = new
                {
                    name = "cbJobId",
                    Content = "Thiếu thông tin việc làm"
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
            request.Source = 0;

            var jobRequest = await _jobItemBusiness.GetById(request.JobId.Value);
            if (jobRequest == null)
            {
                var itemError = new
                {
                    name = "cbJobId",
                    Content = "Thiếu thông tin việc làm"
                };
                listEror.Add(itemError);
            }

            request.PartnerId = jobRequest.PartnerId;
            request.ProjectId = jobRequest.ProjectId;

            var result = await _orderBussiness.AddOrUpdate(request);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }





        public async Task<IActionResult> OnPostAddOrderMarketting(OrderItemAdd request)
        {
            var listEror = new List<object>();
            if (request.CandidateId < 1)
            {
                var itemError = new
                {
                    name = "cbcandidateId",
                    Content = "Thiếu thông tin ứng cử viên"
                };
                listEror.Add(itemError);
            }
            if (request.JobId < 1)
            {
                var itemError = new
                {
                    name = "cbJobId",
                    Content = "Thiếu thông tin việc làm"
                };
                listEror.Add(itemError);
            }

            var jobRequest = await _jobItemBusiness.GetById(request.JobId.Value);
            if (jobRequest == null)
            {
                var itemError = new
                {
                    name = "cbJobId",
                    Content = "Thiếu thông tin việc làm"
                };
                listEror.Add(itemError);
            }
            var candidateItem = await _candidateBusiness.GetById(request.CandidateId.Value);
            request.PartnerId = jobRequest.PartnerId;
            request.ProjectId = jobRequest.ProjectId;
            if (request.Source > 0)
            {
                if (candidateItem == null)
                {
                    var itemError = new
                    {
                        name = "cbcandidateId",
                        Content = "Thiếu thông tin ứng cử viên"
                    };
                    listEror.Add(itemError);
                }
            }
            else
            {
                request.Source = candidateItem.Source;
            }



            if (listEror.Count > 0)
            {
                return new JsonResult(listEror)
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            var result = await _orderBussiness.AddOrUpdate(request);
            var dataReponse = new
            {
                success = result
            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }





        public async Task<ActionResult> OnGet([FromQuery] OrderRequest request)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }
            GetInfoUser();
            if (UserData.RoleCode == "2")
            {
                this.Permision.SearchGrop = false;
            }

            return await GetAll(request);
        }

        public virtual async Task<PartialViewResult> OnGetFormEdit(int id)

        {
            GetInfoUser();
            var resultView = new Candidate()
            {
                Id = id,
                Name = "",
                Email = "",
                IsActive = 1,
                Status = 1,
                Deleted = false,
                Noted = ""
            };
            var allJob = await _jobItemBusiness.GetAll(new JobRequest()
            {
            });
            var allStatus = await _masterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 4
            });
            var allOrder = await _orderBussiness
            .GetALlHistory(new OrderRequest()
            {
                Phone = resultView.Phone,
                Email = resultView.Email
            });
            var resultObject = new
            {
                allJob,
                resultView,
                allStatus,
                allOrder

            };
            if (UserData.RoleCode == "4")
            {
                return Partial("EditOrUpdateCandidateMar", resultObject);
            }


            return Partial("EditOrUpdateCandidate", resultObject);
        }

        public async Task<ActionResult> GetAll(OrderRequest request2)
        {

            request2.UserId = UserData.UserId;
            request2.Marketting = 0;
            request2.RoleCode = UserData.RoleCode;
            if (request2.RoleCode == "4" || request2.RoleCode == "6" || request2.RoleCode == "7")
            {
                request2.Tracking = true;
                this.Permision.SearchGrop = false;
                this.Permision.SearchGrop = false;
            }



            if (UserData.RoleCode == "3")
            {

                if (UserData.UserId == 37)
                {
                    request2.GroupId = 6;


                }
                else if (UserData.UserId == 38)
                {
                    request2.GroupId = 7;

                }
            }

            RequestSearch = request2;

            JobList = await _jobItemBusiness.GetAll(new JobRequest());
            request2.Isapply = 0;
            DataAll = await _orderBussiness.GetAll(request2);
            return Page();
        }


        public async Task<IActionResult> OnPostDelete(int Id = -1)
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

            result = await _orderBussiness.DeleteOrder(Id);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }


        public async Task<IActionResult> OnPostChangeResult(OrderResultRequest request)
        {
            var listEror = new List<object>();
            if (request.Id < 1)
            {
                var itemError = new
                {
                    name = "idREquest",
                    Content = "Thiếu  thông tin đơn hàng"
                };
                listEror.Add(itemError);
            }
            if (!request.Result.HasValue)
            {
                var itemError = new
                {
                    name = "result",
                    Content = "Thiếu thông tin kết quả"
                };
                listEror.Add(itemError);
            }
            if (listEror.Count > 0)
            {
                var dataReponse2 = new
                {
                    success = false,

                };
                return new JsonResult(dataReponse2)
                {
                    StatusCode = StatusCodes.Status200OK

                };
            }
            var result = await _orderBussiness.OrderResultRequest(request);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }


        public async Task<IActionResult> OnPostPushCaseCTV(OrderResultRequest request)
        {
            var listEror = new List<object>();
            if (request.Id < 1)
            {
                var itemError = new
                {
                    name = "idREquest",
                    Content = "Thiếu  thông tin đơn hàng"
                };
                listEror.Add(itemError);
            }
            //if (!request.Result.HasValue)
            //{
            //    var itemError = new
            //    {
            //        name = "result",
            //        Content = "Thiếu thông tin kết quả"
            //    };
            //    listEror.Add(itemError);
            //}
            if (listEror.Count > 0)
            {
                var dataReponse2 = new
                {
                    success = false,

                };
                return new JsonResult(dataReponse2)
                {
                    StatusCode = StatusCodes.Status200OK

                };
            }
            var result = await _orderBussiness.ChangePushCaseCTV(request);
            var dataReponse = new
            {
                success = result,

            };
            return new JsonResult(dataReponse)
            {
                StatusCode = StatusCodes.Status200OK

            };
        }

        public async Task<IActionResult> OnPostChangeReturnOrder(OrderResultRequest request)
        {
            var listEror = new List<object>();
            if (request.Id < 1)
            {
                var itemError = new
                {
                    name = "idREquest",
                    Content = "Thiếu  thông tin đơn hàng"
                };
                listEror.Add(itemError);
            }
            //if (!request.Result.HasValue)
            //{
            //    var itemError = new
            //    {
            //        name = "result",
            //        Content = "Thiếu thông tin kết quả"
            //    };
            //    listEror.Add(itemError);
            //}
            if (listEror.Count > 0)
            {
                var dataReponse2 = new
                {
                    success = false,

                };
                return new JsonResult(dataReponse2)
                {
                    StatusCode = StatusCodes.Status200OK

                };
            }
            var result = await _orderBussiness.ChangeReturnOrder(request);
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
