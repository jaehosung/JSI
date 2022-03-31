using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class ScaleWithPenScene : JSIScene {
            // singleton pattern
            private static ScaleWithPenScene mSingleton = null;
            public static ScaleWithPenScene getSingleton() {
                Debug.Assert(ScaleWithPenScene.mSingleton != null);
                return ScaleWithPenScene.mSingleton;
            }
            public static ScaleWithPenScene createSingleton(XScenario scenario) {
                Debug.Assert(ScaleWithPenScene.mSingleton == null);
                ScaleWithPenScene.mSingleton = new ScaleWithPenScene(scenario);
                return ScaleWithPenScene.mSingleton;
            }
            private ScaleWithPenScene(XScenario scenario) :
                base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
            }

            public override void handleKeyUp(Key k) {
            }

            public override void handlePenDown(Vector2 pt) {
            }

            public override void handlePenDrag(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToScaleStandingCardWithPen.execute(jsi);
                JSICmdToSendTransformStandingCardMsg.execute(jsi);
            }

            public override void handlePenUp(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                XCmdToChangeScene.execute(jsi, this.mReturnScene, null);
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


                // activate and highlight only the selected scale handle.
                JSIStandingCard selectedSC =
                    JSIEditStandingCardScenario.getSingleton().
                    getSelectedStandingCard();
                selectedSC.getScaleHandle().getGameObject().SetActive(true);
                selectedSC.highlightScaleHandle(true);
            }

            public override void wrapUp() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToTakeSnapshot.execute(jsi);
            }
        }
    }
}