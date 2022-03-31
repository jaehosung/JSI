using JSI.Msg;
using X;

namespace JSI.Cmd {
    public class JSICmdToSendGoodByeMsg : XLoggableCmd {
        // fields
        private string mFrom = string.Empty;
        private string mTo = string.Empty;

        // constructor
        private JSICmdToSendGoodByeMsg(XApp app) : base(app) {
            JSIApp jsi = (JSIApp) this.mApp;
            this.mFrom = jsi.getUsername();
            this.mTo = "Everyone";
        }

        public static bool execute(XApp app) {
            JSICmdToSendGoodByeMsg cmd = new JSICmdToSendGoodByeMsg(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp) this.mApp;

            JSIMsgToSayGoodBye msg = new JSIMsgToSayGoodBye(this.mFrom, this.mTo);
            JSISerializableMsgToSayGoodBye sMsg =
                new JSISerializableMsgToSayGoodBye(msg);
            jsi.getDeliveryPerson().sendSerializableMsg(sMsg);

            return true;
        }

        protected override XJson createLogData() {
            JSIApp jsi = (JSIApp)this.mApp;
            XJson data = new XJson();
            data.addMember("from", this.mFrom);
            data.addMember("to", this.mTo);
            return data;
        }
    }
}