using UnityEngine;

namespace JSI.Msg {
    public class JSIMsgToTransformStandingCard : JSIMsg {
        // inner class for content
        public class Content : JSIContent {
            // fields
            public string cardId = string.Empty;
            public Vector3 pos = JSIUtil.VECTOR3_NAN;
            public Quaternion rot = JSIUtil.QUATERNION_NAN;
            public float width = float.NaN;
            public float height = float.NaN;

            // constructor
            public Content(string cardId, Vector3 pos, Quaternion rot,
                float width, float height) {

                this.cardId = cardId;
                this.pos = pos;
                this.rot = rot;
                this.width = width;
                this.height = height;
            }
        }

        // fields
        public Content content = null;

        // constructor
        public JSIMsgToTransformStandingCard(string from, string to,
            string cardId, Vector3 pos, Quaternion rot, float width,
            float height) : base(from, to, JSIMsg.Subject.
            TRANSFORM_STANDING_CARD) {

            this.content = new Content(cardId, pos, rot, width, height);
        }
    }
}