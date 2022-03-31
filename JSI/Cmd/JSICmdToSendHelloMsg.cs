using JSI.Msg;
using X;

namespace JSI.Cmd {
    public class JSICmdToSendHelloMsg : XLoggableCmd {
        // fields
        private string mFrom = string.Empty;
        private string mTo = string.Empty;

        // constructor
        private JSICmdToSendHelloMsg(XApp app) : base(app) {
            JSIApp jsi = (JSIApp) this.mApp;
            this.mFrom = jsi.getUsername();
            this.mTo = "Everyone";
        }

        public static bool execute(XApp app) {
            JSICmdToSendHelloMsg cmd = new JSICmdToSendHelloMsg(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp) this.mApp;

            JSIMsgToSayHello msg = new JSIMsgToSayHello(this.mFrom, this.mTo);
            JSISerializableMsgToSayHello sMsg = new JSISerializableMsgToSayHello(
                msg);
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