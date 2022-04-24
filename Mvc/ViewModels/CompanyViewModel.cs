using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SitefinityWebApp.Mvc.Models;

namespace SitefinityWebApp.MVC.ViewModels
{
    public class CompanyViewModel
    {
        public CompanyModel Company { get; set; }
        public IEnumerable<MeasurementModel> Measurements { get; set; }
    }
}