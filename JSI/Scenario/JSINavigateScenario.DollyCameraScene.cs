using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSINavigateScenario : XScenario {
        public class DollyCameraScene : JSIScene {
            // singleton pattern
            private static DollyCameraScene mSingleton = null;
            public static DollyCameraScene getSingleton() {
                Debug.Assert(DollyCameraScene.mSingleton != null);
                return DollyCameraScene.mSingleton;
            }
            public static DollyCameraScene createSingleton(XScenario scenario) {
                Debug.Assert(DollyCameraScene.mSingleton == null);
                DollyCameraScene.mSingleton = new DollyCameraScene(scenario);
                return DollyCameraScene.mSingleton;
            }
            private DollyCameraScene(XScenario scenario) : base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
            }

            public override void handleKeyUp(Key k) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                switch (k) {
                    case Key.LeftCtrl:
                        XCmdToChangeScene.execute(jsi, this.mReturnScene, null);
                        break;
                    case Key.LeftAlt:
                        XCmdToChangeScene.execute(jsi,
                            JSINavigateScenario.TumbleCameraScene.getSingleton(),
                            this.mReturnScene);
                        break;
                }

            }

            public override void handlePenDown(Vector2 pt) {
            }

            public override void handlePenDrag(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToDollyCamera.execute(jsi);
            }

            public override void handlePenUp(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                XCmdToChangeScene.execute(jsi,
                    JSINavigateScenario.TranslateReadyScene.getSingleton(),
                    this.mReturnScene);
            }

            public override void handleEraserDown(Vector2 pt) {
            }

            public override void handleEraserDrag(Vector2 pt) {
            }

            public override void handleEraserUp(Vector2 pt) {
            }

            public override void handleTouchDown() {
            }

            public override void handleTouchDrag() {
            }

            public override void handleTouchUp() {
            }

            public override void handleLeftPinchStart() {
            }

            public override void handleLeftPinchEnd() {
            }

            public override void handleRightPinchStart() {
            }

            public override void handleRightPinchEnd() {
            }

            public override void handleHandsMove() {
            }

            public override void getReady() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();

                // deactivate stands.
                // deactivate scale handles.
                foreach (JSIStandingCard sc in
                    jsi.getStandingCardMgr().getStandingCards()) {
                    sc.getStand().getGameObject().SetActive(false);
                    sc.getScaleHandle().getGameObject().SetActive(false);
                }
            }

            public override void wrapUp() {
            }
        }
    }
}