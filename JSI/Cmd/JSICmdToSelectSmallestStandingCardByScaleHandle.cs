using X;
using JSI.Scenario;

namespace JSI.Cmd {
    public class JSICmdToSelectSmallestStandingCardByScaleHandle : XLoggableCmd {
        // fields
        private JSIStandingCard mSelectedStandingCard = null;
        private JSICursor2D mCursor = null;

        // private constructor
        private JSICmdToSelectSmallestStandingCardByScaleHandle(XApp app,
            JSICursor2D cursor) : base(app) {

            JSIApp jsi = (JSIApp)this.mApp;
            this.mCursor = cursor;
        }

        // static method to construct and execute this command
        public static bool execute(XApp app, JSICursor2D cursor) {
            JSICmdToSelectSmallestStandingCardByScaleHandle cmd =
                new JSICmdToSelectSmallestStandingCardByScaleHandle(app, cursor);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();
            this.mSelectedStandingCard =
                scenario.selectStandingCardByScaleHandle(this.mCursor);
            scenario.setSelectedStandingCard(this.mSelectedStandingCard);
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