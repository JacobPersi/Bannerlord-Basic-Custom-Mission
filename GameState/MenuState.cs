﻿using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screen;
using TaleWorlds.ScreenSystem;

namespace CustomMission {

    public class MenuState : GameState {

        protected override void OnTick(float dt) {
            if (Input.IsKeyDown(InputKey.F1)) {
                CustomMissionManager.OpenSceneEditor();
            }
        }
    }

    [GameStateScreen(typeof(MenuState))]
    public class CustomGameScene : ScreenBase, IGameStateListener {
        private MenuState _state;
        private Scene _scene;
        public Camera _camera;
        private SceneLayer _sceneLayer;

        public CustomGameScene(MenuState state) {
            this._state = state;
        }

        protected override void OnInitialize() {
            base.OnInitialize();
            this._sceneLayer = new SceneLayer("SceneLayer");
            base.AddLayer(this._sceneLayer);
            this._sceneLayer.SceneView.SetResolutionScaling(true);
            this._camera = Camera.CreateCamera();
            Common.MemoryCleanupGC();

        }

        protected override void OnFinalize() {
            base.OnFinalize();
        }

        protected override void OnActivate() {
            this._scene = Scene.CreateNewScene(true);
            this._scene.SetName("SomeScene");
            this._scene.SetPlaySoundEventsAfterReadyToRender(true);
            this._scene.Read("empty_scene");

            for (int i = 0; i < 40; i++) {
                this._scene.Tick(0.1f);
            }

            Vec3 vec = default(Vec3);
            GameEntity cameraInstance = this._scene.FindEntityWithTag("camera_instance");
            cameraInstance.GetCameraParamsFromCameraScript(this._camera, ref vec);

            SoundManager.SetListenerFrame(this._camera.Frame);
            if (this._sceneLayer != null) {
                this._sceneLayer.SetScene(this._scene);
                this._sceneLayer.SceneView.SetEnable(true);
                this._sceneLayer.SceneView.SetSceneUsesShadows(true);
            }
            LoadingWindow.DisableGlobalLoadingWindow();
            base.OnActivate();
        }

        protected override void OnFrameTick(float dt) {
            base.OnFrameTick(dt);
            if (this._sceneLayer != null && this._sceneLayer.SceneView.ReadyToRender()) {
                this._sceneLayer.SetCamera(this._camera);
            }
            this._scene.Tick(dt);
        }

        protected override void OnDeactivate() {
            base.OnDeactivate();
        }

        void IGameStateListener.OnActivate() { }

        void IGameStateListener.OnDeactivate() { }

        void IGameStateListener.OnInitialize() { }

        void IGameStateListener.OnFinalize() { }
    }
}
