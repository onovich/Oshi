using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Chouten {

    public class InputEntity {

        public Vector2 skillAxis;

        InputKeybindingComponent keybindingCom;

        public void Ctor() {
            keybindingCom.Ctor();
        }

        public void ProcessInput(Camera camera, float dt) {

            if (keybindingCom.IsKeyDown(InputKeyEnum.MoveLeft)) {
                skillAxis.x = -1;
            }
            if (keybindingCom.IsKeyDown(InputKeyEnum.MoveRight)) {
                skillAxis.x = 1;
            }
        }

        public void Keybinding_Set(InputKeyEnum key, KeyCode[] keyCodes) {
            keybindingCom.Bind(key, keyCodes);
        }

        public void Reset() {
            skillAxis = Vector2.zero;
        }

    }

}