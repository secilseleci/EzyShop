using AutoMapper;
using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities.Concrete;
using Models.ViewModels;
using System.Linq.Expressions;

namespace WebUI.Controllers
{
    public class ApplicationController:BaseController
    {            
        private readonly ISellerApplicationService _sellerApplicationService;

        public ApplicationController(
        ISellerApplicationService sellerApplicationService,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper)
         : base(webHostEnvironment: webHostEnvironment, mapper: mapper)
        {
            _sellerApplicationService = sellerApplicationService;
        }

        #region Become Seller/ Create Application

        [HttpGet]
        public IActionResult BecomeSeller()
        {
            return View(new BecomeSellerViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> BecomeSeller(BecomeSellerViewModel model)
        {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var result = await _sellerApplicationService.CreateSellerApplicationAsync(model);
                TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
          

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Read
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            // Tüm başvuruları çek
            var result = await _sellerApplicationService.GetAllSellerApplicationsAsync(a => true);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index", "Home");
            }

            return View(result.Data);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSellerApplications(string status = "all")
        {
            Expression<Func<SellerApplication, bool>> filter = status.ToLower() switch
            {
                "approved" => a => a.Status == ApplicationStatus.Approved,
                "rejected" => a => a.Status == ApplicationStatus.Rejected,
                "pending" => a => a.Status == ApplicationStatus.Pending,
                _ => a => true  
            };

            var result = await _sellerApplicationService.GetAllSellerApplicationsAsync(filter);

            if (!result.Success || result.Data == null || !result.Data.Any())
            {
                return PartialView("_SellerApplicationsTable", new List<SellerApplication>()); // Boş liste döndür
            }

            return PartialView("_SellerApplicationsTable", result.Data);
        }


        #endregion



        #region Act

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveSeller(Guid id)
        {
            var result = await _sellerApplicationService.ApproveSellerAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectSeller(Guid id)
        {
            var result = await _sellerApplicationService.RejectSellerAsync(id);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
            return RedirectToAction("SellerApplications");
        }

        #endregion
    }
}
