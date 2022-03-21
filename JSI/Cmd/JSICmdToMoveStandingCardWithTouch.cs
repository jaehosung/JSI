using X;
using UnityEngine;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToMoveStandingCardWithTouch : XLoggableCmd {
        // fields
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;

        // private constructor
        private JSICmdToMoveStandingCardWithTouch(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                (JSIEditStandingCardScenario)JSIEditStandingCardScenario.
                getSingleton();
            JSITouchMark tm = scenario.getManipulatingTouchMarks()[0];
            this.mPrevPt = tm.getRecentPt(1);
            this.mCurPt = tm.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToMoveStandingCardWithTouch cmd =
                new JSICmdToMoveStandingCardWithTouch(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSICmdToMoveStandingCardWithPen.moveStandingCard(jsi,
                this.mPrevPt, this.mCurPt);
            return true;
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