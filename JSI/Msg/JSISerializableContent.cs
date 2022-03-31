using System;

namespace JSI.Msg {
    [Serializable]
    public abstract class JSISerializableContent {
        public abstract JSIContent toContent();
    }
}