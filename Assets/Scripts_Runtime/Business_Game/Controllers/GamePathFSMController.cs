using System;
using Codice.CM.Client.Differences;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GamePathFSMController {

        public static void FixedTickFSM(GameBusinessContext ctx, PathModel path, float fixdt, out bool isEnd) {
            isEnd = false;
            FixedTickFSM_Any(ctx, path, fixdt);

            PathFSMStatus status = path.FSM_GetStatus();
            if (status == PathFSMStatus.Idle) {
                FixedTickFSM_Idle(ctx, path, fixdt);
            } else if (status == PathFSMStatus.Moving) {
                FixedTickFSM_Moving(ctx, path, fixdt, out isEnd);
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

        static void FixedTickFSM_Moving(GameBusinessContext ctx, PathModel path, float fixdt, out bool isEnd) {
            PathFSMComponent fsm = path.FSM_GetComponent();
            if (fsm.moving_isEntering) {
                fsm.moving_isEntering = false;
            }
            var allow = GamePathDomain.CheckTravelerMovable(ctx, path);
            if (!allow) {
                fsm.Idle_Enter();
                isEnd = true;
                return;
            }

            path.Tick_MoveCarToNext(fixdt, out isEnd);
            if (isEnd) {
                path.PushIndexToNext();
                fsm.Idle_Enter();
            }
        }

    }

}