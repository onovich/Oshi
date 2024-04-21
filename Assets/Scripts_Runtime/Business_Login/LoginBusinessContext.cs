namespace Oshi{

    public class LoginBusinessContext {

        public LoginEventCenter evt;
        public UIAppContext uiContext;

        public LoginBusinessContext() {
            evt = new LoginEventCenter();
        }

    }

}