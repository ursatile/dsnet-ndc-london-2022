using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers {
    public class Hal {
        public static dynamic Paginate(string url, int index, int count, int total) {
            dynamic links = new ExpandoObject();
            links.self = new { href = url };
            links.final = new { href = $"{url}?index={total - (total % count)}&count={count}" };
            links.first = new { href = $"{url}?index=0&count={count}" };
            if (index > 0) links.previous = new { href = $"{url}?index={index - count}&count={count}" };
            if (index + count < total) links.next = new { href = $"{url}?index={index + count}&count={count}" };
            return links;
        }
    }

    public static class HypermediaExtensions {

        public static dynamic ToResource(this Vehicle vehicle) {
            var result = vehicle.ToDynamic();
            result._links = new {
                self = new {
                    href = $"/api/vehicles/{vehicle.Registration}"
                },
                model = new {
                    href = $"/api/models/{vehicle.ModelCode}"
                }
            };
            return result;
        }

        public static dynamic ToDynamic(this object obj) {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(obj.GetType());
            foreach (PropertyDescriptor property in properties) {
                if (Ignore(property)) continue;
                expando.Add(property.Name, property.GetValue(obj));
            }
            return expando;
        }

        private static bool Ignore(PropertyDescriptor property) {
            var attributes = property.Attributes;
            var ignoreAttributes = attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>();
            return ignoreAttributes.Any();
        }
    }
}

