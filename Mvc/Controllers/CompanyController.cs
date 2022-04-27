/* ------------------------------------------------------------------------------
<auto-generated>
    This file was generated by Sitefinity CLI v1.1.0.27
</auto-generated>
------------------------------------------------------------------------------ */

using SitefinityWebApp.Mvc.Models;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Personalization;

using Telerik.Sitefinity;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.DynamicModules;
using Telerik.Sitefinity.Data.Linq.Dynamic;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Telerik.Sitefinity.Utilities.TypeConverters;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Lifecycle;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Versioning;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using SitefinityWebApp.MVC.Controllers.Tools;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.RelatedData;

using Telerik.Sitefinity.Libraries.Model;
using System.Text.RegularExpressions;
using SitefinityWebApp.MVC.ViewModels;

namespace SitefinityWebApp.Mvc.Controllers
{
	[ControllerToolboxItem(Name = "Company_MVC", Title = "Company", SectionName = "CustomWidgets")]
	public class CompanyController : Controller, IPersonalizable
	{
        protected override void HandleUnknownAction(string actionName)
		{
			this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
		}

        // GET: Company
        public ActionResult Index()
		{
			List<CompanyModel> companyModel = new List<CompanyModel>();

			var companies = RetrieveCollectionOfCompanies().Where(p => p.Status == ContentLifecycleStatus.Live && p.Visible == true);

			foreach (var company in companies)
            {
                var logo = company.GetRelatedItems<Image>("Logo").FirstOrDefault();
                string urlLogo = (logo is null) ? "" : logo.MediaUrl;

                companyModel.Add(
					new CompanyModel 
					{
						Name = company.GetString("Name"),
						Email = company.GetString("Email"),
						Website = company.GetString("Website"),
                        urlLogo = urlLogo,
                        urlName = company.UrlName
					}
				);
            }

			return View("Index", companyModel);
		}

        [RelativeRoute("Detail/{urlName}")]
        // GET: Detail
        public ActionResult Detail(string urlName)
		{
            var companyModel = new CompanyDetailViewModel();
            var company = RetrieveCollectionOfCompanies().Where(p => p.Status == ContentLifecycleStatus.Live && p.Visible == true && p.UrlName == urlName).FirstOrDefault();

            var logo = company.GetRelatedItems<Image>("Logo").FirstOrDefault();
            string urlLogo = (logo is null) ? "" : logo.MediaUrl;

            companyModel.Name = company.GetString("Name");
            companyModel.Email = company.GetString("Email");
            companyModel.Website = company.GetString("Website");
            companyModel.urlLogo = urlLogo;
            companyModel.urlName = company.UrlName;

            decimal sumOfGradeCompanyMeasuremenResult = 0;
            int totalOfCompanyMeasurementResult = 0;
            bool hasMeasurementResult = false;
            bool hasCompleteMeasurement = false;

            // totalOfMeasurement
            var measurementController = new MeasurementController();
            var measurements = measurementController.RetrieveCollectionOfMeasurements().Where(p => p.Status == ContentLifecycleStatus.Live && p.Visible == true);
            int totalOfMeasurements = measurements.Count();

            // totalOfCompanyMeasurementResult
            totalOfCompanyMeasurementResult = company.GetChildItemsCount("Telerik.Sitefinity.DynamicTypes.Model.SimpleSIRI.MeasurementResult");

            if (totalOfCompanyMeasurementResult > 0) // has chlid item
            {
                hasMeasurementResult = true;

                if (totalOfCompanyMeasurementResult >= totalOfMeasurements)
                {
                    hasCompleteMeasurement = true; // if not complete (still false), I plan to able to fill measurement which not complete 
                }

                var measurementResults = company.GetChildItems("Telerik.Sitefinity.DynamicTypes.Model.SimpleSIRI.MeasurementResult");

                foreach (var measurementResult in measurementResults)
                {

                    sumOfGradeCompanyMeasuremenResult += measurementResult.GetValue<decimal>("Grade");
                }
            }

            companyModel.sumOfGradeCompanyMeasuremenResult = sumOfGradeCompanyMeasuremenResult;
            companyModel.totalOfCompanyMeasurementResult = totalOfCompanyMeasurementResult;
            companyModel.totalOfMeasurements = totalOfMeasurements;
            companyModel.hasMeasurementResult = hasMeasurementResult;
            companyModel.hasCompleteMeasurement = hasCompleteMeasurement;

            return View("Detail", companyModel);
        }

        [RelativeRoute("Detail/{urlName}/do-measurement")]
        public ActionResult DoMeasurement(string urlName)
        {
            // Company
            var company = RetrieveCollectionOfCompanies().Where(p => p.Status == ContentLifecycleStatus.Live && p.Visible == true && p.UrlName == urlName).FirstOrDefault();

            var logo = company.GetRelatedItems<Image>("Logo").FirstOrDefault();
            string urlLogo = (logo is null) ? "" : logo.MediaUrl;

            var companyModel = new CompanyModel()
            {
                Name = company.GetString("Name"),
                Email = company.GetString("Email"),
                Website = company.GetString("Website"),
                urlLogo = urlLogo,
                urlName = company.UrlName,
            };

            // Measurements
            var measurementController = new MeasurementController();
            var measurements = measurementController.RetrieveCollectionOfMeasurements().Where(p => p.Status == ContentLifecycleStatus.Live && p.Visible == true);

            List<MeasurementModel> measurementModel = new List<MeasurementModel>();
            
            foreach (var measurement in measurements)
            {
                measurementModel.Add(
                    new MeasurementModel
                    {
                        Name = measurement.GetString("Name"),
                        Detail = measurement.GetString("Detail"),
                        urlName = measurement.UrlName,
                    }
                );
            }

            // CompanyViewModel
            var companyViewModel = new CompanyViewModel()
            {
                Company = companyModel,
                Measurements = measurementModel,
            };

            return View("DoMeasurement", companyViewModel);
        }
        
        [RelativeRoute("StoreMeasurementResult")]
        [HttpPost] // must declared
        public ActionResult StoreMeasurementResult(CompanyViewModel companyViewModel)
        {
            var measurementResultController = new MeasurementResultController();

            for (int i = 0; i < companyViewModel.Measurements.Count; i++) {

                measurementResultController.store(
                    companyViewModel.Measurements[i].Grade,
                    companyViewModel.Measurements[i].Summary,
                    companyViewModel.Measurements[i].urlName,
                    companyViewModel.Company.urlName);
            }

            return RedirectToAction("Index");
        }

        [RelativeRoute("Create")]
        public ActionResult Create()
		{
			return View("Create");
		}

        [RelativeRoute("Store")]
        [HttpPost] // must declared
        public ActionResult Store(CompanyModel company)
        {
            // Set the provider name for the DynamicModuleManager here. All available providers are listed in
            // Administration -> Settings -> Advanced -> DynamicModules -> Providers
            var providerName = String.Empty;

            // Set a transaction name and get the version manager
            var transactionName = Guid.NewGuid().ToString();
            var versionManager = VersionManager.GetManager(null, transactionName);

            // Set the culture name for the multilingual fields
            var cultureName = "en";
            Telerik.Sitefinity.Services.SystemManager.CurrentContext.Culture = new CultureInfo(cultureName);

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName, transactionName);
            Type companyType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.SimpleSIRI.Company");
            DynamicContent companyItem = dynamicModuleManager.CreateDataItem(companyType);

            // This is how values for the properties are set
            companyItem.SetString("Name", company.Name, cultureName);
            companyItem.SetString("Email", company.Email, cultureName);
            companyItem.SetString("Website", company.Website, cultureName);

            // Get related item manager
            LibrariesManager logoManager = LibrariesManager.GetManager();
            var logoItem = logoManager.GetImages().FirstOrDefault(i => i.Status == ContentLifecycleStatus.Master);
            // -/ end Get related item manager

            if (logoItem != null)
            {
                // This is how we relate an item
                companyItem.CreateRelation(logoItem, "Logo");
            }

            companyItem.SetString("UrlName", new Lstring(Regex.Replace(company.Name.ToLower(), Slugify.UrlNameCharsToReplace, Slugify.UrlNameReplaceString)), cultureName);
            companyItem.SetValue("Owner", SecurityManager.GetCurrentUserId());
            companyItem.SetValue("PublicationDate", DateTime.UtcNow);


            companyItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Draft", new CultureInfo(cultureName));


            // Create a version and commit the transaction in order changes to be persisted to data store
            versionManager.CreateVersion(companyItem, false);

            // We can now call the following to publish the item
            ILifecycleDataItem publishedCompanyItem = dynamicModuleManager.Lifecycle.Publish(companyItem);

            // You need to set appropriate workflow status
            companyItem.SetWorkflowStatus(dynamicModuleManager.Provider.ApplicationName, "Published");

            // Create a version and commit the transaction in order changes to be persisted to data store
            versionManager.CreateVersion(companyItem, true);

            // Now the item is published and can be seen in the page //

            // Commit the transaction in order for the items to be actually persisted to data store
            TransactionManager.CommitTransaction(transactionName);

            return RedirectToAction("Index");
        }

        // Demonstrates how a collection of Companies can be retrieved
        public IQueryable<DynamicContent> RetrieveCollectionOfCompanies()
		{
			// Set the provider name for the DynamicModuleManager here. All available providers are listed in
			// Administration -> Settings -> Advanced -> DynamicModules -> Providers
			var providerName = String.Empty;

            // Set a transaction name
            var transactionName = Guid.NewGuid().ToString();

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName, transactionName);
			Type companyType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.SimpleSIRI.Company");
			
			// This is how we get the collection of Company items
			var myCollection = dynamicModuleManager.GetDataItems(companyType);
			// At this point myCollection contains the items from type companyType
			return myCollection;
        }
    }
}