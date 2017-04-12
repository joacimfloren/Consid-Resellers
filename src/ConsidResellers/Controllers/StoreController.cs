using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ConsidResellers.Service_Layer.ServiceInterface;
using ConsidResellers.Models;
using ConsidResellers.Controllers.Base;
using ConsidResellers.Models.ViewModels;
using ConsidResellers.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ConsidResellers.Controllers
{
    public class StoreController : BaseController
    {
        private IStoreService _storeService;
        private ICompanyService _companyService;

        public StoreController(IStoreService storeService, ICompanyService companyService, IConfiguration _config) : base(_config)
        {
            _storeService = storeService;
            _companyService = companyService;
        }

        [Route("Stores/{page?}/{pageSize?}")]
        public async Task<IActionResult> Index(int? page, int? pageSize)
        {
            ServiceResponse<Dictionary<List<Store>, int>> serviceResponse = await _storeService.GetAllWithPages(page, pageSize);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            StoreListViewModel viewModel = new StoreListViewModel();

            Pager pager = new Pager(serviceResponse.Data.Values.First(), page, pageSize, _config);
            var stores = _mapper.Map<List<Store>, List<StoreViewModel>>(serviceResponse.Data.Keys.First());

            viewModel.Pager = pager;
            viewModel.Stores = stores;

            return View(viewModel);
        }

        [Route("Store/Get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            ServiceResponse<Store> serviceResponse = await _storeService.Get(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            var storeViewModel = _mapper.Map<Store, StoreViewModel>(serviceResponse.Data);
            return View("Store", storeViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ServiceResponse<List<Company>> serviceResponse = await _companyService.GetAll();
            List<SelectListItem> selectList = new List<SelectListItem>();

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            StoreAddViewModel storeAddViewModel = new StoreAddViewModel();
            storeAddViewModel.CompanyList = GetCompanyListItem(serviceResponse.Data);
            storeAddViewModel.Store = new StoreViewModel();

            return View(storeAddViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(StoreAddViewModel storeAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                ServiceResponse<List<Company>> serviceResponse = await _companyService.GetAll();

                if (serviceResponse.ErrorOccured)
                {
                    ViewBag.Error = serviceResponse.Error;
                    return View("Error");
                }

                storeAddViewModel.CompanyList = GetCompanyListItem(serviceResponse.Data);
                return View(storeAddViewModel);
            }

            if (storeAddViewModel.Store.Longitude != null)
                storeAddViewModel.Store.Longitude = storeAddViewModel.Store.Longitude.Replace(',', '.');
            if (storeAddViewModel.Store.Latitude != null)
                storeAddViewModel.Store.Latitude = storeAddViewModel.Store.Latitude.Replace(',', '.');
            var store = _mapper.Map<StoreViewModel, Store>(storeAddViewModel.Store);
            ServiceResponse<Store> storeResponse = _storeService.Add(store);

            if (storeResponse.ErrorOccured)
            {
                ViewBag.Error = storeResponse.Error;
                return View("Error");
            }

            ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
            confirmationViewModel.Message = storeResponse.Data.Name + " har lagts till.";
            confirmationViewModel.RouteController = "Stores";
            confirmationViewModel.RouteAction = "";
            return View("Success", confirmationViewModel);
        }

        public async Task<IActionResult> ConfirmRemoval(string id)
        {
            ServiceResponse<Store> serviceResponse = await _storeService.Get(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
            confirmationViewModel.Message = "Vill du ta bort " + serviceResponse.Data.Name + "? När du tagit bort en butik går det inte att få tillbaka den.";
            confirmationViewModel.RouteController = "Store";
            confirmationViewModel.RouteAction = "Remove";
            ViewBag.Id = id;
            return View("Confirmation", confirmationViewModel);
        }

        public async Task<IActionResult> Remove(string id)
        {
            ServiceResponse<Store> serviceResponse = await _storeService.Remove(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
            confirmationViewModel.Message = serviceResponse.Data.Name + " har tagits bort.";
            confirmationViewModel.RouteController = "Stores";
            confirmationViewModel.RouteAction = "";
            return View("Success", confirmationViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ServiceResponse<Store> serviceResponse = await _storeService.Get(id);

            if (serviceResponse.ErrorOccured)
            {
                ViewBag.Error = serviceResponse.Error;
                return View("Error");
            }

            ServiceResponse<List<Company>> companiesReponse = await _companyService.GetAll();
            
            if (companiesReponse.ErrorOccured)
            {
                ViewBag.Error = companiesReponse.Error;
                return View("Error");
            }

            var store = _mapper.Map<Store, StoreViewModel>(serviceResponse.Data);
            StoreAddViewModel storeAddViewModel = new StoreAddViewModel();
            storeAddViewModel.Store = store;
            storeAddViewModel.CompanyList = GetCompanyListItem(companiesReponse.Data);

            return View(storeAddViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StoreAddViewModel storeAddViewModel)
        {
            if (!ModelState.IsValid)
            {
                ServiceResponse<List<Company>> serviceResponse = await _companyService.GetAll();

                if (serviceResponse.ErrorOccured)
                {
                    ViewBag.Error = serviceResponse.Error;
                    return View("Error");
                }

                storeAddViewModel.CompanyList = GetCompanyListItem(serviceResponse.Data);
                return View(storeAddViewModel);
            }

            if (storeAddViewModel.Store.Longitude != null)
                storeAddViewModel.Store.Longitude = storeAddViewModel.Store.Longitude.Replace(',', '.');
            if (storeAddViewModel.Store.Latitude != null)
                storeAddViewModel.Store.Latitude = storeAddViewModel.Store.Latitude.Replace(',', '.');
            var store = _mapper.Map<StoreViewModel, Store>(storeAddViewModel.Store);
            ServiceResponse<Store> updateResponse = _storeService.Update(store);

            if (updateResponse.ErrorOccured)
            {
                ViewBag.Error = updateResponse.Error;
                return View("Error");
            }

            ConfirmationViewModel confirmationViewModel = new ConfirmationViewModel();
            confirmationViewModel.Message = updateResponse.Data.Name + " har uppdaterats.";
            confirmationViewModel.RouteController = "Stores";
            confirmationViewModel.RouteAction = "";
            return View("Success", confirmationViewModel);
        }

        private List<SelectListItem> GetCompanyListItem (List<Company> companyList)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            foreach (var company in companyList)
            {
                SelectListItem item = new SelectListItem { Text = company.Name, Value = company.Id.ToString() };
                selectList.Add(item);
            }

            return selectList;
        }
    }
}
