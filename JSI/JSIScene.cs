using JSI.Cmd;
using JSI.Msg;
using UnityEngine;
using UnityEngine.InputSystem;
using X;

namespace JSI {
    public abstract class JSIScene : XScene {
        // constructor
        protected JSIScene(XScenario scenario) : base(scenario) {
        }

        // methods
        public abstract void handleKeyDown(Key k);
        public abstract void handleKeyUp(Key k);
        public abstract void handlePenDown(Vector2 pt);
        public abstract void handlePenDrag(Vector2 pt);
        public abstract void handlePenUp(Vector2 pt);
        public abstract void handleEraserDown(Vector2 pt);
        public abstract void handleEraserUp(Vector2 pt);
        public abstract void handleEraserDrag(Vector2 pt);
        public abstract void handleTouchDown();
        public abstract void handleTouchDrag();
        public abstract void handleTouchUp();
        public void handleVRHeadsetMount() {
            // change game display to VR
            JSIApp jsi = (JSIApp)this.mScenario.getApp();
            jsi.getVRCenterEyeAnchor().SetActive(true);
            jsi.getPerspCameraPerson().getCamera().enabled = false;
            jsi.getOrthoCameraPerson().getCamera().enabled = false;
        }
        public void handleVRHeadsetUnmount() {
            // change game display to JSI
            JSIApp jsi = (JSIApp)this.mScenario.getApp();
            jsi.getVRCenterEyeAnchor().SetActive(false);
            jsi.getPerspCameraPerson().getCamera().enabled = true;
            jsi.getOrthoCameraPerson().getCamera().enabled = true;
        }

        // msg
        public void handleMsg(JSIMsg msg) {
            JSIApp jsi = (JSIApp)this.mScenario.getApp();

            switch(msg.subject) {
                case JSIMsg.Subject.HELLO:
                    break;
                case JSIMsg.Subject.ADD_STANDING_CARD:
                    JSICmdToAddStandingCardByMsg.execute(jsi, msg);
                    break;
                case JSIMsg.Subject.TRANSFORM_STANDING_CARD:
                    JSICmdToTransformStandingCardByMsg.execute(jsi, msg);
                    break;
                case JSIMsg.Subject.GOOD_BYE:
                    break;
            }
        }
    }
}