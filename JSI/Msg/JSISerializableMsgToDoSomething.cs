using System;

namespace JSI.Msg {
    [Serializable]
    public class JSISerializableMsgToDoSomething : JSISerializableMsg {
        // inner class for serializable content
        [Serializable]
        public class SerializableContent : JSISerializableContent {
            // fields

            // constructor
            public SerializableContent(JSIMsgToDoSomething.Content content) {
            }

            // methods
            public override JSIContent toContent() {
                return new JSIMsgToDoSomething.Content();
            }
        }

        // fields
        public SerializableContent content = null;

        // constructor
        public JSISerializableMsgToDoSomething(JSIMsgToDoSomething msg) :
            base(msg) {

            this.content = new SerializableContent(msg.content);
        }

        // methods
        public override JSIMsg toMsg() {
            JSIMsgToDoSomething.Content content =
                (JSIMsgToDoSomething.Content) this.content.toContent();
            return new JSIMsgToDoSomething(this.from, this.to);
        }
    }
}