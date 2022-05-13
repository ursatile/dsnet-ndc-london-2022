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

            Field<ListGraphType<VehicleGraphType>>(
                "VehiclesByYear",
                "Return all vehicles registered in a particular year",
                new QueryArguments(MakeNonNullStringArgument("year", "The vehicles year of first registration")),
                resolve: GetVehiclesByYear);
        }

        private IEnumerable<Vehicle> GetVehicleByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }

        private Vehicle GetVehicle(IResolveFieldContext<object> context) {
            var reg = context.GetArgument<string>("registration");
            return db.FindVehicle(reg);
        }
        private IEnumerable<Vehicle> GetVehiclesByYear(IResolveFieldContext<object> context) {
            string[] yearQuery = context.GetArgument<string>("year").Split(" ");

            // Filter will be first part of array, year second and sorting third.
            string filter = yearQuery[0];
            int year = int.Parse(yearQuery[1]);
            char sort = char.Parse(yearQuery[2]);

            IEnumerable<Vehicle> vehicles = db.ListVehicles();

            IEnumerable<Vehicle> filteredVehicles = filter switch {
                "<" => vehicles.Where(v => v.Year < year),

                "<=" => vehicles.Where(v => v.Year <= year),

                ">" => vehicles.Where(v => v.Year > year),

                ">=" => vehicles.Where(v => v.Year >= year),

                _ => vehicles.Where(v => v.Year == year)
            };

            return sort switch {
                '+' => filteredVehicles.OrderBy(v => v.Year),

                '-' => filteredVehicles.OrderByDescending(v => v.Year),

                _ => filteredVehicles
            };
        }

        //private IEnumerable<Vehicle> GetVehiclesByYear(IResolveFieldContext<object> context) {
        //    var year = context.GetArgument<string>("year");

        //    if (year.StartsWith(">"))
        //        return db.ListVehicles().Where(v => v.Year > int.Parse(year.Substring(1, year.Length - 1)));
        //    else if (year.StartsWith("<"))
        //        return db.ListVehicles().Where(v => v.Year < int.Parse(year.Substring(1, year.Length - 1)));
        //    else
        //        return db.ListVehicles().Where(v => v.Year == int.Parse(year));
        //}

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
