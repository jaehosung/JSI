using System;
using JSI.File;

namespace JSI.Msg {
    [Serializable]
    public class JSISerializableMsgToSayHello : JSISerializableMsg {
        // inner class for serializable content
        [Serializable]
        public class SerializableContent : JSISerializableContent {
            // constructor
            public SerializableContent(JSIMsgToSayHello.Content content) {
            }

            // methods
            public override JSIContent toContent() {
                return new JSIMsgToSayHello.Content();
            }
        }

        // fields
        public SerializableContent content = null;

        // constructor
        public JSISerializableMsgToSayHello(JSIMsgToSayHello msg) :
            base(msg) {

            this.content = new SerializableContent(msg.content);
        }

        // methods
        public override JSIMsg toMsg() {
            JSIMsgToSayHello.Content content =
                (JSIMsgToSayHello.Content) this.content.toContent();
            return new JSIMsgToSayHello(this.from, this.to);
        }
    }
}