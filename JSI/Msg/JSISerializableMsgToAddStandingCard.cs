using System;
using JSI.File;

namespace JSI.Msg {
    [Serializable]
    public class JSISerializableMsgToAddStandingCard : JSISerializableMsg {
        // inner class for serializable content
        [Serializable]
        public class SerializableContent : JSISerializableContent {
            // fields
            public JSISerializableStandingCard standingCard = null;

            // constructor
            public SerializableContent(JSIMsgToAddStandingCard.Content content) {
                this.standingCard = new JSISerializableStandingCard(
                    content.standingCard);
            }

            // methods
            public override JSIContent toContent() {
                return new JSIMsgToAddStandingCard.Content(this.standingCard.
                    toStandingCard());
            }
        }

        // fields
        public SerializableContent content = null;

        // constructor
        public JSISerializableMsgToAddStandingCard(JSIMsgToAddStandingCard msg) :
            base(msg) {

            this.content = new SerializableContent(msg.content);
        }

        // methods
        public override JSIMsg toMsg() {
            JSIMsgToAddStandingCard.Content content =
                (JSIMsgToAddStandingCard.Content) this.content.toContent();
            return new JSIMsgToAddStandingCard(this.from, this.to, content.
                standingCard);
        }
    }
}