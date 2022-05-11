using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#nullable disable

namespace Autobarn.Data.Entities {
	public partial class Model {
		public Model() {
			Vehicles = new HashSet<Vehicle>();
		}

		public string Code { get; set; }
		public string ManufacturerCode { get; set; }
		public string Name { get; set; }

		public virtual Manufacturer Manufacturer { get; set; }
		[JsonIgnore]
		public virtual ICollection<Vehicle> Vehicles { get; set; }
	}
}
