﻿
using Microsoft.Extensions.DependencyInjection;

namespace VS.Human.Rep
{
    public static class DependencyRegister
    {

        public static void ConfigRep(this IServiceCollection services)
        {
            // singleton
            services.AddSingleton<ILoginRep, LoginRep>();
            services.AddSingleton<IEmployeeRep, EmployeeRep>();
            services.AddSingleton<IGroupRep, GroupRep>();
            services.AddSingleton<IMasterDataRep, MasterDataRep>();
            services.AddSingleton<IJobItemRep, JobItemRep>();
            services.AddSingleton<ICandidateRep, CandidateRep>();
            services.AddSingleton<IOrderRep, OrderRep>();
            services.AddSingleton<IDashboardRep, DashboardRep>();
            services.AddSingleton<IParrentChildRep, ParrentChildRep>();
            services.AddSingleton<IOnboardMemberRep, OnboardMemberRep>();
            services.AddSingleton<IGlobalDataRep, GlobalDataRep>();
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IlocationRep, locationRep>();
            services.AddSingleton<IPartnerRep, PartnerRep>();
            services.AddSingleton<IReportTalkTimeGroupByDay, ReportTalkTimeGroupByDayRepository>();
            services.AddSingleton<IReportTalkTimeRepository, ReportTalkTimeRepository>();
            services.AddSingleton<IReportRepository, ReportRepository>();
            services.AddSingleton<IScheduleInterviewRep, ScheduleInterviewRep>();
            services.AddSingleton<IDocumentDataRep, DocumentDataRep>();
            
        }
    }
}
