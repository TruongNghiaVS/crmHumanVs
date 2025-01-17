namespace VS.Human.Rep
{
    public interface IUnitOfWork
    {
        public IOnboardMemberRep OnboardMemberRep { get; set; }
        public IEmployeeRep EmployeeRep { get; set; }

        public IGroupRep GroupRep { get; set; }

        public IPartnerRep PartnerRep { get; set; }

        public IlocationRep LocationRep { get; set; }

        public IMasterDataRep MasterDataRep { get; set; }

        public IJobItemRep JobItemRep { get; set; }

        public ICandidateRep CandidateRep { get; set; }
        public IOrderRep OrderRep { get; set; }
        public IDashboardRep DashboardRep { get; set; }
        public IGlobalDataRep GlobalDataRep { get; set; }
        public IParrentChildRep ParrentChildRep { get; set; }
        public IReportTalkTimeRepository ReportTalkTimeRepository { get; set; }
        public IReportRepository ReportRepository { get; set; }

    }
}
