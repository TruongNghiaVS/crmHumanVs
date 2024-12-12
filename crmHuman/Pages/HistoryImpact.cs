using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VS.Human.Business;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace crmHuman.Pages
{
    [Authorize]
    public class HistoryImpactModel : BaseModel2
    {
        private readonly ILogger<HistoryImpactModel> _logger;
        private readonly IOrderBussiness _empBusiness;
        private readonly IReoportBussiness _reoportBussiness;
        private readonly IJobItemBusiness _jobItemBusiness;
        private readonly ICandidateBusiness _candidateBusiness;
        private readonly ImasterDataBussiness _masterDataBussiness;
        private readonly IPartnerBusiness _partnerBusiness;

        private readonly ImasterDataBussiness _imasterDataBussiness;

        public OrderRequest RequestSearch { get; set; }
        public BaseList DataAll { get; set; }

        public BaseList DataStatus { get; set; }


        public int TotalRecord
        {

            get
            {
                return DataAll.Total;

            }
        }
        public HistoryImpactModel(ILogger<HistoryImpactModel> logger,
            IOrderBussiness empBusiness,
            IJobItemBusiness jobItemBusiness,
            ICandidateBusiness candidateBusiness,
            IPartnerBusiness partnerBusiness,
            ImasterDataBussiness masterDataBussiness,
            IReoportBussiness reoportBussiness,
            ImasterDataBussiness imasterDataBussiness


            )
        {
            _logger = logger;
            _empBusiness = empBusiness;
            _jobItemBusiness = jobItemBusiness;
            TitlePage = "Danh sách tác động";
            KeyPage = "HistoryImpact";
            _candidateBusiness = candidateBusiness;
            _partnerBusiness = partnerBusiness;
            _masterDataBussiness = masterDataBussiness;
            _reoportBussiness = reoportBussiness;
            TableColumnText = new List<string>()
            {
                "STT","Mã","Người tạo","Trạng thái","Nội dung",
                "Ngày tạo"
            };
            _imasterDataBussiness = imasterDataBussiness;
        }




        public async Task<ActionResult> OnGet([FromQuery] OrderRequest request)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }
            GetInfoUser();
            if (UserData.RoleCode == "2" || UserData.RoleCode == "4")
            {
                this.Permision.SearchGrop = false;
            }
            return await GetAll(request);
        }

        public async Task<ActionResult> GetAll(OrderRequest request2)
        {
            RequestSearch = request2;
            request2.UserId = UserData.UserId;
            request2.Marketting = 0;

            request2.RoleCode = UserData.RoleCode;
            DataAll = await _reoportBussiness.GetAllImpact(request2);

            DataStatus = await _masterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 4,
                UserId = UserData.UserId
            });

            return Page();
        }

        public virtual async Task<PartialViewResult> OnGetFormEdit(int id)

        {
            var resultView = new Order()
            {
                Id = id
            };
            if (id > 0)
            {
                resultView = await _empBusiness.GetById(id);
            }
            var allJob = await _jobItemBusiness.GetAll(new JobRequest()
            {
            });
            var allCandidate = await _candidateBusiness.GetAll(new CandidateRequest()
            {
            });

            var AllStatus = await _masterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 4
            });

            var allLinhvuc = await _masterDataBussiness.GetAll(new CommonRequest()
            {
                Type = 2
            });

            var allPartner = await _partnerBusiness.GetAll(new CommonRequest());
            var resultObject = new
            {
                allJob,
                allCandidate,
                resultView,
                AllStatus,
                allLinhvuc,
                allPartner
            };
            return Partial("EditOrUpdateOrder", resultObject);
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




    }
}
