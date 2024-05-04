using System;
using System.Threading.Tasks;
using TenonKit.Prism;
using UnityEngine;

namespace Oshi {

    public class VFXAppContext {

        // Core
        public VFXParticelCore vfxCore;

        // Infra
        public TemplateInfraContext templateInfraContext;

        public int weather_rain_vfx_id;

        public VFXAppContext(string label, Transform root) {
            vfxCore = new VFXParticelCore(label, root);
        }

    }

}