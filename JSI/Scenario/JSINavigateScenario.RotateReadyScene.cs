using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSINavigateScenario : XScenario {
        public class RotateReadyScene : JSIScene {
            // singleton pattern
            private static RotateReadyScene mSingleton = null;
            public static RotateReadyScene getSingleton() {
                Debug.Assert(RotateReadyScene.mSingleton != null);
                return RotateReadyScene.mSingleton;
            }
            public static RotateReadyScene createSingleton(XScenario scenario) {
                Debug.Assert(RotateReadyScene.mSingleton == null);
                RotateReadyScene.mSingleton = new RotateReadyScene(scenario);
                return RotateReadyScene.mSingleton;
            }
            private RotateReadyScene(XScenario scenario) : base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                switch (k) {
                    case Key.LeftAlt:
                        XCmdToChangeScene.execute(jsi,
                            JSINavigateScenario.TranslateReadyScene.getSingleton(),
                            this.mReturnScene);
                        break;
                }
            }

            public override void handleKeyUp(Key k) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                switch (k) {
                    case Key.LeftCtrl:
                        XCmdToChangeScene.execute(jsi, this.mReturnScene, null);
                        break;
                }
            }

            public override void handlePenDown(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICursor2D pc = jsi.getCursorMgr().getPenCursor();

                foreach (JSIStandingCard sc in
                    jsi.getStandingCardMgr().getStandingCards()) {
                    if (pc.hits(sc.getStand())) {
                        JSICmdToSelectSmallestStandingCardByStand.execute(jsi,
                            pc);
                        XCmdToChangeScene.execute(jsi,
                            JSIEditStandingCardScenario.RotateWithPenScene.
                            getSingleton(), this.mReturnScene);
                        return;
                    }
                }

                XCmdToChangeScene.execute(jsi,
                    JSINavigateScenario.TumbleCameraScene.getSingleton(),
                    this.mReturnScene);
            }

            public override void handlePenDrag(Vector2 pt) {
            }

            public override void handlePenUp(Vector2 pt) {
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

                // activate stands.
                // deactivate scale handles.
                foreach (JSIStandingCard sc in
                    jsi.getStandingCardMgr().getStandingCards()) {
                    sc.getStand().getGameObject().SetActive(true);
                    sc.highlightStand(false);
                    sc.getScaleHandle().getGameObject().SetActive(false);
                }
            }

            public override void wrapUp() {
            }
        }
    }
}