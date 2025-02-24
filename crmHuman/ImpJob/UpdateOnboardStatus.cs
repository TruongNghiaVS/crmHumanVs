using Quartz;
using VS.Human.Business;
namespace crmHuman.ImpJob
{

    public class UpdateOnboardStatus : IJob
    {
        private readonly IOnboardMemberBusiness globalDataBusiness;
        public UpdateOnboardStatus(IOnboardMemberBusiness _globalDataBusiness)
        {
            globalDataBusiness = _globalDataBusiness;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await globalDataBusiness.UpdateOnboardStatus();
        }
    }
}
