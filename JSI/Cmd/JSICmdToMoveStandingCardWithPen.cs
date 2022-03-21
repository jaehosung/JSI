using X;
using UnityEngine;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToMoveStandingCardWithPen : XLoggableCmd {
        // fields
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;

        // private constructor
        private JSICmdToMoveStandingCardWithPen(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIPenMark penMark = jsi.getPenMarkMgr().getLastPenMark();
            this.mPrevPt = penMark.getRecentPt(1);
            this.mCurPt = penMark.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToMoveStandingCardWithPen cmd =
                new JSICmdToMoveStandingCardWithPen(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSICmdToMoveStandingCardWithPen.moveStandingCard(jsi, this.mPrevPt,
                this.mCurPt);
            return true;
        }

        public static void moveStandingCard(JSIApp jsi, Vector2 prevPt,
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

            // calculate the position difference between the two points.
            Vector3 diff = curPtOnPlane - prevPtOnPlane;

            // update the position of the selected standing card.
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();
            JSIStandingCard standingCardToMove =
                scenario.getSelectedStandingCard();
            standingCardToMove.getGameObject().transform.position += diff;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            data.addMember("prevPt", this.mPrevPt);
            data.addMember("curPt", this.mCurPt);
            JSIStandingCard sc = JSIEditStandingCardScenario.getSingleton().
                getSelectedStandingCard();
            data.addMember("cardId", sc.getId());
            data.addMember("cardPos", sc.getGameObject().transform.position);
            return data;
        }
    }
}