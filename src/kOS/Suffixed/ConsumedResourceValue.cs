using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;

namespace kOS.Suffixed
{
    [kOS.Safe.Utilities.KOSNomenclature("ConsumedResource")]
    public class ConsumedResourceValue : Structure
    {
        private readonly string name;
        protected readonly SharedObjects shared;
        private readonly float density;
        private readonly ModuleEngines engine;
        private readonly Propellant propellant;

        public ConsumedResourceValue(ModuleEngines engine, Propellant prop, SharedObjects shared)
        {
            this.shared = shared;
            name = prop.displayName;
            density = prop.resourceDef.density;
            this.engine = engine;
            propellant = prop;
            InitializeConsumedResourceSuffixes();
        }

        private void InitializeConsumedResourceSuffixes()
        {
            AddSuffix("NAME", new Suffix<StringValue>(() => name, "The name of the resource (eg LiguidFuel, ElectricCharge)"));
            AddSuffix("DENSITY", new Suffix<ScalarValue>(() => density, "The density of the resource"));
            AddSuffix("FUELFLOW", new Suffix<ScalarValue>(() => propellant.currentAmount / Time.fixedDeltaTime, "The current volumetric flow rate of the resource"));
            AddSuffix("MAXFUELFLOW", new Suffix<ScalarValue>(() => engine ? engine.getMaxFuelFlow(propellant) : 0, "The maximum volumetric flow rate of the resource"));
            AddSuffix("REQUIREDFLOW", new Suffix<ScalarValue>(() => propellant.currentRequirement / Time.fixedDeltaTime, "The required volumetric flow rate of the resource"));
            AddSuffix("MASSFLOW", new Suffix<ScalarValue>(() => propellant.currentAmount * density / Time.fixedDeltaTime, "The current mass flow rate of the resource"));
            AddSuffix("MAXMASSFLOW", new Suffix<ScalarValue>(() => engine ? engine.getMaxFuelFlow(propellant) * density : 0, "The maximum mass flow rate of the resource"));
            AddSuffix("RATIO", new Suffix<ScalarValue>(() => propellant.ratio, "The volumetric flow ratio of the resource"));
            AddSuffix("AMOUNT", new Suffix<ScalarValue>(() => propellant.actualTotalAvailable, "The resources currently available"));
            AddSuffix("CAPACITY", new Suffix<ScalarValue>(() => propellant.totalResourceCapacity, "The total storage capacity currently available"));
        }

        public override string ToString()
        {
            return string.Format("CONSUMEDRESOURCE({0})", name);
        }
    }
}