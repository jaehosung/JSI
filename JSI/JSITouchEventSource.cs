using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;

namespace JSI {
    public class JSITouchEventSource {
        // fields
        private JSIEventListener mEventListener = null;
        public void setEventListener(JSIEventListener eventListener) {
            this.mEventListener = eventListener;
        }
        private List<JSITouchPacket> mPrevTouches = null;
        private List<JSITouchPacket> mCurTouches = null;

        // constructor
        public JSITouchEventSource() {
            this.mPrevTouches = new List<JSITouchPacket>();
            this.mCurTouches = new List<JSITouchPacket>();
            TouchSimulation.Disable();
        }

        // WARNING!!!
        // don't use TouchControl's phase information (e.g. Began, Moved, Ended)
        // because they are VERY unreliable and sometimes do not fire properly
        public void update() {

            // detect pen input
            Vector2 penPt = JSIUtil.VECTOR2_NAN;
            if (Pen.current.tip.isPressed || Pen.current.eraser.isPressed) {
                penPt = Pen.current.position.ReadValue();
            }

            // detect touch changes
            this.mCurTouches.Clear();
            foreach (TouchControl tc in Touchscreen.current.touches) {
                if (tc.press.isPressed) {
                    string id = tc.touchId.ReadValue().ToString();
                    Vector2 pt = tc.position.ReadValue();

                    // Unity also registers pen input as touch input
                    // also, TouchSimulation.Disable() doesn't work
                    // so, manually verify that touch is not pen
                    if (pt != penPt) {
                        JSITouchPacket touch = new JSITouchPacket(id, pt);
                        this.mCurTouches.Add(touch);
                    }
                }
            }

            // touch down
            foreach (JSITouchPacket curTouch in this.mCurTouches) {
                bool prevTouchExists = false;
                foreach (JSITouchPacket prevTouch in this.mPrevTouches) {
                    if (prevTouch.getId() == curTouch.getId()) {
                        prevTouchExists = true;
                    }
                }

                if (!prevTouchExists) {
                    this.mEventListener.touchDown(curTouch);
                }
            }

            // touch dragged
            List<JSITouchPacket> draggedTouches = new List<JSITouchPacket>();
            foreach (JSITouchPacket curTouch in this.mCurTouches) {
                foreach (JSITouchPacket prevTouch in this.mPrevTouches) {
                    if (prevTouch.getId() == curTouch.getId() &&
                        prevTouch.getPt() != curTouch.getPt()) {

                        draggedTouches.Add(curTouch);
                    }
                }
            }
            
            if (draggedTouches.Count > 0) {
                this.mEventListener.touchDragged(draggedTouches);
            }

            // touch up
            foreach (JSITouchPacket prevTouch in this.mPrevTouches) {
                bool curTouchExists = false;
                foreach (JSITouchPacket curTouch in this.mCurTouches) {
                    if (prevTouch.getId() == curTouch.getId()) {
                        curTouchExists = true;
                    }
                }

                if (!curTouchExists) {
                    this.mEventListener.touchUp(prevTouch);
                }
            }

            // update the previous data
            this.mPrevTouches.Clear();
            foreach (JSITouchPacket touch in this.mCurTouches) {
                this.mPrevTouches.Add(touch);
            }
        }
    }
}