namespace JSI.Msg {
    public abstract class JSIMsg {
        // constants
        public enum Subject {
            DO_SOMETHING,
            //
            HELLO,
            ADD_STANDING_CARD,
            TRANSFORM_STANDING_CARD,
            GOOD_BYE,
        }

        // fields
        public string from;
        public string to;
        public Subject subject;

        // constructor
        public JSIMsg(string from, string to, Subject subject) {
            this.from = from;
            this.to = to;
            this.subject = subject;
        }
    }
}