using AppLogistics.Components.Lookups;
using AppLogistics.Components.Mvc;
using AppLogistics.Components.Security;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using Microsoft.AspNetCore.Mvc;
using NonFactors.Mvc.Lookup;

namespace AppLogistics.Controllers
{
    [AllowUnauthorized]
    public class LookupController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public LookupController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [NonAction]
        public virtual JsonResult GetData<TModel, TView>(MvcLookup<TModel, TView> lookup, LookupFilter filter)
            where TModel : BaseModel
            where TView : BaseView
        {
            lookup.Filter = filter;

            return Json(lookup.GetData());
        }

        [AjaxOnly]
        public JsonResult Activity(LookupFilter filter)
        {
            return GetData(new MvcLookup<Activity, ActivityView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Afp(LookupFilter filter)
        {
            return GetData(new MvcLookup<Afp, AfpView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult BranchOffice(LookupFilter filter)
        {
            return GetData(new MvcLookup<BranchOffice, BranchOfficeView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Carrier(LookupFilter filter)
        {
            return GetData(new MvcLookup<Carrier, CarrierView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Client(LookupFilter filter)
        {
            return GetData(new MvcLookup<Client, ClientView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Country(LookupFilter filter)
        {
            return GetData(new MvcLookup<Country, CountryView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult DocumentType(LookupFilter filter)
        {
            return GetData(new MvcLookup<DocumentType, DocumentTypeView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult EducationLevel(LookupFilter filter)
        {
            return GetData(new MvcLookup<EducationLevel, EducationLevelView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult EmployeeActive(LookupFilter filter)
        {
            filter.AdditionalFilters[nameof(Employee.Active)] = true;
            return GetData(new MvcLookup<Employee, EmployeeView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Eps(LookupFilter filter)
        {
            return GetData(new MvcLookup<Eps, EpsView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult EthnicGroup(LookupFilter filter)
        {
            return GetData(new MvcLookup<EthnicGroup, EthnicGroupView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult MaritalStatus(LookupFilter filter)
        {
            return GetData(new MvcLookup<MaritalStatus, MaritalStatusView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult NoveltiesInService(LookupFilter filter)
        {
            return GetData(new MvcLookup<Novelty, NoveltyView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Product(LookupFilter filter)
        {
            return GetData(new MvcLookup<Product, ProductView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult RatePerClient(LookupFilter filter, int rateClientId)
        {
            filter.AdditionalFilters[nameof(Rate.ClientId)] = rateClientId;
            return GetData(new MvcLookup<Rate, RateView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Role(LookupFilter filter)
        {
            return GetData(new MvcLookup<Role, RoleView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Sector(LookupFilter filter)
        {
            return GetData(new MvcLookup<Sector, SectorView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult Sex(LookupFilter filter)
        {
            return GetData(new MvcLookup<Sex, SexView>(_unitOfWork), filter);
        }

        [AjaxOnly]
        public JsonResult VehicleType(LookupFilter filter)
        {
            return GetData(new MvcLookup<VehicleType, VehicleTypeView>(_unitOfWork), filter);
        }

        protected override void Dispose(bool disposing)
        {
            _unitOfWork.Dispose();

            base.Dispose(disposing);
        }
    }
}
