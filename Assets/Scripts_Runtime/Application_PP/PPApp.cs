using UnityEngine;

namespace Oshi {

    public static class PPApp {

        public static void ColorAdjustMents_SetColor(PPAppContext ctx, Color color) {
            var volume = ctx.volume;
            var colorAdjustments = ctx.colorAdjustments;
            colorAdjustments.colorFilter.value = color;
        }

    }

}