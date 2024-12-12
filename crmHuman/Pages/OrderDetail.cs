using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VS.Human.Business;
using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace crmHuman.Pages
{
    [Authorize]
    public class OrderDetailModel : BaseModel2
    {
        private readonly ILogger<OrderDetailModel> _logger;
        private readonly IOrderBussiness _empBusiness;
        private readonly IJobItemBusiness _jobItemBusiness;
        private readonly ICandidateBusiness _candidateBusiness;
        private readonly IPartnerBusiness _partnerBusiness;

        private readonly IOnboardMemberBusiness _onboardMemberBusiness;

        private readonly ImasterDataBussiness _imasterDataBussiness;


        public OrderRequest RequestSearch { get; set; }
        public BaseList DataAll { get; set; }

        public OrderInfoIndexModel OrderInfo { get; set; }

        public BaseList AllJob { get; set; }
        public BaseList AllCandidate { get; set; }
        public BaseList AllPartner { get; set; }

        public BaseList AllImpact { get; set; }
        public BaseList AllStatus { get; set; }

        public BaseList AllLinhvuc { get; set; }

        public int TotalRecord
        {

            get
            {
                return DataAll.Total;

            }
        }
        public OrderDetailModel(ILogger<OrderDetailModel> logger,
            IOrderBussiness empBusiness,
            IJobItemBusiness jobItemBusiness,
            ICandidateBusiness candidateBusiness,
            IPartnerBusiness partnerBusiness,
            ImasterDataBussiness imasterDataBussiness,
            IOnboardMemberBusiness onboardMemberBusiness
            )
        {
            _logger = logger;
            _empBusiness = empBusiness;
            _jobItemBusiness = jobItemBusiness;
            TitlePage = "Danh sách chi tiết đơn hàng";
            KeyPage = "Order";
            _candidateBusiness = candidateBusiness;
            _partnerBusiness = partnerBusiness;
            _imasterDataBussiness = imasterDataBussiness;
            AllJob = new BaseList();
            AllCandidate = new BaseList();
            AllPartner = new BaseList();
            AllImpact = new BaseList();
            AllStatus = new BaseList();
            TableColumnText = new List<string>()
            {
                "STT","Mã","Ứng cử viên","Vị trí","Trạng thái","Ngày tạo","Cập nhật gần nhất","Thao tác"
            };
            _onboardMemberBusiness = onboardMemberBusiness;


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
            var resultView = new OrderInfoIndexModel()
            {
            };
            if (orderId > 0)
            {
                resultView = await _empBusiness.GetInfo(orderId);
            }

            if (resultView == null)
            {
                return Page();
            }
            OrderInfo = resultView;
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

            AllImpact = await _empBusiness.GetAllImpact(new OrderRequest()
            {
                OrderCode = resultView.Code,
                OrderId = resultView.Id

            });
            AllStatus = await _imasterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 4
            });

            AllLinhvuc = await _imasterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 2
            });
            return Page();
        }


    }
}
