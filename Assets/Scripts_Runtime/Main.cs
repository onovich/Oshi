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
        [SerializeField] bool isTestMode_testGiven;
        [SerializeField] bool isTestMode_testLast;

        InputEntity inputEntity;

        AssetsInfraContext assetsInfraContext;
        TemplateInfraContext templateInfraContext;

        LoginBusinessContext loginBusinessContext;
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

            loginBusinessContext = new LoginBusinessContext();
            gameBusinessContext = new GameBusinessContext();

            uiAppContext = new UIAppContext("UI", mainCanvas, hudFakeCanvas, mainCamera);
            vfxAppContext = new VFXAppContext("VFX", vfxRoot);
            cameraAppContext = new CameraAppContext(mainCamera, new Vector2(Screen.width, Screen.height));
            ppAppContext = new PPAppContext(mainVolume);
            soundAppContext = new SoundAppContext(soundRoot);

            assetsInfraContext = new AssetsInfraContext();
            templateInfraContext = new TemplateInfraContext();

            // Inject
            loginBusinessContext.uiContext = uiAppContext;
            loginBusinessContext.templateInfraContext = templateInfraContext;
            loginBusinessContext.soundContext = soundAppContext;

            gameBusinessContext.inputEntity = inputEntity;
            gameBusinessContext.assetsInfraContext = assetsInfraContext;
            gameBusinessContext.templateInfraContext = templateInfraContext;
            gameBusinessContext.uiContext = uiAppContext;
            gameBusinessContext.vfxContext = vfxAppContext;
            gameBusinessContext.cameraContext = cameraAppContext;
            gameBusinessContext.ppAppContext = ppAppContext;
            gameBusinessContext.soundContext = soundAppContext;
            gameBusinessContext.mainCamera = mainCamera;

            cameraAppContext.templateInfraContext = templateInfraContext;
            ppAppContext.templateInfraContext = templateInfraContext;

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
            LoginBusiness.Enter(loginBusinessContext);
        }

        void Update() {

            if (!isLoadedAssets) {
                return;
            }

            var dt = Time.deltaTime;
            LoginBusiness.Tick(loginBusinessContext, dt);
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

            GameBusiness.Init(gameBusinessContext);

            UIApp.Init(uiAppContext);
            VFXApp.Init(vfxAppContext);

        }

        void Binding() {
            var uiEvt = uiAppContext.evt;

            // UI
            // - Login
            uiEvt.Login_OnStartGameClickHandle += () => {
                LoginBusiness.Exit(loginBusinessContext);
                GameBusiness.StartGame(gameBusinessContext, isTestMode_testGiven, isTestMode_testLast);
            };

            uiEvt.Login_OnExitGameClickHandle += () => {
                LoginBusiness.ExitApplication(loginBusinessContext);
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

            loginBusinessContext.evt.Clear();
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
        }

#if UNITY_EDITOR
        void OnDrawGizmos() {
            GameBusiness.OnDrawCameraGizmos(gameBusinessContext, drawCameraGizmos);
            GameBusiness.OnDrawMapGizmos(gameBusinessContext, drawMapGizmos);
        }
#endif

    }

}