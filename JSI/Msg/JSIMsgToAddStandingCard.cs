namespace JSI.Msg {
    public class JSIMsgToAddStandingCard : JSIMsg {
        // inner class for content
        public class Content : JSIContent {
            // fields
            public JSIStandingCard standingCard = null;

            // constructor
            public Content(JSIStandingCard sc) {
                this.standingCard = sc;
            }
        }

        // fields
        public Content content = null;

        // constructor
        public JSIMsgToAddStandingCard(string from, string to,
            JSIStandingCard sc) : base(from, to,
            JSIMsg.Subject.ADD_STANDING_CARD) {

            this.content = new Content(sc);
        }
    }
}