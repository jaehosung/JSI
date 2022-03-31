namespace JSI.Msg {
    public class JSIMsgToSayHello : JSIMsg {
        // inner class for content
        public class Content : JSIContent {
            // constructor
            public Content() {
            }
        }

        // fields
        public Content content = null;

        // constructor
        public JSIMsgToSayHello(string from, string to) : base(from, to,
            JSIMsg.Subject.HELLO) {

            this.content = new Content();
        }
    }
}