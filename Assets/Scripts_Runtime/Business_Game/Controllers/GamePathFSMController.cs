using Codice.CM.Client.Differences;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GamePathFSMController {

        public static void FixedTickFSM(GameBusinessContext ctx, PathModel path, float fixdt) {

            FixedTickFSM_Any(ctx, path, fixdt);

            PathFSMStatus status = path.FSM_GetStatus();
            if (status == PathFSMStatus.Idle) {
                FixedTickFSM_Idle(ctx, path, fixdt);
            } else if (status == PathFSMStatus.Moving) {
                FixedTickFSM_Moving(ctx, path, fixdt);
            } else {
                GLog.LogError($"GamePathFSMController.FixedTickFSM: unknown status: {status}");
            }

        }

        static void FixedTickFSM_Any(GameBusinessContext ctx, PathModel path, float fixdt) {

        }

        static void FixedTickFSM_Idle(GameBusinessContext ctx, PathModel path, float fixdt) {
            PathFSMComponent fsm = path.FSM_GetComponent();
            if (fsm.idle_isEntering) {
                fsm.idle_isEntering = false;
            }
        }

        static void FixedTickFSM_Moving(GameBusinessContext ctx, PathModel path, float fixdt) {
            PathFSMComponent fsm = path.FSM_GetComponent();
            if (fsm.moving_isEntering) {
                fsm.moving_isEntering = false;
            }
            path.Tick_MoveCarToNext(fixdt, () => {
                path.PushIndexToNext();
                fsm.Idle_Enter();
            });
        }

    }

}