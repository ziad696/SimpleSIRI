using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitefinityWebApp.MVC.ViewModels
{
    public class CompanyDetailViewModel
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Website { get; set; }
		public string urlName { get; set; }
		public string urlLogo { get; set; }
		public decimal sumOfGradeCompanyMeasuremenResult { get; set; } // perlu untuk menampilkan jumlah grade yang dimiliki company
		public int totalOfCompanyMeasurementResult { get; set; }  // perlu untuk menampilkan berapa banyak measurement result yg dimiliki company 
        public int totalOfMeasurements { get; set; }
        public bool hasMeasurementResult { get; set; }
		public bool hasCompleteMeasurement { get; set; }
	}
}