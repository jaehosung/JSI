using System.Linq;
using System.Text;
using JSI.Msg;
using X;

namespace JSI.Cmd {
    public class JSICmdToSendAddStandingCardMsg : XLoggableCmd {
        // fields
        private string mFrom = string.Empty;
        private string mTo = string.Empty;
        private JSIStandingCard mSc = null;

        // constructor
        private JSICmdToSendAddStandingCardMsg(XApp app) : base(app) {

            JSIApp jsi = (JSIApp) this.mApp;
            this.mFrom = jsi.getUsername();
            this.mTo = "Everyone";
            this.mSc = jsi.getStandingCardMgr().getStandingCards().Last();
        }

        public static bool execute(XApp app) {
            JSICmdToSendAddStandingCardMsg cmd =
                new JSICmdToSendAddStandingCardMsg(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp) this.mApp;

            JSIMsgToAddStandingCard msg = new JSIMsgToAddStandingCard(
                jsi.getUsername(), this.mTo, this.mSc);
            JSISerializableMsgToAddStandingCard sMsg =
               new JSISerializableMsgToAddStandingCard(msg);
            jsi.getDeliveryPerson().sendSerializableMsg(sMsg);

            return true;
        }

        protected override XJson createLogData() {
            JSIApp jsi = (JSIApp)this.mApp;
            XJson data = new XJson();
            data.addMember("from", this.mFrom);
            data.addMember("to", this.mTo);
            data.addMember("cardId", this.mSc.getId());
            return data;
        }
    }
}