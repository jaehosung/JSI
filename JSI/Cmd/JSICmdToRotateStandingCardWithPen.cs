using X;
using UnityEngine;
using JSI.Scenario;
using JSI.AppObject;

namespace JSI.Cmd {
    public class JSICmdToRotateStandingCardWithPen : XLoggableCmd {
        // fields
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;

        // private constructor
        private JSICmdToRotateStandingCardWithPen(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIPenMark penMark = jsi.getPenMarkMgr().getLastPenMark();
            this.mPrevPt = penMark.getRecentPt(1);
            this.mCurPt = penMark.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToRotateStandingCardWithPen cmd =
                new JSICmdToRotateStandingCardWithPen(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSICmdToRotateStandingCardWithPen.rotateStandingCard(jsi,
                this.mPrevPt, this.mCurPt);
            return true;
        }

        public static void rotateStandingCard(JSIApp jsi, Vector2 prevPt,
            Vector2 curPt) {

            JSIPerspCameraPerson cp = jsi.getPerspCameraPerson();

            // create the ground plane.
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            // project the previous screen point to the plane.
            Ray prevPtRay = cp.getCamera().ScreenPointToRay(prevPt);
            float prevPtDist = float.NaN;
            groundPlane.Raycast(prevPtRay, out prevPtDist);
            Vector3 prevPtOnPlane = prevPtRay.GetPoint(prevPtDist);

            // project the current screen point to the plane.
            Ray curPtRay = cp.getCamera().ScreenPointToRay(curPt);
            float curPtDist = float.NaN;
            groundPlane.Raycast(curPtRay, out curPtDist);
            Vector3 curPtOnPlane = curPtRay.GetPoint(curPtDist);

            // calculate rotation
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();
            JSIStandingCard standingCardToRotate =
                scenario.getSelectedStandingCard();
            JSIAppCircle3D stand = standingCardToRotate.getStand();
            Vector3 standCtr = stand.getGameObject().transform.position;

            Quaternion prevRot = Quaternion.LookRotation(Vector3.up,
                prevPtOnPlane - standCtr);
            Quaternion curRot = Quaternion.LookRotation(Vector3.up,
                curPtOnPlane - standCtr);
            // curRot = delRot * prevRot
            // curRot * Inverse(prevRot) = delRot * prevRot * Inverse(prevRot)
            Quaternion delRot = curRot * Quaternion.Inverse(prevRot);

            // update the rotation of the selected standing card.
            standingCardToRotate.getGameObject().transform.rotation =
                delRot *
                standingCardToRotate.getGameObject().transform.rotation;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            data.addMember("prevPt", this.mPrevPt);
            data.addMember("curPt", this.mCurPt);
            JSIStandingCard sc = JSIEditStandingCardScenario.getSingleton().
                getSelectedStandingCard();
            data.addMember("cardId", sc.getId());
            data.addMember("cardRot", sc.getGameObject().transform.rotation);
            return data;
        }
    }
}