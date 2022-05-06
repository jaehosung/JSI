using X;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToSelectStandingCardByHand : XLoggableCmd {
        // fields
        private JSIStandingCard mSelectedStandingCard = null;
        private JSIStandingCard mSc = null;
        private JSIHand mHand = null;

        // private constructor
        private JSICmdToSelectStandingCardByHand(XApp app,
            JSIStandingCard sc, JSIHand hand) : base(app) {

            JSIApp jsi = (JSIApp)this.mApp;
            this.mSc = sc;
            this.mHand = hand;
        }

        // static method to construct and execute this command
        public static bool execute(XApp app, JSIStandingCard sc, JSIHand hand) {
            JSICmdToSelectStandingCardByHand cmd =
                new JSICmdToSelectStandingCardByHand(app, sc, hand);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();
            this.mSelectedStandingCard = this.mSc;
            scenario.mInitPinchPoint = this.mHand.calcPinchPos();
            scenario.mInitStandingCardPos = this.mSc.getGameObject().transform.position;

            
            return true;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            JSIStandingCard sc = JSIEditStandingCardScenario.getSingleton().
                getSelectedStandingCard();
            data.addMember("cardId", sc.getId());
            return data;
        }
    }
}