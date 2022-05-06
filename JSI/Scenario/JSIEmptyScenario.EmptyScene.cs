using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEmptyScenario : XScenario {
        public class EmptyScene : JSIScene {
            // singleton pattern
            private static EmptyScene mSingleton = null;
            public static EmptyScene getSingleton() {
                Debug.Assert(EmptyScene.mSingleton != null);
                return EmptyScene.mSingleton;
            }
            public static EmptyScene createSingleton(XScenario scenario) {
                Debug.Assert(EmptyScene.mSingleton == null);
                EmptyScene.mSingleton = new EmptyScene(scenario);
                return EmptyScene.mSingleton;
            }
            private EmptyScene(XScenario scenario) : base(scenario) {
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

            public override void wrapUp() {
            }
        }
    }
}