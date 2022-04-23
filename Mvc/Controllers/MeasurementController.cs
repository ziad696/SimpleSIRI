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

namespace SitefinityWebApp.Mvc.Controllers
{
	[ControllerToolboxItem(Name = "Measurement_MVC", Title = "Measurement", SectionName = "CustomWidgets")]
	public class MeasurementController : Controller, IPersonalizable
	{
		protected override void HandleUnknownAction(string actionName)
		{
			this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
		}

		// GET: Measurement
		public ActionResult Index()
		{
			List<MeasurementModel> measurementModel = new List<MeasurementModel>();

			var measurements = RetrieveCollectionOfMeasurements().Where(p => p.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live && p.Visible == true);

			foreach (var company in measurements)
			{
				measurementModel.Add(
					new MeasurementModel
					{
						Name = company.GetString("Name"),
						Detail = company.GetString("Detail"),
					}
				);
			}

			return View("Index", measurementModel);
		}

        // Demonstrates how a collection of Measurements can be retrieved
        public IQueryable<DynamicContent> RetrieveCollectionOfMeasurements()
        {
            // Set the provider name for the DynamicModuleManager here. All available providers are listed in
            // Administration -> Settings -> Advanced -> DynamicModules -> Providers
            var providerName = String.Empty;

            // Set a transaction name
            var transactionName = Guid.NewGuid().ToString();

            DynamicModuleManager dynamicModuleManager = DynamicModuleManager.GetManager(providerName, transactionName);
            Type measurementType = TypeResolutionService.ResolveType("Telerik.Sitefinity.DynamicTypes.Model.SimpleSIRI.Measurement");
            
            // This is how we get the collection of Measurement items
            var myCollection = dynamicModuleManager.GetDataItems(measurementType);
            // At this point myCollection contains the items from type measurementType
            return myCollection;
        }
	}
}