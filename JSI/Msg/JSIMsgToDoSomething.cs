namespace JSI.Msg {
    public class JSIMsgToDoSomething : JSIMsg {
        // inner class for content
        public class Content : JSIContent {
            // fields

            // constructor
            public Content() {
            }
        }

        // fields
        public Content content = null;

        // constructor
        public JSIMsgToDoSomething(string from, string to) : base(from, to,
            JSIMsg.Subject.DO_SOMETHING) {

            this.content = new Content();
        }
    }
}