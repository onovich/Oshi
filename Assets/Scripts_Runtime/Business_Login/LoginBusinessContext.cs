namespace Oshi{

    public class LoginBusinessContext {

        public LoginEventCenter evt;
        public UIAppContext uiContext;

        public TemplateInfraContext templateInfraContext;
        public SoundAppContext soundContext;

        public LoginBusinessContext() {
            evt = new LoginEventCenter();
        }

    }

}