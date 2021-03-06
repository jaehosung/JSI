using JSI.Cmd;
using UnityEngine;
using X;

namespace JSI {
    public class JSIApp : XApp {
        // editor fields
        [SerializeField]
        private string mWebSocketUri = "ws://localhost:751";
        [SerializeField]
        private string mUsername = "Joon Hyub Lee";
        public string getUsername() {
            return this.mUsername;
        }
        [SerializeField]
        private GameObject mVRCameraRig = null;
        [SerializeField]
        private GameObject mVRCenterEyeAnchor = null;
        public GameObject getVRCenterEyeAnchor() {
            return this.mVRCenterEyeAnchor;
        }
        [SerializeField]
        private GameObject mVRLeftHandPrefab = null;
        [SerializeField]
        private GameObject mVRRightHandPrefab = null;

        // fields
        private JSIPerspCameraPerson mPerspCameraPerson = null;
        public JSIPerspCameraPerson getPerspCameraPerson() {
            return this.mPerspCameraPerson;
        }
        private JSIOrthoCameraPerson mOrthoCameraPerson = null;
        public JSIOrthoCameraPerson getOrthoCameraPerson() {
            return this.mOrthoCameraPerson;
        }
        private JSIGrid mGrid = null;
        private XLogMgr mLogMgr = null;
        public override XLogMgr getLogMgr() {
            return this.mLogMgr;
        }
        private JSITouchMarkMgr mTouchMarkMgr = null;
        public JSITouchMarkMgr getTouchMarkMgr() {
            return this.mTouchMarkMgr;
        }
        private JSIPenMarkMgr mPenMarkMgr = null;
        public JSIPenMarkMgr getPenMarkMgr() {
            return this.mPenMarkMgr;
        }
        private JSIPtCurve2DMgr mPtCurve2DMgr = null;
        public JSIPtCurve2DMgr getPtCurve2DMgr() {
            return this.mPtCurve2DMgr;
        }
        private JSIStandingCardMgr mStandingCardMgr = null;
        public JSIStandingCardMgr getStandingCardMgr() {
            return this.mStandingCardMgr;
        }
        private JSISnapshotMgr mSnapshotMgr = null;
        public JSISnapshotMgr getSnapshotMgr() {
            return this.mSnapshotMgr;
        }
        private XScenarioMgr mScenarioMgr = null;
        public override XScenarioMgr getScenarioMgr() {
            return this.mScenarioMgr;
        }

        private JSIKeyEventSource mKeyEventSource = null;
        // private JSIMouseEventSource mMouseEventSource = null;
        private JSIPenEventSource mPenEventSource = null;
        private JSITouchEventSource mTouchEventSource = null;
        private JSIVREventSource mVREventSource = null;
        private JSIEventListener mEventListener = null;
        private JSIDeliveryPerson mDeliveryPerson = null;
        public JSIDeliveryPerson getDeliveryPerson() {
            return this.mDeliveryPerson;
        }

        // private JSICursor2D mCursor = null;
        // public JSICursor2D getCursor() {
        //     return this.mCursor;
        // }
        private JSICursorMgr mCursorMgr = null;
        public JSICursorMgr getCursorMgr() {
            return this.mCursorMgr;
        }
        private JSIHandMgr mHandMgr = null;
        public JSIHandMgr getHandMgr() {
            return this.mHandMgr;
        }

        private void configureUnity() {
            // necessary for manually refreshing collider physics
            Physics2D.simulationMode = SimulationMode2D.Script; // Unity 2020
            // Physics2D.autoSimulation = false; // Unity 2019
            // enable multi-threading for faster physics performance
            Physics2D.jobOptions = new PhysicsJobOptions2D {
                useMultithreading = true };
            // for disabling all the unncessary graphics options
            QualitySettings.SetQualityLevel(0);
            // for maximum refresh rate of pen and touch input
            Application.targetFrameRate = -1; // does not work if VR is connected
            // the only graphics quality setting that should be important
            QualitySettings.antiAliasing = 0;
            // QualitySettings.antiAliasing = 4;
            // enable collision with both sides of the mesh
            Physics.queriesHitBackfaces = true;
        }

        private void configureVR() {
            // change game display to JSI
            this.mVRCenterEyeAnchor.SetActive(false);
            this.mPerspCameraPerson.getCamera().enabled = true;
            this.mOrthoCameraPerson.getCamera().enabled = true;

            // set VR camera background
            Camera vrCamera = this.mVRCenterEyeAnchor.GetComponent<Camera>();
            vrCamera.clearFlags = CameraClearFlags.Color;
            vrCamera.backgroundColor = JSIPerspCameraPerson.BG_COLOR;
        }

        private void Start() {
            this.configureUnity();

            this.mPerspCameraPerson = new JSIPerspCameraPerson();
            this.mOrthoCameraPerson = new JSIOrthoCameraPerson();
            this.mGrid = new JSIGrid();

            this.configureVR();

            // new JSIAppRect3D("TestRect3D", 1f, 2f,
            //    new Color(0.5f, 0f, 0f, 0.5f));
            // new JSIAppCircle3D("TestCircle3D", 1f,
            //    new Color(0.5f, 0f, 0f, 0.5f));
            // new JSIStandingCard("TestStandingCard", 1f, 2f,
            //    new Vector3(0f, 1f, 0f), Quaternion.identity, null);

            this.mPenMarkMgr = new JSIPenMarkMgr();
            this.mPtCurve2DMgr = new JSIPtCurve2DMgr();
            this.mStandingCardMgr = new JSIStandingCardMgr();
            this.mSnapshotMgr = new JSISnapshotMgr(this);
            this.mScenarioMgr = new JSIScenarioMgr(this);
            this.mLogMgr = new XLogMgr();
            this.mLogMgr.setPrintOn(true);

            this.mKeyEventSource = new JSIKeyEventSource();
            // this.mMouseEventSource = new JSIMouseEventSource();
            this.mPenEventSource = new JSIPenEventSource();
            this.mTouchEventSource = new JSITouchEventSource();
            this.mVREventSource = new JSIVREventSource(this);
            this.mEventListener = new JSIEventListener(this);
            // this.mCursor = new JSICursor2D(this);
            this.mTouchMarkMgr = new JSITouchMarkMgr();
            this.mCursorMgr = new JSICursorMgr(this);
            this.mHandMgr = new JSIHandMgr(this.mVRLeftHandPrefab, this.
                mVRRightHandPrefab);

            this.mKeyEventSource.setEventListener(this.mEventListener);
            // this.mMouseEventSource.setEventListener(this.mEventListener);
            this.mPenEventSource.setEventListener(this.mEventListener);
            this.mTouchEventSource.setEventListener(this.mEventListener);
            this.mVREventSource.setEventListener(this.mEventListener);

            // this.mDeliveryPerson = new JSIDeliveryPerson();
            // this.mDeliveryPerson.setEventListener(this.mEventListener);
            // this.mDeliveryPerson.connectTo(this.mWebSocketUri);

            // send hello message to everyone.
            JSICmdToSendHelloMsg.execute(this);
        }
        private void Update() {
            this.mOrthoCameraPerson.update();
            this.mKeyEventSource.update();
            // this.mMouseEventSource.update();
            this.mPenEventSource.update();
            this.mTouchEventSource.update();
            this.mVREventSource.update();
        }

        private void OnApplicationQuit() {
            // send good bye message to everyone.
            JSICmdToSendGoodByeMsg.execute(this);
        }
    }
}