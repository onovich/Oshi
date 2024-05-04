using System;
using System.Threading.Tasks;
using TenonKit.Prism;
using UnityEngine;

namespace Oshi {

    public class VFXAppContext {

        // Core
        public VFXParticelCore vfxCore;

        public VFXAppContext(string label, Transform root) {
            vfxCore = new VFXParticelCore(label, root);
        }

    }

}