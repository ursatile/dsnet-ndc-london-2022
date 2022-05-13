using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            Field<VehicleGraphType>("Vehicle", "Return a single vehicle",
                new QueryArguments(MakeNonNullStringArgument("registration",
                    "The registration plate of the car you want")),
                resolve: GetVehicle);

            Field<ListGraphType<VehicleGraphType>>(
                "VehiclesByColor",
                "Return all vehicles matching a particular color",
                new QueryArguments(MakeNonNullStringArgument("color",
                    "What color cars do you want?")),
                resolve: GetVehicleByColor);
        }

        private IEnumerable<Vehicle> GetVehicleByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var reg = context.GetArgument<string>("registration");
            return db.FindVehicle(reg);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) {
            return db.ListVehicles();

        }
    }
}
