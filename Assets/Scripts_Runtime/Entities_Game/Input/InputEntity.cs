using UnityEngine;

namespace Alter {

    public class InputEntity {

        public Vector2 moveAxis;
        public bool RotateLeft;
        public bool RotateRight;

        InputKeybindingComponent keybindingCom;

        public void Ctor() {
            keybindingCom.Ctor();
        }

        public void ProcessInput(float dt) {

            // Move Axis
            if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveLeft)) {
                moveAxis.x = -1;
            } else if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveRight)) {
                moveAxis.x = 1;
            }

            if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveDown)) {
                moveAxis.y = -1;
            } else if (keybindingCom.IsKeyPressing(InputKeyEnum.MoveUp)) {
                moveAxis.y = 1;
            }

        }

        public void Keybinding_Set(InputKeyEnum key, KeyCode[] keyCodes) {
            keybindingCom.Bind(key, keyCodes);
        }

        public void Reset() {
            moveAxis = Vector2.zero;
        }

    }

}