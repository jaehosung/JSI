using System.Collections.Generic;
using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class MoveNRotateWithTouchScene : JSIScene {
            // singleton pattern
            private static MoveNRotateWithTouchScene mSingleton = null;
            public static MoveNRotateWithTouchScene getSingleton() {
                Debug.Assert(MoveNRotateWithTouchScene.mSingleton != null);
                return MoveNRotateWithTouchScene.mSingleton;
            }
            public static MoveNRotateWithTouchScene createSingleton(
                XScenario scenario) {
                Debug.Assert(MoveNRotateWithTouchScene.mSingleton == null);
                MoveNRotateWithTouchScene.mSingleton =
                    new MoveNRotateWithTouchScene(scenario);
                return MoveNRotateWithTouchScene.mSingleton;
            }
            private MoveNRotateWithTouchScene(XScenario scenario) :
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
                if (tms.Contains(scenario.getManipulatingTouchMarks()[0]) ||
                    tms.Contains(scenario.getManipulatingTouchMarks()[1])) {

                    JSICmdToMoveNRotateStandingCardWithTouch.execute(jsi);
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

                    XCmdToChangeScene.execute(jsi, JSIEditStandingCardScenario.
                        MoveWithTouchScene.getSingleton(), this.mReturnScene);
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