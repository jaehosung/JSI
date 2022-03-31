using System.Text;
using JSI.Msg;
using X;

namespace JSI.Cmd {
    public class JSICmdToAddStandingCardByMsg : XLoggableCmd {
        //fields
        private JSIMsgToAddStandingCard mMsg = null;

        // constructor
        private JSICmdToAddStandingCardByMsg(XApp app, JSIMsg msg) : base(app) {
            this.mMsg = (JSIMsgToAddStandingCard) msg;
        }

        public static bool execute(XApp app, JSIMsg msg) {
            JSICmdToAddStandingCardByMsg cmd = new JSICmdToAddStandingCardByMsg(
                app, msg);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            jsi.getStandingCardMgr().getStandingCards().Add(this.mMsg.content.
                standingCard);
            return true;
        }

        protected override XJson createLogData() {
            JSIApp jsi = (JSIApp)this.mApp;
            XJson data = new XJson();
            data.addMember("cardId", this.mMsg.content.standingCard.getId());
            return data;
        }
    }
}