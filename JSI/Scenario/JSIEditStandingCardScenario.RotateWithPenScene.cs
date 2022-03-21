using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class RotateWithPenScene : JSIScene {
            // singleton pattern
            private static RotateWithPenScene mSingleton = null;
            public static RotateWithPenScene getSingleton() {
                Debug.Assert(RotateWithPenScene.mSingleton != null);
                return RotateWithPenScene.mSingleton;
            }
            public static RotateWithPenScene createSingleton(XScenario scenario) {
                Debug.Assert(RotateWithPenScene.mSingleton == null);
                RotateWithPenScene.mSingleton = new RotateWithPenScene(scenario);
                return RotateWithPenScene.mSingleton;
            }
            private RotateWithPenScene(XScenario scenario) : base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                switch (k) {
                    case Key.LeftAlt:
                        XCmdToChangeScene.execute(jsi,
                            JSIEditStandingCardScenario.MoveWithPenScene.
                            getSingleton(), this.mReturnScene);
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
            }

            public override void handlePenDrag(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToRotateStandingCardWithPen.execute(jsi);
            }

            public override void handlePenUp(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                XCmdToChangeScene.execute(jsi,
                    JSINavigateScenario.RotateReadyScene.getSingleton(),
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

            public override void getReady() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();

                // deactivate all stands.
                // deactivate all scale handles.
                foreach (JSIStandingCard sc in
                    jsi.getStandingCardMgr().getStandingCards()) {
                    sc.getStand().getGameObject().SetActive(false);
                    sc.getScaleHandle().getGameObject().SetActive(false);
                }

                // activate and highlight only the selected stand.
                JSIStandingCard selectedSC =
                    JSIEditStandingCardScenario.getSingleton().
                    getSelectedStandingCard();
                selectedSC.getStand().getGameObject().SetActive(true);
                selectedSC.highlightStand(true);
            }

            public override void wrapUp() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToTakeSnapshot.execute(jsi);
            }
        }
    }
}