using System;
using System.Collections.Generic;

#nullable disable

namespace Autobarn.Data.Entities {
	public partial class Vehicle {
		public string Registration { get; set; }
		public string ModelCode { get; set; }
		public string Color { get; set; }
		public int? Year { get; set; }

		public virtual Model VehicleModel { get; set; }
	}
}
