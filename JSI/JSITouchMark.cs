using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JSI {
    public class JSITouchMark {
        // constants
        public static readonly float MIN_DIST_BTWN_PTS = 1f; // in px

        // fields
        private string mId = string.Empty;
        public string getId() {
            return this.mId;
        }
        private List<Vector2> mPts = null;
        public List<Vector2> getPts() {
            return this.mPts;
        }

        // constructor
        public JSITouchMark(JSITouchPacket tp) {
            this.mId = tp.getId();
            this.mPts = new List<Vector2>();
            this.mPts.Add(tp.getPt());
        }

        // methods
        public bool addPt(Vector2 pt) {
            Vector2 lastPt = this.getLastPt();

            if (Vector2.Distance(lastPt, pt) >= JSITouchMark.MIN_DIST_BTWN_PTS) {
                this.mPts.Add(pt);
                return true;
            } else {
                return false;
            }
        }

        public Vector2 getLastPt() {
            return this.mPts.Last();
        }

        public Vector2 getFirstPt() {
            return this.mPts.First();
        }

        public Vector2 getRecentPt(int i) {
            int size = this.mPts.Count;
            int index = size - 1 - i;
            Debug.Assert(index >= 0 && index < size);
            return this.mPts[index];
        }
    }
}