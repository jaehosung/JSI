using X;
using UnityEngine;
using JSI.Scenario;
using JSI.AppObject;

namespace JSI.Cmd {
    public class JSICmdToMoveNRotateStandingCardWithTouch : XLoggableCmd {
        // fields
        private Vector2 mPrevPt1 = JSIUtil.VECTOR2_NAN;
        private Vector2 mCurPt1 = JSIUtil.VECTOR2_NAN;
        private Vector2 mPrevPt2 = JSIUtil.VECTOR2_NAN;
        private Vector2 mCurPt2 = JSIUtil.VECTOR2_NAN;

        // private constructor
        private JSICmdToMoveNRotateStandingCardWithTouch(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                (JSIEditStandingCardScenario)JSIEditStandingCardScenario.
                getSingleton();
            JSITouchMark tm1 = scenario.getManipulatingTouchMarks()[0];
            if (tm1.getPts().Count > 1) {
                this.mPrevPt1 = tm1.getRecentPt(1);
            } else {
                this.mPrevPt1 = tm1.getRecentPt(0);
            }
            this.mCurPt1 = tm1.getRecentPt(0);
            JSITouchMark tm2 = scenario.getManipulatingTouchMarks()[1];
            if (tm2.getPts().Count > 1) {
                this.mPrevPt2 = tm2.getRecentPt(1);
            } else {
                this.mPrevPt2 = tm2.getRecentPt(0);
            }
            this.mCurPt2 = tm2.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToMoveNRotateStandingCardWithTouch cmd =
                new JSICmdToMoveNRotateStandingCardWithTouch(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario = JSIEditStandingCardScenario.
                getSingleton();
            JSIPerspCameraPerson cp = jsi.getPerspCameraPerson();

            // create the ground plane.
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            // project the previous screen point to the plane.
            Ray prevPt1Ray = cp.getCamera().ScreenPointToRay(this.mPrevPt1);
            float prevPt1Dist = float.NaN;
            groundPlane.Raycast(prevPt1Ray, out prevPt1Dist);
            Vector3 prevPt1OnPlane = prevPt1Ray.GetPoint(prevPt1Dist);

            Ray prevPt2Ray = cp.getCamera().ScreenPointToRay(this.mPrevPt2);
            float prevPt2Dist = float.NaN;
            groundPlane.Raycast(prevPt2Ray, out prevPt2Dist);
            Vector3 prevPt2OnPlane = prevPt2Ray.GetPoint(prevPt2Dist);

            // project the current screen point to the plane.
            Ray curPt1Ray = cp.getCamera().ScreenPointToRay(this.mCurPt1);
            float curPt1Dist = float.NaN;
            groundPlane.Raycast(curPt1Ray, out curPt1Dist);
            Vector3 curPt1OnPlane = curPt1Ray.GetPoint(curPt1Dist);

            Ray curPt2Ray = cp.getCamera().ScreenPointToRay(this.mCurPt2);
            float curPt2Dist = float.NaN;
            groundPlane.Raycast(curPt2Ray, out curPt2Dist);
            Vector3 curPt2OnPlane = curPt2Ray.GetPoint(curPt2Dist);

            // get previous position & rotation.

            JSIStandingCard sc = scenario.getSelectedStandingCard();
            Vector3 prevPos = sc.getGameObject().transform.position;
            Quaternion prevRot = sc.getGameObject().transform.rotation;

            // calculate rotation.
            Vector3 prevDirOnPlane = (prevPt2OnPlane - prevPt1OnPlane).normalized;
            Vector3 curDirOnPlane = (curPt2OnPlane - curPt1OnPlane).normalized;
            Quaternion delRot = Quaternion.FromToRotation(prevDirOnPlane,
                curDirOnPlane);
            Quaternion curRot = delRot * prevRot;

            // calculate position.
            Vector3 prevMidPtOnPlane = 0.5f * (prevPt1OnPlane + prevPt2OnPlane);
            Vector3 curMidPtOnPlane = 0.5f * (curPt1OnPlane + curPt2OnPlane);
            Vector3 prevOffset = prevPos - prevMidPtOnPlane;
            Vector3 curOffset = delRot * prevOffset;
            Vector3 curPos = curMidPtOnPlane + curOffset;

            // update the translation & rotation of the selected standing card.
            sc.getGameObject().transform.position = curPos;
            sc.getGameObject().transform.rotation = curRot;

            return true;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            data.addMember("prevPt1", this.mPrevPt1);
            data.addMember("curPt1", this.mCurPt1);
            data.addMember("prevPt2", this.mPrevPt2);
            data.addMember("curPt2", this.mCurPt2);
            JSIStandingCard sc = JSIEditStandingCardScenario.getSingleton().
                getSelectedStandingCard();
            data.addMember("cardId", sc.getId());
            data.addMember("cardPos", sc.getGameObject().transform.position);
            data.addMember("cardRot", sc.getGameObject().transform.rotation);
            return data;
        }
    }
}