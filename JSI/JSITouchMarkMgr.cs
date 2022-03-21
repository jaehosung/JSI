using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JSI {
    public class JSITouchMarkMgr {
        // constants
        private static readonly int MAX_NUM_UP_TOUCH_MARKS = 100;

        //fields
        private List<JSITouchMark> mDownTouchMarks = null;
        public List<JSITouchMark> getDownTouchMarks() {
            return this.mDownTouchMarks;
        }
        private List<JSITouchMark> mDraggedTouchMarks = null;
        public List<JSITouchMark> getDraggedTouchMarks() {
            return this.mDraggedTouchMarks;
        }
        private List<JSITouchMark> mUpTouchMarks = null;
        public List<JSITouchMark> getUpTouchMarks() {
            return this.mUpTouchMarks;
        }
        private bool mWasTouchDownJustNow = false;
        public bool wasTouchDownJustNow() {
            return this.mWasTouchDownJustNow;
        }
        public void setTouchDownWasJustNow(bool b) {
            this.mWasTouchDownJustNow = b;
        }
        private bool mWasTouchDragJustNow = false;
        public bool wasTouchDragJustNow() {
            return this.mWasTouchDragJustNow;
        }
        public void setTouchDragWasJustNow(bool b) {
            this.mWasTouchDragJustNow = b;
        }
        private bool mWasTouchUpJustNow = false;
        public bool wasTouchUpJustNow() {
            return this.mWasTouchUpJustNow;
        }
        public void setTouchUpWasJustNow(bool b) {
            this.mWasTouchUpJustNow = b;
        }

        //constructor
        public JSITouchMarkMgr() {
            this.mDownTouchMarks = new List<JSITouchMark>();
            this.mDraggedTouchMarks = new List<JSITouchMark>();
            this.mUpTouchMarks = new List<JSITouchMark>();
        }

        public void addUpTouchMark(JSITouchMark touchMark) {
            this.mUpTouchMarks.Add(touchMark);
            if (this.mUpTouchMarks.Count > JSITouchMarkMgr.
                MAX_NUM_UP_TOUCH_MARKS) {

                this.mUpTouchMarks.RemoveAt(0);
                Debug.Assert(this.mUpTouchMarks.Count <= JSITouchMarkMgr.
                    MAX_NUM_UP_TOUCH_MARKS);
            }
        }

        public JSITouchMark getFirstDownTouchMark() {
            return this.getEarliestDownTouchMark(0);
        }
        public JSITouchMark getSecondDownTouchMark() {
            return this.getEarliestDownTouchMark(1);
        }
        public JSITouchMark getLastDownTouchMark() {
            return this.getLatestDownTouchMark(0);
        }

        public JSITouchMark getEarliestDownTouchMark(int i) {
            int size = this.mDownTouchMarks.Count;
            int index = i;
            if (index < 0 || index >= size) {
                return null;
            } else {
                return this.mDownTouchMarks[index];
            }
        }

        public JSITouchMark getLatestDownTouchMark(int i) {
            int size = this.mDownTouchMarks.Count;
            int index = size - 1 - i;
            if (index < 0 || index >= size) {
                return null;
            } else {
                return this.mDownTouchMarks[index];
            }
        }

        public JSITouchMark getLastUpTouchMark() {
            return this.getLatestUpTouchMark(0);
        }

        public JSITouchMark getLatestUpTouchMark(int i) {
            int size = this.mUpTouchMarks.Count;
            int index = size - 1 - i;
            if (index < 0 || index >= size) {
                return null;
            } else {
                return this.mUpTouchMarks[index];
            }
        }

        public bool touchDown(JSITouchPacket tp) {
            foreach (JSITouchMark tm in this.mDownTouchMarks) {
                if (tp.getId() == tm.getId()) {
                    return false;
                }
            }

            JSITouchMark touchMark = new JSITouchMark(tp);
            this.mDownTouchMarks.Add(touchMark);
            return true;
        }

        public bool touchDrag(List<JSITouchPacket> tps) {
            this.mDraggedTouchMarks.Clear();
            foreach (JSITouchPacket tp in tps) {
                foreach (JSITouchMark tm in this.mDownTouchMarks) {
                    if (tp.getId() == tm.getId() && tm.addPt(tp.getPt())) {
                        this.mDraggedTouchMarks.Add(tm);
                    }
                }
            }

            bool isAnyTouchDragged = this.mDraggedTouchMarks.Count > 0;
            return isAnyTouchDragged;
        }

        public bool touchUp(JSITouchPacket tp) {
            JSITouchMark upTouchMark = null;
            foreach (JSITouchMark tm in this.mDownTouchMarks) {
                if (tp.getId() == tm.getId()) {
                    upTouchMark = tm;
                }
            }

            if (upTouchMark != null) {
                this.mDownTouchMarks.Remove(upTouchMark);
                this.addUpTouchMark(upTouchMark);
                return true;
            } else {
                return false;
            }
        }
    }
}