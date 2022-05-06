using X;
using UnityEngine;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToMoveStandingCardWithHand : XLoggableCmd {
        // fields
        private Vector3 mPrevPt = Vector3.zero;
        private Vector3 mCurPt = Vector3.zero;
        private JSIHand mHand = null;

        // private constructor
        private JSICmdToMoveStandingCardWithHand(XApp app, JSIHand hand) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            this.mHand = hand;
            // this.mPrevPt = null;  //pinch point
            // this.mCurPt = penMark.getRecentPt(0);
            
        }

        // static method to construct and execute this command
        public static bool execute(XApp app, JSIHand hand) {
            JSICmdToMoveStandingCardWithHand cmd =
                new JSICmdToMoveStandingCardWithHand(app, hand);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSICmdToMoveStandingCardWithHand.moveStandingCard(jsi, this.mHand);
            return true;
        }

        public static void moveStandingCard(JSIApp jsi, JSIHand hand) {

            
            // update the position of the selected standing card.
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();

            Vector3 initPinchPoint =  scenario.mInitPinchPoint;
            Vector3 initStandingCardPos = scenario.mInitStandingCardPos;
            Vector3 diff = hand.calcPinchPos()-initPinchPoint;
            diff.y = 0;
            Vector3 pos = initStandingCardPos + diff;
            
            JSIStandingCard standingCardToMove =
                scenario.getSelectedStandingCard();
            standingCardToMove.getGameObject().transform.position = pos;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            JSIStandingCard sc = JSIEditStandingCardScenario.getSingleton().
                getSelectedStandingCard();
            data.addMember("cardId", sc.getId());
            data.addMember("cardPos", sc.getGameObject().transform.position);
            return data;
        }
    }
}