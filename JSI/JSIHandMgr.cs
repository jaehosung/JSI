using UnityEngine;

namespace JSI {
    public class JSIHandMgr {
        // fields
        private JSIHand mLeftHand = null;
        public JSIHand getLeftHand() {
            return this.mLeftHand;
        }
        private JSIHand mRightHand = null;
        public JSIHand getRightHand() {
            return this.mRightHand;
        }

        // constructor
        public JSIHandMgr(GameObject leftHandPrefab, GameObject rightHandPrefab) {
            this.mLeftHand = new JSIHand(leftHandPrefab);
            this.mRightHand = new JSIHand(rightHandPrefab);
        }
    }
}