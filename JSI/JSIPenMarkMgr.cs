using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JSI {
    public class JSIPenMarkMgr {
        // constants
        private static readonly int MAX_NUM_PEN_MARKS = 10;

        //fields
        private List<JSIPenMark> mPenMarks = null;
        public List<JSIPenMark> getPenMarks() {
            return mPenMarks;
        }
        private List<JSIPenMark> mEraserMarks = null;
        public List<JSIPenMark> getEraserMarks() {
            return this.mEraserMarks;
        }

        //constructor
        public JSIPenMarkMgr() {
            this.mPenMarks = new List<JSIPenMark>();
            this.mEraserMarks = new List<JSIPenMark>();
        }

        public void addPenMark(JSIPenMark penMark) {
            this.mPenMarks.Add(penMark);
            if (this.mPenMarks.Count > JSIPenMarkMgr.MAX_NUM_PEN_MARKS) {
                this.mPenMarks.RemoveAt(0);
                Debug.Assert(this.mPenMarks.Count <= JSIPenMarkMgr.
                    MAX_NUM_PEN_MARKS);
            }
        }

        public void addEraserMark(JSIPenMark eraserMark) {
            this.mEraserMarks.Add(eraserMark);
            if (this.mEraserMarks.Count > JSIPenMarkMgr.MAX_NUM_PEN_MARKS) {
                this.mEraserMarks.RemoveAt(0);
                Debug.Assert(this.mEraserMarks.Count <= JSIPenMarkMgr.
                    MAX_NUM_PEN_MARKS);
            }
        }

        public JSIPenMark getLastPenMark() {
            int size = this.mPenMarks.Count;
            if (size == 0) {
                return null;
            } else {
                return this.mPenMarks.Last();
            }
        }

        public JSIPenMark getLastEraserMark() {
            int size = this.mEraserMarks.Count;
            if (size == 0) {
                return null;
            }
            else {
                return this.mEraserMarks.Last();
            }
        }

        public JSIPenMark getRecentPenMark(int i) {
            int size = this.mPenMarks.Count;
            int index = size - 1 - i;
            if (index < 0 || index >= size) {
                return null;
            } else {
                return this.mPenMarks[index];
            }
        }

        public JSIPenMark getRecentEraserMark(int i) {
            int size = this.mEraserMarks.Count;
            int index = size - 1 - i;
            if (index < 0 || index >= size) {
                return null;
            } else {
                return this.mEraserMarks[index];
            }
        }

        public bool penDown(Vector2 pt) {
            JSIPenMark penMark = new JSIPenMark(pt);
            this.addPenMark(penMark);
            return true;
        }

        public bool penDrag(Vector2 pt) {
            JSIPenMark penMark = this.getLastPenMark();

            if (penMark != null) {
                return penMark.addPt(pt);
            } else {
                return false;
            }
        }

        public bool penUp(Vector2 pt) {
            return true;
        }

        public bool eraserDown(Vector2 pt) {
            JSIPenMark eraserMark = new JSIPenMark(pt);
            this.addEraserMark(eraserMark);
            return true;
        }

        public bool eraserDrag(Vector2 pt) {
            JSIPenMark eraserMark = this.getLastEraserMark();

            if (eraserMark != null) {
                return eraserMark.addPt(pt);
            }
            else {
                return false;
            }
        }

        public bool eraserUp(Vector2 pt) {
            return true;
        }
    }
}