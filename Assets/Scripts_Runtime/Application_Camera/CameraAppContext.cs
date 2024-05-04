using System;
using System.Threading.Tasks;
using MortiseFrame.Swing;
using TenonKit.Vista.Camera2D;
using UnityEngine;

namespace Oshi {

    public class CameraAppContext {

        public TemplateInfraContext templateInfraContext;

        // CameraCore
        public Camera2DCore cameraCore;
        public int mainCameraID;

        public Camera mainCamera;

        public CameraAppContext(Camera mainCamera, Vector2 screenSize) {
            cameraCore = new Camera2DCore(mainCamera, screenSize);
            this.mainCamera = mainCamera;
        }

    }

}