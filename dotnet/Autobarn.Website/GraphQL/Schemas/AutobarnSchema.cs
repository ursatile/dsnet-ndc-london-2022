using Autobarn.Data;
using Autobarn.Website.GraphQL.Queries;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Schemas {
	public class AutobarnSchema : Schema {
		public AutobarnSchema(IAutobarnDatabase db) => Query = new VehicleQuery(db);
	}
}