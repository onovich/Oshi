using System;

namespace Alter {

    public class UIEventCenter {

        // Login
        public Action Login_OnClickLoginHandle;
        public void Login_Login() {
            Login_OnClickLoginHandle?.Invoke();
        }

    }

}