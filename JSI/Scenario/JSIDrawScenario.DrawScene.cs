using JSI.Cmd;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIDrawScenario : XScenario {
        public class DrawScene : JSIScene {
            // singleton pattern
            private static DrawScene mSingleton = null;
            public static DrawScene getSingleton() {
                Debug.Assert(DrawScene.mSingleton != null);
                return DrawScene.mSingleton;
            }
            public static DrawScene createSingleton(XScenario scenario) {
                Debug.Assert(DrawScene.mSingleton == null);
                DrawScene.mSingleton = new DrawScene(scenario);
                return DrawScene.mSingleton;
            }
            private DrawScene(XScenario scenario) : base(scenario) {
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
                JSICmdToUpdateCurPtCurve2D.execute(jsi);
            }

            public override void handlePenUp(Vector2 pt) {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToAddCurPtCurve2DToPtCurve2Ds.execute(jsi);
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
            }

            public override void wrapUp() {}
        }
    }
}