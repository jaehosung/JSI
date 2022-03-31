using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIDefaultScenario : XScenario {
        public class ReadyScene : JSIScene {
            // singleton pattern
            private static ReadyScene mSingleton = null;
            public static ReadyScene getSingleton() {
                Debug.Assert(ReadyScene.mSingleton != null);
                return ReadyScene.mSingleton;
            }
            public static ReadyScene createSingleton(XScenario scenario) {
                Debug.Assert(ReadyScene.mSingleton == null);
                ReadyScene.mSingleton = new ReadyScene(scenario);
                return ReadyScene.mSingleton;
            }
            private ReadyScene(XScenario scenario) : base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                switch (k) {
                    case Key.LeftCtrl:
                        XCmdToChangeScene.execute(jsi,
                            JSINavigateScenario.RotateReadyScene.getSingleton(),
                            this);
                        break;
                }
            }

            public override void handleKeyUp(Key k) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                switch (k) {
                    case Key.Enter:
                        //JSICmdToCreateEmptyStandingCard.execute(jsi);
                        JSICmdToCreateStandingCard.execute(jsi);
                        JSICmdToTakeSnapshot.execute(jsi);
                        JSICmdToSendAddStandingCardMsg.execute(jsi);
                        break;
                    case Key.Z:
                        JSICmdToUndo.execute(jsi);
                        break;
                    case Key.Y:
                        JSICmdToRedo.execute(jsi);
                        break;
                    case Key.S:
                        JSICmdToSaveFile.execute(jsi);
                        break;
                    case Key.O:
                        JSICmdToOpenFile.execute(jsi);
                        JSICmdToAutoSave.execute(jsi);
                        break;
                }
            }

            public override void handlePenDown(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICursor2D pc = jsi.getCursorMgr().getPenCursor();

                foreach (JSIStandingCard sc in jsi.getStandingCardMgr().
                    getStandingCards()) {

                    if (pc.hits(sc.getScaleHandle())) {
                        JSICmdToSelectSmallestStandingCardByScaleHandle.
                            execute(jsi, pc);
                        XCmdToChangeScene.execute(jsi,
                            JSIEditStandingCardScenario.ScaleWithPenScene.
                            getSingleton(), this);
                        return;
                    }
                }
                JSICmdToCreateCurPtCurve2D.execute(jsi);
                XCmdToChangeScene.execute(jsi,
                    JSIDrawScenario.DrawScene.getSingleton(), this);
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
                JSITouchMark tm = jsi.getTouchMarkMgr().getLastDownTouchMark();
                JSICursor2D tc = jsi.getCursorMgr().findTouchCursor(tm);

                foreach (JSIStandingCard sc in jsi.getStandingCardMgr().
                    getStandingCards()) {

                    // scale handle
                    if (tc.hits(sc.getScaleHandle())) {
                        JSICmdToSelectSmallestStandingCardByScaleHandle.
                            execute(jsi, tc);
                        XCmdToChangeScene.execute(jsi,
                            JSIEditStandingCardScenario.ScaleWithTouchScene.
                            getSingleton(), this);
                        return;
                    }

                    // stand
                    if (tc.hits(sc.getStand())) {
                        JSICmdToSelectSmallestStandingCardByStand.
                            execute(jsi, tc);
                        XCmdToChangeScene.execute(jsi,
                            JSIEditStandingCardScenario.MoveWithTouchScene.
                            getSingleton(), this);
                        return;
                    }
                }
            }

            public override void handleTouchDrag() {
            }

            public override void handleTouchUp() {
            }

            public override void getReady() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();

                // deactivate stands.
                // activate scale handles.
                foreach (JSIStandingCard sc in
                    jsi.getStandingCardMgr().getStandingCards()) {
                    sc.highlightStand(false);
                    sc.highlightScaleHandle(false);
                    sc.getStand().getGameObject().SetActive(true);
                    sc.getScaleHandle().getGameObject().SetActive(true);
                }

                // auto save log & sketch
                // should be added to all 'M' scenes
                JSICmdToAutoSave.execute(jsi);
            }

            public override void wrapUp() {
            }
        }
    }
}