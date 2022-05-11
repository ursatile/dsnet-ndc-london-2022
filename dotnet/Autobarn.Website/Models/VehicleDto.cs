using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Autobarn.Website.Models {
	public class VehicleDto {

		public string ModelName { get; set; }

		[HiddenInput(DisplayValue = false)]
		public string ModelCode { get; set; }

		private string registration;

		private static string NormalizeRegistration(string reg) {
			return reg == null ? reg : Regex.Replace(reg.ToUpperInvariant(), "[^A-Z0-9]", "");
		}

		[Required]
		[DisplayName("Registration Plate")]
		public string Registration {
			get => NormalizeRegistration(registration);
			set => registration = value;
		}

		[Required]
		[DisplayName("Year of first registration")]
		[Range(1950, 2021)]
		public int Year { get; set; }

		[Required]
		[DisplayName("Colour")]
		public string Color { get; set; }

		private static string[] colors = new[] {
			"Black", "Blue", "Gold", "Green", "Grey", "Orange",
			"Purple", "Red", "Silver", "Turquoise", "White", "Yellow"
		};

		private static readonly SelectListItem blankSelectListItem = new("select...", String.Empty);
		public static IEnumerable<SelectListItem> ListColors(string selectedColor) {
			var items = new List<SelectListItem> { blankSelectListItem };
			items.AddRange(colors.Select(c => new SelectListItem(c, c, c == selectedColor)));
			return items;
		}
	}
}

