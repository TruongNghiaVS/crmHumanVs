namespace VS.Human.Rep
{
    public class UnitOfWork : IUnitOfWork
    {
        public IEmployeeRep EmployeeRep { get; set; }
        public IGroupRep GroupRep { get; set; }

        public IlocationRep LocationRep { get; set; }

        public IPartnerRep PartnerRep { get; set; }
        public IMasterDataRep MasterDataRep { get; set; }

        public IJobItemRep JobItemRep { get; set; }

        public ICandidateRep CandidateRep { get; set; }
        public IOrderRep OrderRep { get; set; }
        public IDashboardRep DashboardRep { get; set; }
        public IParrentChildRep ParrentChildRep { get; set; }
        public IOnboardMemberRep OnboardMemberRep { get; set; }
        public IGlobalDataRep GlobalDataRep { get; set; }

        public IReportTalkTimeRepository ReportTalkTimeRepository { get; set; }
        public IReportRepository ReportRepository { get; set; }
        public UnitOfWork(
            IEmployeeRep employeeRep,
            IGroupRep groupRep,
            IPartnerRep partnerRep,
            IMasterDataRep masterDataRep,
            IJobItemRep jobItemRep,
            ICandidateRep candidateRep,
            IOrderRep orderRep,
            IDashboardRep dashboardRep,
            IParrentChildRep parrentChildRep,
            IOnboardMemberRep onboardMemberRep,
            IGlobalDataRep globalDataRep,
            IlocationRep locationRep,
            IReportTalkTimeRepository reportTalkTimeRepository,
            IReportRepository _reportRepository
            )
        {
            this.EmployeeRep = employeeRep;
            this.GroupRep = groupRep;
            this.PartnerRep = partnerRep;
            this.MasterDataRep = masterDataRep;
            this.JobItemRep = jobItemRep;
            this.CandidateRep = candidateRep;
            this.OrderRep = orderRep;
            this.DashboardRep = dashboardRep;
            this.ParrentChildRep = parrentChildRep;
            this.OnboardMemberRep = onboardMemberRep;
            this.GlobalDataRep = globalDataRep;
            ReportTalkTimeRepository = reportTalkTimeRepository;
            LocationRep = locationRep;
            ReportRepository = _reportRepository;
        }
    }
}
