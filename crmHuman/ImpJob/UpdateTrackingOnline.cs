using crmHuman.Model;
using Quartz;
using VS.Human.Business;
namespace crmHuman.ImpJob
{

    public class UpdateTrackingOnline : IJob
    {

        private readonly IGlobalDataBusiness globalDataBusiness;

        public UpdateTrackingOnline(IGlobalDataBusiness _globalDataBusiness)
        {
            globalDataBusiness = _globalDataBusiness;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            GlobalVar.GlobalData.TotalCaseSource = await globalDataBusiness.GetCountSource();
            GlobalVar.GlobalData.TotalCaseSourceCTV = await globalDataBusiness.GetCountSourceCTV();

        }
    }
}