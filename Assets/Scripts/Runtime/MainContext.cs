public class MainContext {

    public bool isLoaded;
    public bool isTearDown;

    public readonly AssetsCoreContext assetsCoreContext;
    public readonly TemplateCoreContext templateCoreContext;

    public MainContext() {
        isLoaded = false;
        isTearDown = false;
        assetsCoreContext = new AssetsCoreContext();
        templateCoreContext = new TemplateCoreContext();
    }

}