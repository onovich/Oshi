using UnityEngine;

namespace Oshi {

    public class LoginBusiness {

        public static void Enter(LoginBusinessContext ctx) {
            UIApp.Login_Open(ctx.uiContext);
            // BGM
            var soundTable = ctx.templateInfraContext.SoundTable_Get();
            SoundApp.BGM_PlayLoop(ctx.soundContext, soundTable.bgmLoop[0], 0, soundTable.bgmVolume[0], true);
        }

        public static void Tick(LoginBusinessContext ctx, float dt) {
        }

        public static void Exit(LoginBusinessContext ctx) {
            UIApp.Login_Close(ctx.uiContext);
        }

        public static void ExitApplication(LoginBusinessContext ctx) {
            Exit(ctx);
            Application.Quit();
            GLog.Log("Application.Quit");
        }

        public static void OnUILoginClick(LoginBusinessContext ctx) {
            ctx.evt.Login();
        }

    }

}