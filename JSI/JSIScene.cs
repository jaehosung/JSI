using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI {
    public abstract class JSIScene : XScene {
        // constructor
        protected JSIScene(XScenario scenario) : base(scenario) {
        }

        // methods
        public abstract void handleKeyDown(Key k);
        public abstract void handleKeyUp(Key k);
        public abstract void handlePenDown(Vector2 pt);
        public abstract void handlePenDrag(Vector2 pt);
        public abstract void handlePenUp(Vector2 pt);
        public abstract void handleEraserDown(Vector2 pt);
        public abstract void handleEraserUp(Vector2 pt);
        public abstract void handleEraserDrag(Vector2 pt);
        public abstract void handleTouchDown();
        public abstract void handleTouchDrag();
        public abstract void handleTouchUp();
    }
}