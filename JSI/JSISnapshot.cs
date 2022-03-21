using System.Collections.Generic;
using JSI.File;

namespace JSI {
    public class JSISnapshot {
        // fields
        private JSISnapshot mPrevSnapshot = null;
        public JSISnapshot getPrevSnapshot() {
            return this.mPrevSnapshot;
        }
        public void setPrevSnapshot(JSISnapshot prevSnapshot) {
            this.mPrevSnapshot = prevSnapshot;
        }
        private JSISnapshot mNextSnapshot = null;
        public JSISnapshot getNextSnapshot() {
            return this.mNextSnapshot;
        }
        public void setNextSnapshot(JSISnapshot nextSnapshot) {
            this.mNextSnapshot = nextSnapshot;
        }
        private List<JSISerializableStandingCard> mSerializableStandingCards;
        public List<JSISerializableStandingCard> getSerializableStandingCards() {
            return this.mSerializableStandingCards;
        }

        // constructor
        public JSISnapshot(List<JSISerializableStandingCard> standingCards) {
            this.mSerializableStandingCards = standingCards;
        }

        // methods
        private bool areStandingCardsSame(JSISerializableStandingCard sSc1,
            JSISerializableStandingCard sSc2) {

            return sSc1.id == sSc2.id && sSc1.width == sSc2.width && sSc1.height ==
                sSc2.height && sSc1.pos == sSc2.pos && sSc1.rot == sSc2.rot;
        }
        public bool containsStandingCard(
            JSISerializableStandingCard standingCard) {

            foreach (JSISerializableStandingCard sSc in
                this.mSerializableStandingCards) {

                if (this.areStandingCardsSame(standingCard, sSc)) {
                    return true;
                }
            }
            return false;
        }
    }
}