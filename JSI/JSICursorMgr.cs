using System.Collections.Generic;
using UnityEngine;

namespace JSI {
    public class JSICursorMgr {
        // constants
        public static readonly float PEN_RADIUS = 8f; // in px
        public static readonly float TOUCH_RADIUS = 20f; // in px

        // fields
        private JSICursor2D mPenCursor = null;
        public JSICursor2D getPenCursor() {
            return this.mPenCursor;
        }
        private List<JSICursor2D> mTouchCursors = null;
        public List<JSICursor2D> getTouchCursors() {
            return this.mTouchCursors;
        }

        // constructor
        public JSICursorMgr(JSIApp jsi) {
            // pen cursor
            this.mPenCursor = new JSICursor2D(jsi, JSIUtil.createId(),
                "PenCursor", JSICursorMgr.PEN_RADIUS, Vector2.zero);
            this.mPenCursor.getGameObject().SetActive(false);

            // touch cursors
            this.mTouchCursors = new List<JSICursor2D>();
        }

        // public methods
        public JSICursor2D findTouchCursor(JSITouchMark tm) {
            if(tm!=null){
                foreach (JSICursor2D tc in this.mTouchCursors) {
                    if (tm.getId() == tc.getId()) {
                        return tc;
                    }
                }
            }
            return null;
        }
        public JSICursor2D findTouchCursor(JSITouchPacket tp) {
            foreach (JSICursor2D tc in this.mTouchCursors) {
                if (tp.getId() == tc.getId()) {
                    return tc;
                }
            }
            return null;
        }
    }
}