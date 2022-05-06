using System.Collections.Generic;
using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class MoveWithTouchScene : JSIScene {
            // singleton pattern
            private static MoveWithTouchScene mSingleton = null;
            public static MoveWithTouchScene getSingleton() {
                Debug.Assert(MoveWithTouchScene.mSingleton != null);
                return MoveWithTouchScene.mSingleton;
            }
            public static MoveWithTouchScene createSingleton(
                XScenario scenario) {
                Debug.Assert(MoveWithTouchScene.mSingleton == null);
                MoveWithTouchScene.mSingleton = new MoveWithTouchScene(
                    scenario);
                return MoveWithTouchScene.mSingleton;
            }
            private MoveWithTouchScene(XScenario scenario) :
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
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;
                JSITouchMark tm = jsi.getTouchMarkMgr().getLastDownTouchMark();
                JSICursor2D tc = jsi.getCursorMgr().findTouchCursor(tm);

                if (tc.hits(scenario.getSelectedStandingCard().getStand())) {
                    XCmdToChangeScene.execute(jsi, JSIEditStandingCardScenario.
                        MoveNRotateWithTouchScene.getSingleton(),
                        this.mReturnScene);
                    return;
                }
            }

            public override void handleTouchDrag() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;
                List<JSITouchMark> tms = jsi.getTouchMarkMgr().
                    getDraggedTouchMarks();
                if (tms.Contains(scenario.getManipulatingTouchMarks()[0])) {
                    JSICmdToMoveStandingCardWithTouch.execute(jsi);
                    JSICmdToSendTransformStandingCardMsg.execute(jsi);
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