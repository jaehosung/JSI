using System;

namespace JSI.Msg {
    [Serializable]
    public abstract class JSISerializableMsg {
        // fields
        public string from = string.Empty;
        public string to = string.Empty;
        public string subject = string.Empty;

        // constructor
        public JSISerializableMsg(JSIMsg msg) {
            this.from = msg.from.ToString();
            this.to = msg.to.ToString();
            this.subject = msg.subject.ToString();
        }

        // abstract methods
        public abstract JSIMsg toMsg();
    }
}