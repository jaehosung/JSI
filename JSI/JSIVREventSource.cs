namespace JSI {
    public class JSIVREventSource {
        // constants
        // trigger value is continuous from 0 to 1
        private static readonly float TRIGGER_THRESHOLD = 0.95f;

        // fields
        private JSIEventListener mEventListener = null;
        public void setEventListener(JSIEventListener eventListener) {
            this.mEventListener = eventListener;

            // headset mounted
            OVRManager.HMDMounted += this.mEventListener.VRheadsetMounted;
            // headset unmounted
            OVRManager.HMDUnmounted += this.mEventListener.VRheadsetUnmounted;
        }

        // constructor
        public JSIVREventSource() {}

        // methods
        public void update() {
        }
    }
}