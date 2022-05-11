using Newtonsoft.Json;
using System.Collections.Generic;

#nullable disable

namespace Autobarn.Data.Entities {
	public partial class Manufacturer {
		public Manufacturer() {
			Models = new HashSet<Model>();
		}

		public string Code { get; set; }
		public string Name { get; set; }

		[JsonIgnore]
		public virtual ICollection<Model> Models { get; set; }
	}
}
