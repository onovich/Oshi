using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour {

    MainContext ctx;

    void Start() {

        // From Scene
        var canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        // Ctor
        ctx = new MainContext();

        // Binding
        Binding();

        Action action = async () => {
            try {
                // Load
                await Load();
                // Init
                Init();
                // Enter
                Enter();

                ctx.isLoaded = true;

            } catch (Exception e) {
                Debug.Log(e);
            }
        };
        action.Invoke();

    }

    async Task Load() {

        var assetsCoreContext = ctx.assetsCoreContext;
        var templateCoreContext = ctx.templateCoreContext;

        await AssetsCore.Load(assetsCoreContext);
        await TemplateCore.Load(templateCoreContext);

    }

    void Init() {

        Application.targetFrameRate = 120;

    }

    void Enter() {

    }

    void Binding() {

    }

    void Update() {

        if (!ctx.isLoaded) {
            return;
        }

    }

    void OnApplicationQuit() {
        TearDown();
    }

    void OnDestory() {
        TearDown();
    }

    void TearDown() {
        if (ctx.isTearDown) {
            return;
        }
        ctx.isTearDown = true;
    }

}