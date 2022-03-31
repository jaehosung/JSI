using System;
using JSI.File;

namespace JSI.Msg {
    [Serializable]
    public class JSISerializableMsgToTransformStandingCard : JSISerializableMsg {
        // inner class for serializable content
        [Serializable]
        public class SerializableContent : JSISerializableContent {
            // fields
            public string cardId = string.Empty;
            public JSISerializableVector3 pos = null;
            public JSISerializableQuaternion rot = null;
            public float width = float.NaN;
            public float height = float.NaN;

            // constructor
            public SerializableContent(
                JSIMsgToTransformStandingCard.Content content) {

                this.cardId = content.cardId;
                this.pos = new JSISerializableVector3(content.pos);
                this.rot = new JSISerializableQuaternion(content.rot);
                this.width = content.width;
                this.height = content.height;
            }

            // methods
            public override JSIContent toContent() {
                return new JSIMsgToTransformStandingCard.Content(this.cardId,
                    this.pos.toVector3(), this.rot.toQuaternion(),
                    this.width, this.height);
            }
        }

        // fields
        public SerializableContent content = null;

        // constructor
        public JSISerializableMsgToTransformStandingCard(
            JSIMsgToTransformStandingCard msg) : base(msg) {

            this.content = new SerializableContent(msg.content);
        }

        // methods
        public override JSIMsg toMsg() {
            JSIMsgToTransformStandingCard.Content content =
                (JSIMsgToTransformStandingCard.Content) this.content.toContent();

            return new JSIMsgToTransformStandingCard(
                this.from, this.to, content.cardId, content.pos, content.rot,
                content.width, content.height);
        }
    }
}