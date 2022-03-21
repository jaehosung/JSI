using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class MoveWithPenScene : JSIScene {
            // singleton pattern
            private static MoveWithPenScene mSingleton = null;
            public static MoveWithPenScene getSingleton() {
                Debug.Assert(MoveWithPenScene.mSingleton != null);
                return MoveWithPenScene.mSingleton;
            }
            public static MoveWithPenScene createSingleton(XScenario scenario) {
                Debug.Assert(MoveWithPenScene.mSingleton == null);
                MoveWithPenScene.mSingleton = new MoveWithPenScene(scenario);
                return MoveWithPenScene.mSingleton;
            }
            private MoveWithPenScene(XScenario scenario) : base(scenario) {
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
                            JSIEditStandingCardScenario.
                            RotateWithPenScene.getSingleton(),
                            this.mReturnScene);
                        break;
                }
            }

            public override void handlePenDown(Vector2 pt) {
            }

            public override void handlePenDrag(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToMoveStandingCardWithPen.execute(jsi);
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