using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Autobarn.Website.Controllers {
    public class Hal {
        public static dynamic Paginate(string url,char current, List<char> available) {
            dynamic links = new ExpandoObject();
            links.self = new { href = $"{url}?index={current}" };
            links.final = new { href = $"{url}?index={available.Last()}" };
            links.first = new { href = $"{url}?index={available.First()}" };
            if (current != available.First()) links.previous = new { href = $"{url}?index={available[available.IndexOf(current)-1]}" };
            if (current != available.Last()) links.next = new { href = $"{url}?index={available[available.IndexOf(current) + 1]}" };
            return links;
        }
    }
}
