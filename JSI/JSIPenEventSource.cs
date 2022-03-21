using UnityEngine;
using UnityEngine.InputSystem;

namespace JSI {
    public class JSIPenEventSource {
        // fields
        private JSIEventListener mEventListener = null;
        public void setEventListener(JSIEventListener eventListener) {
            this.mEventListener = eventListener;
        }
        private bool mWasPenDown = false;
        private bool mIsPenDown = false;
        private bool mWasEraserDown = false;
        private bool mIsEraserDown = false;
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;

        // constructor
        public JSIPenEventSource() {}

        // methods
        public void update() {
            // detect pen & eraser changes
            this.mIsPenDown = Pen.current.tip.isPressed;
            this.mIsEraserDown = Pen.current.eraser.isPressed;
            this.mCurPt = Pen.current.position.ReadValue();

            // pen down
            if (!this.mWasPenDown && this.mIsPenDown) {
                this.mEventListener.penDown(this.mCurPt);
            }

            // pen drag
            if (this.mWasPenDown && this.mIsPenDown &&
                this.mPrevPt != this.mCurPt) {

                this.mEventListener.penDragged(this.mCurPt);
            }

            // pen up
            if (this.mWasPenDown && !this.mIsPenDown) {
                this.mEventListener.penUp(this.mCurPt);
            }

            // eraser down
            if (!this.mWasEraserDown && this.mIsEraserDown) {
                this.mEventListener.eraserDown(this.mCurPt);
            }

            // eraser drag
            if (this.mWasEraserDown && this.mIsEraserDown &&
                this.mPrevPt != this.mCurPt) {
                this.mEventListener.eraserDragged(this.mCurPt);
            }

            // eraser up
            if (this.mWasEraserDown && !this.mIsEraserDown) {
                this.mEventListener.eraserUp(this.mCurPt);
            }

            // updates the previous data
            this.mWasPenDown = this.mIsPenDown;
            this.mWasEraserDown = this.mIsEraserDown;
            this.mPrevPt = this.mCurPt;
        }
    }
}