using System.Collections.Generic;
using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class ScaleWithTouchScene : JSIScene {
            // singleton pattern
            private static ScaleWithTouchScene mSingleton = null;
            public static ScaleWithTouchScene getSingleton() {
                Debug.Assert(ScaleWithTouchScene.mSingleton != null);
                return ScaleWithTouchScene.mSingleton;
            }
            public static ScaleWithTouchScene createSingleton(
                XScenario scenario) {
                Debug.Assert(ScaleWithTouchScene.mSingleton == null);
                ScaleWithTouchScene.mSingleton = new ScaleWithTouchScene(
                    scenario);
                return ScaleWithTouchScene.mSingleton;
            }
            private ScaleWithTouchScene(XScenario scenario) :
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
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;
                List<JSITouchMark> tms = jsi.getTouchMarkMgr().
                    getDraggedTouchMarks();
                if (tms.Contains(scenario.getManipulatingTouchMarks()[0])) {
                    JSICmdToScaleStandingCardWithTouch.execute(jsi);
                }
            }

            public override void handleTouchUp() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;
                JSIStandingCard sc = scenario.getSelectedStandingCard();
                JSITouchMark tm = jsi.getTouchMarkMgr().getLastUpTouchMark();

                if (scenario.getManipulatingTouchMarks().Contains(tm)) {
                    scenario.getManipulatingTouchMarks().Remove(tm);

                    XCmdToChangeScene.execute(jsi, this.mReturnScene, null);
                }
            }

            public override void getReady() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                // scene entered by touch down
                if (jsi.getTouchMarkMgr().wasTouchDownJustNow()) {
                    JSITouchMark tm = jsi.getTouchMarkMgr().getLastDownTouchMark();
                    scenario.getManipulatingTouchMarks().Add(tm);
                }

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