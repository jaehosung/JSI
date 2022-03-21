using System.Collections.Generic;
using System.Linq;
using JSI.File;

/*
Implementation
- each snapshot has the list of all standing cards visible at that momement
- each snapshot has the reference of the previous & next snapshot (linked list)
                                current
                                   v
o <--> o <--> o <--> o <--> o <--> o
- by performing undo, current snapshot pointer moves to previous snapshot
- by performing redo, current snapshot pointer moves to next snapshot
- when snapshot moves from one snapshot to another (e.g. A --> B),
  the delta between two snapshots can be found,
  and appropriate add & remove commands are executed
           current
              v
o <--> o <--> o <--> o <--> o <--> o
              B                    A
- when the user makes changes (add or remove standing cards) after undoing,
  a new snapshot is made, and that snapshot is made current snapshot
                  current
                     v
                /--> o
               /
o <--> o <--> o <--> o <--> o <--> o
*/
namespace JSI {
    public class JSISnapshotMgr {
        // fields
        private JSIApp mJSI;
        private JSISnapshot mCurSnapshot;
        public JSISnapshot getCurSnapshot() {
            return this.mCurSnapshot;
        }

        // constructor
        public JSISnapshotMgr(JSIApp jsi) {
            this.mJSI = jsi;
            this.mCurSnapshot = new JSISnapshot(
                new List<JSISerializableStandingCard>());
        }

        // methods
        private bool areSnapshotsSame(JSISnapshot s1, JSISnapshot s2) {
            foreach (JSISerializableStandingCard sSc in
                s1.getSerializableStandingCards()) {

                if (!s2.containsStandingCard(sSc)) {
                    return false;
                }
            }
            foreach (JSISerializableStandingCard sSc in
                s2.getSerializableStandingCards()) {

                if (!s1.containsStandingCard(sSc)) {
                    return false;
                }
            }
            return false;
        }

        public bool takeSnapshot() {
            // make a new snapshot
            List<JSISerializableStandingCard> sStandingCards =
                new List<JSISerializableStandingCard>();
            foreach (JSIStandingCard sc in this.mJSI.getStandingCardMgr().
                getStandingCards()) {

                sStandingCards.Add(new JSISerializableStandingCard(sc));
            }
            JSISnapshot nextSnapshot = new JSISnapshot(sStandingCards);

            // register this snapshot only if it is any different from
            // the previous snapshot
            if (!this.areSnapshotsSame(this.mCurSnapshot, nextSnapshot)) {
                this.mCurSnapshot.setNextSnapshot(nextSnapshot);
                nextSnapshot.setPrevSnapshot(this.mCurSnapshot);
                this.mCurSnapshot = nextSnapshot;
                return true;
            } else {
                return false;
            }
        }

        public bool undo() {
            // if next snapshot exists
            // go from current snapshot to next snapshot
            if (this.mCurSnapshot.getPrevSnapshot() == null) {
                return false;
            }

            this.applyDiff(this.mCurSnapshot, this.mCurSnapshot.
                getPrevSnapshot());
            this.mCurSnapshot = this.mCurSnapshot.getPrevSnapshot();
            return true;
        }

        public bool redo() {
            // if next snapshot exists
            // go from current snapshot to next snapshot
            if (this.mCurSnapshot.getNextSnapshot() == null) {
                return false;
            }

            this.applyDiff(this.mCurSnapshot, this.mCurSnapshot.
                getNextSnapshot());
            this.mCurSnapshot = this.mCurSnapshot.getNextSnapshot();
            return true;
        }

        private void applyDiff(JSISnapshot fromSnapshot, JSISnapshot toSnapshot) {
            // if "from snapshot" has cards that are not in "to snapshot",
            // remove them
            foreach (JSISerializableStandingCard sSc in fromSnapshot.
                getSerializableStandingCards()) {

                if (!toSnapshot.containsStandingCard(sSc)) {
                    JSIStandingCard sc = this.mJSI.getStandingCardMgr().findById(
                        sSc.id);
                    this.mJSI.getStandingCardMgr().getStandingCards().Remove(sc);
                    sc.destroyGameObject();
                }
            }
            // if "to snapshot" has cards that are not in "from snapshot",
            // add them
            foreach (JSISerializableStandingCard sSc in toSnapshot.
                getSerializableStandingCards()) {

                if (!fromSnapshot.containsStandingCard(sSc)) {
                    this.mJSI.getStandingCardMgr().getStandingCards().Add(sSc.
                        toStandingCard());
                }
            }
        }
    }
}