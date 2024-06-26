using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace Oshi {

    public class Main : MonoBehaviour {

        [SerializeField] bool drawCameraGizmos;
        [SerializeField] bool drawMapGizmos;
        [SerializeField] bool isTestMode;
        [SerializeField] int testMapTypeID;

        InputEntity inputEntity;

        AssetsInfraContext assetsInfraContext;
        TemplateInfraContext templateInfraContext;
        DBInfraContext dBInfraContext;

        GameBusinessContext gameBusinessContext;

        UIAppContext uiAppContext;
        VFXAppContext vfxAppContext;
        CameraAppContext cameraAppContext;
        PPAppContext ppAppContext;
        SoundAppContext soundAppContext;

        bool isLoadedAssets;
        bool isTearDown;

        void Start() {

            isLoadedAssets = false;
            isTearDown = false;

            Canvas mainCanvas = GameObject.Find("MainCanvas").GetComponent<Canvas>();
            Transform hudFakeCanvas = GameObject.Find("HUDFakeCanvas").transform;
            Camera mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
            Volume mainVolume = GameObject.Find("MainVolume").GetComponent<Volume>();
            Transform vfxRoot = GameObject.Find("VFXRoot").transform;
            Transform soundRoot = GameObject.Find("SoundRoot").transform;

            inputEntity = new InputEntity();

            gameBusinessContext = new GameBusinessContext();

            uiAppContext = new UIAppContext("UI", mainCanvas, hudFakeCanvas, mainCamera);
            vfxAppContext = new VFXAppContext("VFX", vfxRoot);
            cameraAppContext = new CameraAppContext(mainCamera, new Vector2(Screen.width, Screen.height));
            ppAppContext = new PPAppContext(mainVolume);
            soundAppContext = new SoundAppContext(soundRoot);

            assetsInfraContext = new AssetsInfraContext();
            templateInfraContext = new TemplateInfraContext();
            dBInfraContext = new DBInfraContext();

            // Inject
            gameBusinessContext.inputEntity = inputEntity;
            gameBusinessContext.assetsInfraContext = assetsInfraContext;
            gameBusinessContext.templateInfraContext = templateInfraContext;
            gameBusinessContext.uiContext = uiAppContext;
            gameBusinessContext.vfxContext = vfxAppContext;
            gameBusinessContext.cameraContext = cameraAppContext;
            gameBusinessContext.ppAppContext = ppAppContext;
            gameBusinessContext.soundContext = soundAppContext;
            gameBusinessContext.mainCamera = mainCamera;
            gameBusinessContext.dbInfraContext = dBInfraContext;

            cameraAppContext.templateInfraContext = templateInfraContext;
            ppAppContext.templateInfraContext = templateInfraContext;
            vfxAppContext.templateInfraContext = templateInfraContext;

            // TODO Camera

            // Binding
            Binding();

            Action action = async () => {
                try {
                    await LoadAssets();
                    Init();
                    Enter();
                    isLoadedAssets = true;
                } catch (Exception e) {
                    GLog.LogError(e.ToString());
                }
            };
            action.Invoke();

        }

        void Enter() {
            GameBusiness.EnterLogin(gameBusinessContext);
        }

        void Update() {

            if (!isLoadedAssets) {
                return;
            }

            var dt = Time.deltaTime;
            GameBusiness.Tick(gameBusinessContext, dt);

            UIApp.LateTick(uiAppContext, dt);

        }

        void Init() {

            Application.targetFrameRate = 120;

            var inputEntity = this.inputEntity;
            inputEntity.Ctor();
            inputEntity.Keybinding_Set(InputKeyEnum.MoveLeft, new KeyCode[] { KeyCode.A, KeyCode.LeftArrow });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveRight, new KeyCode[] { KeyCode.D, KeyCode.RightArrow });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveUp, new KeyCode[] { KeyCode.W, KeyCode.UpArrow });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveDown, new KeyCode[] { KeyCode.S, KeyCode.DownArrow });
            inputEntity.Keybinding_Set(InputKeyEnum.Restart, new KeyCode[] { KeyCode.R });
            inputEntity.Keybinding_Set(InputKeyEnum.Exit, new KeyCode[] { KeyCode.Escape });
            inputEntity.Keybinding_Set(InputKeyEnum.Undo, new KeyCode[] { KeyCode.Z });

            GameBusiness.Init(gameBusinessContext);

            UIApp.Init(uiAppContext);
            VFXApp.Init(vfxAppContext);

        }

        void Binding() {
            var uiEvt = uiAppContext.evt;

            // UI
            // - Login
            uiEvt.Login_OnStartGameClickHandle += () => {
                GameBusiness.ExitLogin(gameBusinessContext);
                GameBusiness.EnterGame(gameBusinessContext, isTestMode, testMapTypeID);
            };

            uiEvt.Login_OnExitGameClickHandle += () => {
                GameBusiness.ExitApplication(gameBusinessContext);
            };

            uiEvt.Login_OnLoadGameClickHandle += () => {
                GameBusiness.ExitLogin(gameBusinessContext);
                GameBusiness.Login_OnLoadGameClick(gameBusinessContext);
            };

            // - GameInfo
            uiEvt.GameInfo_OnRestartBtnClickHandle += () => {
                GameBusiness.UIGameInfo_OnRestartBtnClick(gameBusinessContext);
            };

            // - GameOver
            uiEvt.GameOver_OnRestartGameClickHandle += () => {
                GameBusiness.UIGameOver_OnRestartGame(gameBusinessContext);
            };

            uiEvt.GameOver_OnExitGameClickHandle += () => {
                GameBusiness.UIGameOver_OnExitGameClick(gameBusinessContext);
            };

            uiEvt.GameOver_OnNextLevelClickHandle += () => {
                GameBusiness.UIGameOver_OnNextLevelClick(gameBusinessContext);
            };

        }

        async Task LoadAssets() {
            await UIApp.LoadAssets(uiAppContext);
            await VFXApp.LoadAssets(vfxAppContext);
            await AssetsInfra.LoadAssets(assetsInfraContext);
            await TemplateInfra.LoadAssets(templateInfraContext);
            await SoundApp.LoadAssets(soundAppContext);
        }

        void OnApplicationQuit() {
            TearDown();
        }

        void OnDestroy() {
            TearDown();
        }

        void TearDown() {
            if (isTearDown) {
                return;
            }
            isTearDown = true;

            uiAppContext.evt.Clear();

            AssetsInfra.ReleaseAssets(assetsInfraContext);
            TemplateInfra.Release(templateInfraContext);
            SoundApp.ReleaseAssets(soundAppContext);
            VFXApp.ReleaseAssets(vfxAppContext);
            UIApp.ReleaseAssets(uiAppContext);

            GameBusiness.TearDown(gameBusinessContext);
            SoundApp.TearDown(soundAppContext);
            VFXApp.TearDown(vfxAppContext);
            UIApp.TearDown(uiAppContext);
            DBInfra.TearDown(dBInfraContext);
        }

#if UNITY_EDITOR
        void OnDrawGizmos() {
            GameBusiness.OnDrawCameraGizmos(gameBusinessContext, drawCameraGizmos);
            GameBusiness.OnDrawMapGizmos(gameBusinessContext, drawMapGizmos);
        }
#endif

    }

}