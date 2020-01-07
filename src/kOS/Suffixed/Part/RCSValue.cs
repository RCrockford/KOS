using kOS.Safe;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using kOS.Safe.Exceptions;
using kOS.Safe.Utilities;
using kOS.Suffixed.PartModuleField;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace kOS.Suffixed.Part
{
    [kOS.Safe.Utilities.KOSNomenclature("RCS")]
    public class RCSValue : PartValue
    {
        private readonly ModuleRCS module;

        /// <summary>
        /// Do not call! VesselTarget.ConstructPart uses this, would use `friend VesselTarget` if this was C++!
        /// </summary>
        internal RCSValue(SharedObjects shared, global::Part part, PartValue parent, DecouplerValue decoupler, ModuleRCS module)
            : base(shared, part, parent, decoupler)
        {
            this.module = module;
            RegisterInitializer(RCSInitializeSuffixes);
        }
        private void RCSInitializeSuffixes()
        {
            AddSuffix("ENABLE", new NoArgsVoidSuffix(Enable));
            AddSuffix("DISABLE", new NoArgsVoidSuffix(Disable));
            AddSuffix("ENABLEYAW", new NoArgsVoidSuffix(EnableYaw));
            AddSuffix("DISABLEYAW", new NoArgsVoidSuffix(DisableYaw));
            AddSuffix("ENABLEPITCH", new NoArgsVoidSuffix(EnablePitch));
            AddSuffix("DISABLEPITCH", new NoArgsVoidSuffix(DisablePitch));
            AddSuffix("ENABLEROLL", new NoArgsVoidSuffix(EnableRoll));
            AddSuffix("DISABLEROLL", new NoArgsVoidSuffix(DisableRoll));
            AddSuffix("ENABLESTARBOARD", new NoArgsVoidSuffix(EnableX));
            AddSuffix("DISABLESTARBOARD", new NoArgsVoidSuffix(DisableX));
            AddSuffix("ENABLETOP", new NoArgsVoidSuffix(EnableY));
            AddSuffix("DISABLETOP", new NoArgsVoidSuffix(DisableY));
            AddSuffix("ENABLEFORE", new NoArgsVoidSuffix(EnableZ));
            AddSuffix("DISABLEFORE", new NoArgsVoidSuffix(DisableZ));
            AddSuffix("THRUSTLIMIT", new ClampSetSuffix<ScalarValue>(() => module.thrustPercentage, value => module.thrustPercentage = value, 0f, 100f, 0f, "thrust limit percentage for this rcs thruster"));
            AddSuffix("AVAILABLETHRUST", new Suffix<ScalarValue>(() => module.GetThrust(useThrustLimit: true)));
            AddSuffix("AVAILABLETHRUSTAT",  new OneArgsSuffix<ScalarValue, ScalarValue>((ScalarValue atmPressure) => module.GetThrust(atmPressure, useThrustLimit: true)));
            AddSuffix("MAXTHRUST", new Suffix<ScalarValue>(() => module.GetThrust()));
            AddSuffix("THRUST", new Suffix<ScalarValue>(() => module.thrusterPower));
            AddSuffix("FUELFLOW", new Suffix<ScalarValue>(() => module.maxFuelFlow));
            AddSuffix("ISP", new Suffix<ScalarValue>(() => module.GetIsp()));
            AddSuffix(new[] { "VISP", "VACUUMISP" }, new Suffix<ScalarValue>(() => module.GetIsp(0)));
            AddSuffix(new[] { "SLISP", "SEALEVELISP" }, new Suffix<ScalarValue>(() => module.GetIsp(1)));
            AddSuffix("FLAMEOUT", new Suffix<BooleanValue>(() => module.flameout));
            AddSuffix("ISPAT", new OneArgsSuffix<ScalarValue, ScalarValue>((ScalarValue atmPressure) => module.GetIsp(atmPressure)));
            AddSuffix("MAXTHRUSTAT", new OneArgsSuffix<ScalarValue, ScalarValue>((ScalarValue atmPressure) => module.GetThrust(atmPressure)));
        }

        public static ListValue PartsToList(IEnumerable<global::Part> parts, SharedObjects sharedObj)
        {
            var toReturn = new ListValue();
            var vessel = VesselTarget.CreateOrGetExisting(sharedObj);
            foreach (var part in parts)
            {
                foreach (var module in part.Modules)
                {
                    if (module is IEngineStatus)
                    {
                        toReturn.Add(vessel[part]);
                        // Only add each part once
                        break;
                    }
                }
            }
            return toReturn;
        }

        public void Enable()
        {
            ThrowIfNotCPUVessel();
            module.rcsEnabled = true;
        }
        public void Disable()
        {
            ThrowIfNotCPUVessel();
            module.rcsEnabled = false;
        }

        public void EnableYaw()
        {
            ThrowIfNotCPUVessel();
            module.enableYaw = true;
        }
        public void DisableYaw()
        {
            ThrowIfNotCPUVessel();
            module.enableYaw = false;
        }

        public void EnablePitch()
        {
            ThrowIfNotCPUVessel();
            module.enablePitch = true;
        }
        public void DisablePitch()
        {
            ThrowIfNotCPUVessel();
            module.enablePitch = false;
        }

        public void EnableRoll()
        {
            ThrowIfNotCPUVessel();
            module.enableRoll = true;
        }
        public void DisableRoll()
        {
            ThrowIfNotCPUVessel();
            module.enableRoll = false;
        }

        public void EnableX()
        {
            ThrowIfNotCPUVessel();
            module.enableX = true;
        }
        public void DisableX()
        {
            ThrowIfNotCPUVessel();
            module.enableX = false;
        }

        public void EnableY()
        {
            ThrowIfNotCPUVessel();
            module.enableY = true;
        }
        public void DisableY()
        {
            ThrowIfNotCPUVessel();
            module.enableY = false;
        }

        public void EnableZ()
        {
            ThrowIfNotCPUVessel();
            module.enableZ = true;
        }
        public void DisableZ()
        {
            ThrowIfNotCPUVessel();
            module.enableZ = false;
        }
    }

    public static class ModuleRCSExtensions
    {
        /// <summary>
        /// Get engine thrust
        /// </summary>
        /// <param name="rcs">The rcs thruster (can be null - returns zero in that case)</param>
        /// <param name="atmPressure">
        ///   Atmospheric pressure (defaults to pressure at current location if omitted/null,
        ///   1.0 means Earth/Kerbin sea level, 0.0 is vacuum)</param>
        /// <returns>The thrust</returns>
        public static float GetThrust(this ModuleRCS rcs, double? atmPressure = null, bool useThrustLimit = false)
        {
            if (rcs == null)
                return 0f;
            float throttle = 1.0f;
            if (useThrustLimit)
                throttle = throttle * rcs.thrustPercentage / 100.0f;
            // thrust is fuel flow rate times isp times g
            // Assume min fuel flow is 0 as it's not exposed.
            return (float)(rcs.maxFuelFlow * throttle * GetIsp(rcs, atmPressure) * rcs.G);
        }
        /// <summary>
        /// Get engine ISP
        /// </summary>
        /// <param name="rcs">The rcs thruster (can be null - returns zero in that case)</param>
        /// <param name="atmPressure">
        ///   Atmospheric pressure (defaults to pressure at current location if omitted/null,
        ///   1.0 means Earth/Kerbin sea level, 0.0 is vacuum)</param>
        /// <returns></returns>
        public static float GetIsp(this ModuleRCS rcs, double? atmPressure = null)
        {
            return rcs == null ? 0f : rcs.atmosphereCurve.Evaluate(Mathf.Max(0f, (float)(atmPressure ?? rcs.part.staticPressureAtm)));
        }
    }
}
