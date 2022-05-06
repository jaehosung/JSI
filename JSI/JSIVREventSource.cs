namespace JSI {
    public class JSIVREventSource {
        // constants
        // trigger value is continuous from 0 to 1
        private static readonly float TRIGGER_THRESHOLD = 0.95f;

        // fields
        private JSIApp mJSI = null;
        private JSIEventListener mEventListener = null;
        public void setEventListener(JSIEventListener eventListener) {
            this.mEventListener = eventListener;

            // headset mounted
            OVRManager.HMDMounted += this.mEventListener.VRheadsetMounted;
            // headset unmounted
            OVRManager.HMDUnmounted += this.mEventListener.VRheadsetUnmounted;
        }
        private bool mWasLeftHandPinching = false;
        private bool mWasRightHandPinching = false;

        // constructor
        public JSIVREventSource(JSIApp jsi) {
            this.mJSI = jsi;
        }

        // methods
        public void update() {
            // update values
            JSIHand leftHand = this.mJSI.getHandMgr().getLeftHand();
            JSIHand rightHand = this.mJSI.getHandMgr().getRightHand();
            bool isLeftHandPinching = leftHand.isPinching();
            bool isRightHandPinching = rightHand.isPinching();

            // left hand
            if (!this.mWasLeftHandPinching && isLeftHandPinching) {
                this.mEventListener.leftPinchStart();
            }
            if (this.mWasLeftHandPinching && !isLeftHandPinching) {
                this.mEventListener.leftPinchEnd();
            }

            // right hand
            if (!this.mWasRightHandPinching && isRightHandPinching) {
                this.mEventListener.rightPinchStart();
            }
            if (this.mWasRightHandPinching && !isRightHandPinching) {
                this.mEventListener.rightPinchEnd();
            }

            // both hands
            // always call, because hands are always moving
            this.mEventListener.handsMove();

            // update cache values
            this.mWasLeftHandPinching = isLeftHandPinching;
            this.mWasRightHandPinching = isRightHandPinching;
        }
    }
}