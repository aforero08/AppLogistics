using AppLogistics.Components.Extensions.Native;
using AppLogistics.Data.Core;
using AppLogistics.Objects;
using AppLogistics.Objects.Models.Operation.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppLogistics.Services
{
    public class ServiceService : BaseService, IServiceService
    {
        public ServiceService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public TView Get<TView>(int id) where TView : ServiceView
        {
            var service = UnitOfWork.GetAs<Service, TView>(id);

            if (service == null)
            {
                return null;
            }

            var employees = UnitOfWork.Select<Holding>()
                .Where(h => h.ServiceId == id)
                .Select(h => h.EmployeeId)
                .ToList();
            service.SelectedEmployees = employees.ToArray();

            var novelties = UnitOfWork.Select<Novelty>()
                .Join(UnitOfWork.Select<ServiceNovelty>(), n => n.Id, sn => sn.NoveltyId, (n, sn) => new { Novelty = n, ServiceNovelty = sn })
                .Where(result => result.ServiceNovelty.ServiceId == id)
                .Select(result => result.ServiceNovelty.NoveltyId)
                .ToList();
            service.SelectedNovelties = novelties.ToArray();

            return service;
        }

        public TView GetEdit<TView>(int id) where TView : ServiceCreateEditView
        {
            var service = UnitOfWork.GetAs<Service, TView>(id);

            if (service == null)
            {
                return null;
            }

            service.SpecifyVehicleType = service.VehicleTypeId.HasValue;

            var employees = UnitOfWork.Select<Holding>()
                .Where(h => h.ServiceId == id)
                .Select(h => h.EmployeeId)
                .ToList();
            service.SelectedEmployees = employees.ToArray();

            var novelties = UnitOfWork.Select<Novelty>()
                .Join(UnitOfWork.Select<ServiceNovelty>(), n => n.Id, sn => sn.NoveltyId, (n, sn) => new { Novelty = n, ServiceNovelty = sn })
                .Where(result => result.ServiceNovelty.ServiceId == id)
                .Select(result => result.ServiceNovelty.NoveltyId)
                .ToList();
            service.SelectedNovelties = novelties.ToArray();

            return service;
        }

        public IQueryable<ServiceView> GetViews()
        {
            return UnitOfWork
                .Select<Service>()
                .To<ServiceView>()
                .OrderByDescending(service => service.Id);
        }

        public void Create(ServiceCreateEditView view)
        {
            var service = BuildService(view);

            UnitOfWork.Insert(service);
            UnitOfWork.Commit();
        }

        private Service BuildService(ServiceCreateEditView view)
        {
            var rate = UnitOfWork.Get<Rate>(view.RateId);
            var prices = CalculateServicePrices(rate, view);

            var service = UnitOfWork.To<Service>(view);
            service.Rate = rate;
            service.FullPrice = prices.FullPrice;
            service.HoldingPrice = prices.HoldingPrice;
            service.Holdings = GenerateHoldings(view, prices.PricePerEmployee);
            service.ServiceNovelties = GenerateServiceNovelties(view);

            return service;
        }

        private ServicePrices CalculateServicePrices(Rate rate, ServiceCreateEditView view)
        {
            var fullPrice = rate.Price * (decimal)view.Quantity * view.SelectedEmployees.Length;
            var holdingPrice = fullPrice * (decimal)rate.EmployeePercentage / 100;

            if (rate.SplitFare)
            {
                fullPrice = fullPrice / view.SelectedEmployees.Length;
                holdingPrice = fullPrice * (decimal)rate.EmployeePercentage / 100;
            }

            var pricePerEmployee = holdingPrice / view.SelectedEmployees.Length;

            return new ServicePrices
            {
                FullPrice = fullPrice,
                HoldingPrice = holdingPrice,
                PricePerEmployee = pricePerEmployee
            };
        }

        private IList<Holding> GenerateHoldings(ServiceCreateEditView view, decimal pricePerEmployee)
        {
            var holdings = new List<Holding>();
            foreach (var employeeId in view.SelectedEmployees)
            {
                var holding = new Holding
                {
                    Employee = UnitOfWork.Get<Employee>(employeeId),
                    Price = pricePerEmployee,
                };

                holdings.Add(holding);
            }

            return holdings;
        }

        private ICollection<ServiceNovelty> GenerateServiceNovelties(ServiceCreateEditView view)
        {
            var serviceNovelties = new List<ServiceNovelty>();

            if (view.SelectedNovelties == null)
            {
                return serviceNovelties;
            }

            foreach (var noveltyId in view.SelectedNovelties)
            {
                var serviceNovelty = new ServiceNovelty
                {
                    Novelty = UnitOfWork.Get<Novelty>(noveltyId)
                };
                serviceNovelties.Add(serviceNovelty);
            }

            return serviceNovelties;
        }

        public void Edit(ServiceCreateEditView view)
        {
            var existingService = UnitOfWork.Get<Service>(view.Id);
            var rate = UnitOfWork.Get<Rate>(view.RateId);
            var prices = CalculateServicePrices(rate, view);

            // Delete old holdings (for now, if the rate changes this is the way to always update the holdings)
            var existingHoldings = UnitOfWork.Select<Holding>().Where(h => h.ServiceId == view.Id);
            UnitOfWork.DeleteRange(existingHoldings);

            var updatedService = UnitOfWork.Map<ServiceCreateEditView, Service>(view, existingService);
            updatedService.FullPrice = prices.FullPrice;
            updatedService.HoldingPrice = prices.HoldingPrice;
            updatedService.ServiceNovelties = GetUpdatedServiceNovelties(existingService.ServiceNovelties, view);
            updatedService.Holdings = GenerateHoldings(view, prices.PricePerEmployee);
            updatedService.VehicleTypeId = view.VehicleTypeId;
            
            UnitOfWork.Update(updatedService);
            UnitOfWork.Commit();
        }

        private ICollection<ServiceNovelty> GetUpdatedServiceNovelties(ICollection<ServiceNovelty> existingNovelties, ServiceCreateEditView view)
        {
            if (existingNovelties == null && view.SelectedNovelties == null)
            {
                return null;
            }

            if (existingNovelties == null && view.SelectedNovelties != null)
            {
                return GenerateServiceNovelties(view);
            }

            if (existingNovelties != null && view.SelectedNovelties == null)
            {
                UnitOfWork.DeleteRange(UnitOfWork.Select<ServiceNovelty>().Where(sn => sn.ServiceId == view.Id));
                return null;
            }

            if (existingNovelties.Select(n => n.NoveltyId).OrderBy(n => n).SequenceEqual(view.SelectedNovelties.OrderBy(n => n)))
            {
                return existingNovelties;
            }

            UnitOfWork.DeleteRange(UnitOfWork.Select<ServiceNovelty>().Where(sn => sn.ServiceId == view.Id));
            return GenerateServiceNovelties(view);
        }

        public void Delete(int id)
        {
            var existingHoldings = UnitOfWork.Select<Holding>().Where(h => h.ServiceId == id);
            UnitOfWork.DeleteRange(existingHoldings);

            var existingServiceNovelties = UnitOfWork.Select<ServiceNovelty>().Where(sn => sn.ServiceId == id);
            UnitOfWork.DeleteRange(existingServiceNovelties);

            UnitOfWork.Delete<Service>(id);
            UnitOfWork.Commit();
        }

        public void Finalize(int id)
        {
            var service = UnitOfWork.Get<Service>(id);
            service.EndDate = DateTime.Now.UtcToDefaultTimeZone();
            UnitOfWork.Update(service);
            UnitOfWork.Commit();
        }
    }
}
