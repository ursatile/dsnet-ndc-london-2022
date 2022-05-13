using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Types {
    public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(c => c.Registration);
            Field(c => c.Color);
            Field(c => c.Year);
            Field(c => c.VehicleModel,
                nullable: false,
                type: typeof(ModelGraphType)
            ).Description("The model of vehicle");
        }
    }

    public sealed class ModelGraphType : ObjectGraphType<Model> {
        public ModelGraphType() {
            Name = "Model";
            Field(m => m.Name);
            Field(m => m.Code);
            Field(m => m.Manufacturer,
                    nullable: false,
                    type: typeof(ManufacturerGraphType))
                .Description("Who manufactures this model of car?");
        }
    }

    public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
        public ManufacturerGraphType() {
            Name = "manufacturer";
            Field(m => m.Name);
            Field(m => m.Code);
        }
    }
}
