using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Oshi {

    public class PPAppContext {

        public Volume volume;
        public ColorAdjustments colorAdjustments;

        public TemplateInfraContext templateInfraContext;

        public PPAppContext(Volume volume) {
            this.volume = volume;
            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        }

    }

}