namespace JSI.Msg {
    public class JSIMsgToSayGoodBye : JSIMsg {
        // inner class for content
        public class Content : JSIContent {
            // constructor
            public Content() {
            }
        }

        // fields
        public Content content = null;

        // constructor
        public JSIMsgToSayGoodBye(string from, string to) : base(from, to,
            JSIMsg.Subject.GOOD_BYE) {

            this.content = new Content();
        }
    }
}