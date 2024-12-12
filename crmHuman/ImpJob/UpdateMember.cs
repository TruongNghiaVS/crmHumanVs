using crmHuman.Model;
using Quartz;
using VS.Human.Business;
namespace crmHuman.ImpJob
{

    public class UpdateMember : IJob
    {

        private readonly IGroupBusiness globalDataBusiness;

        public UpdateMember(IGroupBusiness _globalDataBusiness)
        {
            globalDataBusiness = _globalDataBusiness;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var allgroup = await globalDataBusiness.GetAll(new VS.Human.Item.GroupRequest() { });

            if (allgroup == null || allgroup.Data == null)
            {
                return;
            }
            foreach (var group in allgroup.Data)
            {
                var itemgroup = group as dynamic;
                var item = new SelectDisplay()
                {
                    Code = itemgroup.Code,
                    Name = itemgroup.Name,
                    RelId = itemgroup.RelId,
                    LinkId = itemgroup.LinkId
                };

                GlobalGroupMember.GlobalData.AddGroup(item);

            }
            var allMembers = await globalDataBusiness.GetAllMember(-1);
            foreach (var member in allMembers.Data)
            {
                var itemgroup = member as dynamic;
                var item = new SelectDisplay()
                {
                    Code = itemgroup.Code,
                    Name = itemgroup.Name,
                    RelId = itemgroup.RelId,
                    LinkId = itemgroup.LinkId
                };
            }
        }
    }
}
