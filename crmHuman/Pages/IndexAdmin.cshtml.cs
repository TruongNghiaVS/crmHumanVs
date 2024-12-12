﻿using crmHuman.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using VS.Human.Business;
using VS.Human.Item;

namespace crmHuman.Pages
{
    [Authorize]
    public class IndexAdminModel : BaseModel2
    {
        private readonly ILogger<IndexAdminModel> _logger;

        private readonly IDashboardBusinness dashboardBusinness;

        private readonly ImasterDataBussiness _masterDataBusinness;
        private readonly IJobItemBusiness _jobItemBusiness;

        public BaseList TopOrder;
        public BaseList TopImpact;

        public BaseList ParamDashboard;
        public BaseList ReportGroupStatus;

        public BaseList StatusListOverviewDashboard { get; set; }

        public BaseList OrderList { get; set; }
        public dynamic InfoDashboard { get; set; }

        public dynamic RequestPage { get; set; }

        public BaseList JobList { get; set; }

        public BaseList StatusList { get; set; }

        public IndexAdminModel(
         ILogger<IndexAdminModel> logger,
        IDashboardBusinness dashboardBusinness,
        ImasterDataBussiness imasterDataBussiness,
        IJobItemBusiness jobItemBusiness
            )
        {
            _logger = logger;
            this.dashboardBusinness = dashboardBusinness;
            TopOrder = new BaseList();
            TopImpact = new BaseList();
            ReportGroupStatus = new BaseList();
            InfoDashboard = new { };
            _masterDataBusinness = imasterDataBussiness;
            _jobItemBusiness = jobItemBusiness;
        }

        public async Task<IActionResult> OnPostLogOut(LoginRequest request)
        {
            var authenticationScheme = HttpContext.User.FindFirstValue(ClaimTypes.AuthenticationMethod);
            if (authenticationScheme == null)
            {


            }
            await HttpContext.SignOutAsync(authenticationScheme);
            return Redirect("/login");

        }


        public async Task<IActionResult> OnPostCall(CallRequest request)
        {
            List<Claim> roleClaims = HttpContext.User.FindAll(ClaimTypes.Role).ToList();
            var userId = UserData.UserId;

            var data = new StringContent(JsonConvert.SerializeObject(new
            {
                phoneNumber = request.Phone,
                userId,
                lineCode = UserData.LineCode
            }));
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var linkUrl = "http://192.168.1.9:3002";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(linkUrl);
                var reponse = await client.PostAsync("api/client/makeCall", data);
                var result = await reponse.Content.ReadAsStringAsync();
            }
            var reponseResult = new
            {
                Success = true,
                Data = true


            };
            return new JsonResult(reponseResult) { StatusCode = StatusCodes.Status200OK };
        }

        public async Task<ActionResult> OnGet([FromQuery] OrderRequest request)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/Login");
            }


            var userId = UserData.UserId;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var roleCodeText = "";
            RequestPage = request;

            if (identity != null)
            {
                var userClaims = identity.Claims;
                var idUser = identity.Claims.FirstOrDefault(o => o.Type == "userId")?.Value;
                var userName = userClaims.FirstOrDefault(o => o.Type == "UserName")?.Value;
                var roleCode = userClaims.FirstOrDefault(o => o.Type == "RoleCode")?.Value;
                var fullName = userClaims.FirstOrDefault(o => o.Type == "FullName")?.Value;
                var lineCode = userClaims.FirstOrDefault(o => o.Type == "LineCode")?.Value;
                if (UserData == null)
                {
                    UserData = new UserDataView();
                }
                UserData.UserName = userName;
                UserData.FullName = fullName;
                UserData.UserId = int.Parse(idUser);
                UserData.RoleCode = roleCode;
                UserData.LineCode = lineCode;
                roleCodeText = roleCode;
                UserActive.DataActiveOnline.AddOrUpdate(idUser, userName, fullName);
            }

            RequestPage.UserId = UserData.UserId;

            RequestPage.RoleCode = UserData.RoleCode;




            if (RequestPage.RoleCode == "4")
            {

                this.Permision.SearchGrop = false;
            }
            if (UserData.RoleCode == "2" || UserData.RoleCode == "4")
            {
                this.Permision.SearchGrop = false;
            }
            if (UserData.RoleCode == "3")
            {

                if (UserData.UserId == 37)
                {
                    RequestPage.GroupId = 6;


                }
                else if (UserData.UserId == 38)
                {
                    RequestPage.GroupId = 7;

                }
            }

            var orderRequest = new OrderRequest()
            {
                UserId = UserData.UserId,
                Limit = 10,
                From = request.From,
                Status = request.Status,
                Job = request.Job,
                GroupId = request.GroupId,
                MemberId = request.MemberId,
                To = request.To
            };
            orderRequest.RoleCode = roleCodeText;
            var allOrderApply = await dashboardBusinness.GetAllOrderApply(orderRequest);
            var allOnboardCV = await dashboardBusinness.GetAllOnboardCV(orderRequest);
            var allOrderDraft = await dashboardBusinness.GetAllOrderDraft(orderRequest);
            var allOrderCV = await dashboardBusinness.GetALlOrderRemove(orderRequest);

            var allCountCVChartAllCV = await dashboardBusinness.GetAllCountCVGroupByDate(orderRequest);

            var allCountCVApply = await dashboardBusinness.GetAllCountCVApplyGroupByDate(orderRequest);

            var allCountCVOnboard = await dashboardBusinness.GetAllCountCVOnboardGroupByDate(orderRequest);
            double totalCVAply = 0;
            double totalCVUV = 0;
            double sumTotalCV = 0;
            double sumOnboard = 0;
            var listOrderDraft = new List<Object>();
            var listOrderApply = new List<Object>();

            double sumOnboardcV = 0;
            double SumcvPass = 0;
            foreach (var item in allOnboardCV.Data)
            {
                sumOnboardcV++;
                var iteminfo = item as dynamic;
                if (iteminfo.Result == 1)
                {
                    SumcvPass++;
                }


            }

            string rateCVPass = "0";

            if (sumOnboardcV > 0 && SumcvPass > 0)
            {
                rateCVPass = (SumcvPass / sumOnboardcV).ToString("0.00%");
            }
            foreach (var item in allOrderDraft.Data)
            {
                var iteminfo = item as dynamic;
                totalCVUV = iteminfo.TotalRecord;
                listOrderDraft.Add(iteminfo);
            }

            foreach (var item in allOrderApply.Data)
            {
                var iteminfo = item as dynamic;
                totalCVAply = iteminfo.TotalRecord;
                if (iteminfo.Result == 1)
                {
                    sumOnboard++;
                }
                listOrderApply.Add(iteminfo);
            }
            foreach (var item in allOrderCV.Data)
            {
                var iteminfo = item as dynamic;
                sumTotalCV = iteminfo.TotalRecord;



            }
            var listUser = UserActive.DataActiveOnline.GetListUser(UserData.UserId);

            string rateUV = "0";
            string rateOB = "0";
            if (totalCVAply > 0 && sumTotalCV > 0)
            {
                rateUV = (totalCVAply / sumTotalCV).ToString("0.00%");
            }

            if (totalCVAply > 0 && sumOnboard > 0 && sumTotalCV > 0)
            {
                rateOB = (sumOnboard / totalCVAply).ToString("0.00%");
            }
            InfoDashboard = new
            {

                rateOB,
                rateUV,
                totalCVUV,
                totalCVAply,
                sumTotalCV,
                sumOnboard,
                listOrderDraft,
                listOrderApply,
                allCountCVChartAllCV,
                allCountCVApply,
                listUser,
                allCountCVOnboard,
                rateCVPass,
                sumOnboardcV,
                SumcvPass,
                allOnboardCV
            };
            return Page();
        }


    }
}
