using System;
using JSI.File;

namespace JSI.Msg {
    [Serializable]
    public class JSISerializableMsgToSayGoodBye : JSISerializableMsg {
        // inner class for serializable content
        [Serializable]
        public class SerializableContent : JSISerializableContent {
            // constructor
            public SerializableContent(JSIMsgToSayGoodBye.Content content) {
            }

            // methods
            public override JSIContent toContent() {
                return new JSIMsgToSayGoodBye.Content();
            }
        }

        // fields
        public SerializableContent content = null;

        // constructor
        public JSISerializableMsgToSayGoodBye(JSIMsgToSayGoodBye msg) :
            base(msg) {

            this.content = new SerializableContent(msg.content);
        }

        // methods
        public override JSIMsg toMsg() {
            JSIMsgToSayGoodBye.Content content =
                (JSIMsgToSayGoodBye.Content) this.content.toContent();
            return new JSIMsgToSayGoodBye(this.from, this.to);
        }
    }
}