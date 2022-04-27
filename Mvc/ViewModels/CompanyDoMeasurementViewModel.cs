using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SitefinityWebApp.Mvc.Models;

namespace SitefinityWebApp.MVC.ViewModels
{
    public class CompanyDoMeasurementViewModel
    {
        public CompanyModel Company { get; set; }
        public List<MeasurementModel> Measurements { get; set; }
    }
}