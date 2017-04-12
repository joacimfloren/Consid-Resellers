using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConsidResellers.Service_Layer.ServiceInterface;
using ConsidResellers.Models;
using ConsidResellers.Models.ViewModels;
using ConsidResellers.Controllers.Base;
using ConsidResellers.Helpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ConsidResellers.Controllers
{
    public class CompanyController : BaseController
    {
        private ICompanyService _companyService;

        public CompanyController(ICompanyService companyService, IConfiguration _config) : base(_config)
        {
            _companyService = companyService;
        }

        [Route("Companies/{page?}/{pageSize?}")]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            ServiceResponse<Dictionary<List<Company>, int>> serviceResponse = await _companyService.GetAllWithPages(page, pageSize);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }
            
            CompanyListViewModel viewModel = new CompanyListViewModel();

            Pager pager = new Pager(serviceResponse.Data.Values.First(), page, pageSize, _config);
            var companies = _mapper.Map<List<Company>, List<CompanyViewModel>>(serviceResponse.Data.Keys.First());

            viewModel.Pager = pager;
            viewModel.Companies = companies;

            return View(viewModel);
        }

        [Route("Company/Get/{id}/{page?}/{pageSize?}")]
        public async Task<IActionResult> Get(string id, int? page, int? pageSize)
        {
            ServiceResponse<Dictionary<Company, int>> serviceResponse = await _companyService.GetWithStores(id, page, pageSize);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            var companyViewModel = _mapper.Map<Company, CompanyViewModel>(serviceResponse.Data.Keys.First());

            Pager pager = new Pager(serviceResponse.Data.Values.First(), page, pageSize, _config);
            CompanyFullViewModel companyFullViewModel = new CompanyFullViewModel();
            companyFullViewModel.Company = companyViewModel;
            companyFullViewModel.StorePager = pager;
            
            return View("Company", companyFullViewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                var company = _mapper.Map<CompanyViewModel, Company>(companyViewModel);
                ServiceResponse<Company> serviceResponse = _companyService.Add(company);

                if (serviceResponse.ErrorOccured)
                {
                    ViewBag.Error = serviceResponse.Error;
                    return View("Error");
                }

                ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
                confirmationViewModel.Message = serviceResponse.Data.Name + " har lagts till.";
                confirmationViewModel.RouteController = "Companies";
                confirmationViewModel.RouteAction = "";
                return View("Success", confirmationViewModel);
            }

            return View();
        }

        public async Task<IActionResult> ConfirmRemoval(string id)
        {
            ServiceResponse<Company> serviceResponse = await _companyService.Get(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
            confirmationViewModel.Message = "Vill du ta bort " + serviceResponse.Data.Name + "? Om du tar bort ett företag tas också dess butiker bort.";
            confirmationViewModel.RouteController = "Company";
            confirmationViewModel.RouteAction = "Remove";
            ViewBag.Id = id;
            return View("Confirmation", confirmationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(string id)
        {
            ServiceResponse<Company> serviceResponse = await _companyService.Remove(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
            confirmationViewModel.Message = serviceResponse.Data.Name + " och dess eventuella butiker har tagits bort.";
            confirmationViewModel.RouteController = "Companies";
            confirmationViewModel.RouteAction = "";
            return View("Success", confirmationViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ServiceResponse<Company> serviceResponse = await _companyService.Get(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            CompanyViewModel companyViewModel = _mapper.Map<Company, CompanyViewModel>(serviceResponse.Data);
            return View(companyViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                ServiceResponse<Company> serviceResponse = await _companyService.Get(companyViewModel.Id);

                if (serviceResponse.ErrorOccured)
                {
                    ViewBag.Error = serviceResponse.Error;
                    return View("Error");
                }

                var company = _mapper.Map<CompanyViewModel, Company>(companyViewModel);
                serviceResponse = _companyService.Update(company);

                if (serviceResponse.ErrorOccured)
                {
                    ViewBag.Error = serviceResponse.Error;
                    return View("Error");
                }

                ConfirmationViewModel confirmViewModel = new ConfirmationViewModel();
                confirmViewModel.Message = serviceResponse.Data.Name + " har uppdaterats.";
                confirmViewModel.RouteController = "Companies";
                confirmViewModel.RouteAction = "";
                return View("Success", confirmViewModel);
            }

            return View();
        }
    }
}