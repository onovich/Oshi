using System;

namespace Alter.UI {

    public class UIEventCenter {

        // Login
        public Action Login_OnStartGameClickHandle;
        public void Login_OnStartGameClick() {
            Login_OnStartGameClickHandle?.Invoke();
        }

        public Action Login_OnExitGameClickHandle;
        public void Login_OnExitGameClick() {
            Login_OnExitGameClickHandle?.Invoke();
        }

        // GameOver
        public Action GameOver_OnRestartGameClickHandle;
        public void GameOver_OnRestartGameClick() {
            GameOver_OnRestartGameClickHandle?.Invoke();
        }

        public Action GameOver_OnExitGameClickHandle;
        public void GameOver_OnExitGameClick() {
            GameOver_OnExitGameClickHandle?.Invoke();
        }

        // Inventory
        public Action<int> Inventory_OnLeftClickTreasureHandle;
        public void Inventory_OnLeftClickTreasure(int index) {
            Inventory_OnLeftClickTreasureHandle?.Invoke(index);
        }

        public Action<int> Inventory_OnRightClickTreasureHandle;
        public void Inventory_OnRightClickTreasure(int index) {
            Inventory_OnRightClickTreasureHandle?.Invoke(index);
        }

        public Action Inventory_OnClickCloseHandle;
        public void Inventory_OnClickClose() {
            Inventory_OnClickCloseHandle?.Invoke();
        }

        // BluePrint
        public Action<int> BluePrint_OnClickChooseBluePrintHandle;
        public void BluePrint_OnClickChooseBluePrint(int index) {
            BluePrint_OnClickChooseBluePrintHandle?.Invoke(index);
        }

        public void Clear() {
            Login_OnStartGameClickHandle = null;
            Login_OnExitGameClickHandle = null;

            GameOver_OnRestartGameClickHandle = null;
            GameOver_OnExitGameClickHandle = null;

            Inventory_OnLeftClickTreasureHandle = null;
            Inventory_OnRightClickTreasureHandle = null;
            Inventory_OnClickCloseHandle = null;

            BluePrint_OnClickChooseBluePrintHandle = null;
        }


    }

}