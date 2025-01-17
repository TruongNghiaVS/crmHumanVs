using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VS.Human.Business;
using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace crmHuman.Pages
{
    [Authorize]
    public class OrderDetailNewModel : BaseModel2
    {
        private readonly ILogger<OrderDetailNewModel> _logger;
        private readonly IOrderBussiness _empBusiness;
        private readonly IJobItemBusiness _jobItemBusiness;
        private readonly ICandidateBusiness _candidateBusiness;
        private readonly IPartnerBusiness _partnerBusiness;

        private readonly IOnboardMemberBusiness _onboardMemberBusiness;

        private readonly ImasterDataBussiness _imasterDataBussiness;
        private readonly IReportCDRBussiness _reportCDRBussiness;


        public OrderRequest RequestSearch { get; set; }
        public BaseList DataAll { get; set; }

        public OrderInfoIndexModel OrderInfo { get; set; }
        public Candidate CandidateInfo { get; set; }
        public BaseList AllJob { get; set; }
        public BaseList allOrder { get; set; }

        public BaseList AllCandidate { get; set; }
        public BaseList AllPartner { get; set; }

        public BaseList AllImpact { get; set; }
        public BaseList AllStatus { get; set; }
        public BaseList AllTinh { get; set; }
        public BaseList AllLinhvuc { get; set; }
        public BaseList AllFileRecord { get; set; }
        public int TotalRecord
        {

            get
            {
                return DataAll.Total;

            }
        }
        public OrderDetailNewModel(ILogger<OrderDetailNewModel> logger,
            IOrderBussiness empBusiness,
            IJobItemBusiness jobItemBusiness,
            ICandidateBusiness candidateBusiness,
            IPartnerBusiness partnerBusiness,
            ImasterDataBussiness imasterDataBussiness,
            IOnboardMemberBusiness onboardMemberBusiness,
            IReportCDRBussiness reportCDRBussiness
            )
        {
            _logger = logger;
            _empBusiness = empBusiness;
            _jobItemBusiness = jobItemBusiness;
            TitlePage = "Chi tiết hồ sơ";
            KeyPage = "Order";
            _candidateBusiness = candidateBusiness;
            _partnerBusiness = partnerBusiness;
            _imasterDataBussiness = imasterDataBussiness;
            AllJob = new BaseList();
            AllCandidate = new BaseList();
            AllPartner = new BaseList();
            AllImpact = new BaseList();
            AllStatus = new BaseList();
            AllTinh = new BaseList();
            TableColumnText = new List<string>()
            {
                "STT","Mã","Ứng cử viên","Vị trí","Trạng thái","Ngày tạo","Cập nhật gần nhất","Thao tác"
            };
            _onboardMemberBusiness = onboardMemberBusiness;
            _reportCDRBussiness = reportCDRBussiness;
        }


        public async Task<IActionResult> OnpostAdd(OrderImpactHIstoryAdd request)
        {
            var listEror = new List<object>();

            if (request.NewStatus == 12)
            {
                if (!request.DateFrom.HasValue)
                {
                    var itemError = new
                    {

                        name = "dateFrom",
                        Content = "Thiếu thông tin ngày onboard"
                    };
                    listEror.Add(itemError);
                }

            }
            if (request.NewStatus < 0)
            {
                var itemError = new
                {

                    name = "cbSelectStatus",
                    Content = "Thiếu thông tin trạng thái"
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
            var result = await _empBusiness.AddImpact(request);


            var dataReponse = new
            {
                success = result,

            };


            if (request.NewStatus == 12)
            {
                var orderInfo = await _empBusiness.GetByCode(request.OrderCode);
                var resultCheckt = new OrderResultRequest()
                {
                    CreatedBy = UserData.UserId,
                    Result = 1,
                    Id = orderInfo.Id
                };
                await _empBusiness.OrderResultRequest(resultCheckt);

                if (orderInfo != null)
                {
                    var jobIno = await _jobItemBusiness.GetById(orderInfo.JobId.Value);
                    var WarrantyDate = 0;
                    if (jobIno.WarrantyDate.HasValue)
                    {
                        WarrantyDate = jobIno.WarrantyDate.Value;
                    }
                    var Warrantyday = request.DateFrom.Value.AddDays(WarrantyDate);
                    Warrantyday = new DateTime(Warrantyday.Year, Warrantyday.Month, Warrantyday.Day, 23, 59, 59);
                    var itemonboard = new OnboardMember()
                    {
                        Assignee = orderInfo.Assignee,
                        OnboardDate = request.DateFrom,
                        JobId = orderInfo.JobId,
                        PartnerId = orderInfo.PartnerId,
                        ProjectId = orderInfo.ProjectId,
                        Status = request.NewStatus,
                        CandidateId = orderInfo.CandidateId,
                        CVLink = orderInfo.CVLink,
                        Noted = request.Noted,
                        OrderCode = orderInfo.Code,
                        Code = "",
                        IsActive = 1,
                        CreateAt = DateTime.Now,
                        Deleted = false,
                        SystemStatus = request.NewStatus,
                        Source = orderInfo.Source,
                        ShortDes = orderInfo.ShortDes,
                        Dpd = WarrantyDate,
                        Warrantydate = Warrantyday
                    };
                    await _onboardMemberBusiness.AddOrUpdate(itemonboard);

                }

            }



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
            var orderId = request.OrderId;
            return await getInfo(orderId);
        }

        public async Task<ActionResult> getInfo(int orderId)
        {
            var resultView = new OrderInfoIndexModel();
            int? applyFor = 2;
            if (orderId > 0)
            {
                resultView = await _empBusiness.GetInfo(orderId);

            }
            else
            {
                resultView = new OrderInfoIndexModel()
                {
                    Id = -1,
                    Isapply = 0,
                    Gender = -1,
                    RankLevel = -1,
                    Regional = "-1",
                    Experience = -1
                };
            }

            if (resultView.Isapply.HasValue)
            {
                if (resultView.Isapply == 0)
                {
                    applyFor = 0;
                }
                else if (resultView.Isapply == 1)
                {
                    applyFor = 1;
                }
            }
            AllJob = await _jobItemBusiness.GetAll(new JobRequest()
            {
                UserId = UserData.UserId
            });
            AllCandidate = await _candidateBusiness.GetAll(new CandidateRequest()
            {
                RoleCode = UserData.RoleCode,
                UserId = UserData.UserId,
                LoadAll = 1
            });
            AllPartner = await _partnerBusiness.GetAll(new CommonRequest());
            AllStatus = await _imasterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 4,
                ApplyFor = applyFor
            });
            AllTinh = await _imasterDataBussiness.GetallRegional();
            AllLinhvuc = await _imasterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 2
            });
            if (CandidateInfo == null)
            {
                CandidateInfo = new Candidate
                {
                    Name = "",
                    Phone = "",
                    Email = "",
                    Dob = null

                };
            }
            if (resultView == null || resultView.CandidateId.HasValue == false)
            {
                OrderInfo = resultView;
                CandidateInfo = new Candidate
                {
                    Name = "",
                    Phone = "",
                    Email = "",
                    Dob = null,
                    Id = -1

                };
                allOrder = new BaseList();
                return Page();
            }
            var candidateInfoTemp = await _candidateBusiness.GetById(resultView.CandidateId.Value);
            if (candidateInfoTemp == null)
            {
                candidateInfoTemp = new Candidate
                {
                    Name = "",
                    Phone = "",
                    Email = "",
                    Dob = null

                };
            }
            else
            {
                var dataOrder = await _empBusiness
                .GetALlHistory(new OrderRequest()
                {
                    Phone = candidateInfoTemp.Phone,
                    Email = candidateInfoTemp.Email
                });
                allOrder = dataOrder;

            }
            CandidateInfo = candidateInfoTemp;
            OrderInfo = resultView;
            AllImpact = await _empBusiness.GetAllImpact(new OrderRequest()
            {
                OrderCode = resultView.Code,
                OrderId = resultView.Id

            });
            var result = await _reportCDRBussiness.GetAllRecordingByOrderId(resultView.Id);
            if (result == null)
            {

            }
            AllFileRecord = new BaseList() { Data = result.Data, Total = result.Total };
            return Page();
        }


    }
}
