using crmHuman.ImpJob;
using Microsoft.AspNetCore.Authentication.Cookies;
using Quartz;
using Quartz.Impl;
using VS.Human.Business;
namespace crmHuman
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.Config();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Home/Forbidden";
                options.LoginPath = "/Login";
            });

            builder.Services.AddQuartz(q =>
            {
                var jobKey = new JobKey("UpdateTrackingOnline");
                q.AddJob<UpdateTrackingOnline>(opts => opts.WithIdentity(jobKey));
                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("UpdateTrackingOnline-trigger")
                    .WithCronSchedule(" 0/3 * * * * ? *")
                );
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapRazorPages();

            app.Run();

            var scheduler = StdSchedulerFactory.GetDefaultScheduler();


            scheduler.Start();
        }
    }
}