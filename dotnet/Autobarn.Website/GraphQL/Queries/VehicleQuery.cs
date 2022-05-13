using System.Collections;
using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.Types;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
    public class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;
            Field<ListGraphType<VehicleGraphType>>(
                "Vehicles",
                "Return all the vehicles in the system",
                resolve: GetAllVehicles
            );
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) {
            return db.ListVehicles();

        }
    }
}
