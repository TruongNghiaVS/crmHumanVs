using Microsoft.AspNetCore.Http;
using VS.Human.Item;
using VS.Human.Rep;

namespace VS.Human.Business.Imp
{
    public class ReportBussiness : BaseBusiness, IReoportBussiness
    {
        public ReportBussiness(IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
            : base(unitOfWork, httpContextAccessor)
        {

        }

        public async Task<BaseList> GetAllImpact(OrderRequest request)
        {
            return await _unitOfWork.OrderRep.GetAllImpactReport(request);
        }

        public async Task<BaseList> GetAllOrderStatus(OrderRequest request)
        {
            return await _unitOfWork.OrderRep.GetAllOrderStatus(request);
        }


    }
}
