using X;
using UnityEngine;
using JSI.Scenario;
using JSI.Geom;

namespace JSI.Cmd {
    public class JSICmdToScaleStandingCardWithTouch : XLoggableCmd {
        // fields
        private Vector2 mPrevPt = Vector2.zero;
        private Vector2 mCurPt = Vector2.zero;

        // private constructor
        private JSICmdToScaleStandingCardWithTouch(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                (JSIEditStandingCardScenario)JSIEditStandingCardScenario.
                getSingleton();
            JSITouchMark tm = scenario.getManipulatingTouchMarks()[0];
            this.mPrevPt = tm.getRecentPt(1);
            this.mCurPt = tm.getRecentPt(0);
        }

        // static method to construct and execute this command
        public static bool execute(XApp app) {
            JSICmdToScaleStandingCardWithTouch cmd =
                new JSICmdToScaleStandingCardWithTouch(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario = JSIEditStandingCardScenario.
                getSingleton();
            JSIStandingCard standingCard = scenario.getSelectedStandingCard();

            float scaleFactor = JSICmdToScaleStandingCardWithPen.calcScaleFactor(
                jsi, standingCard, this.mPrevPt, this.mCurPt);
            JSICmdToScaleStandingCardWithPen.scaleStandingCardByScaleFactor(jsi,
                standingCard, scaleFactor);
            return true;
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            data.addMember("prevPt", this.mPrevPt);
            data.addMember("curPt", this.mCurPt);
            JSIStandingCard sc = JSIEditStandingCardScenario.getSingleton().
                getSelectedStandingCard();
            JSIRect3D rect = (JSIRect3D)sc.getCard().getGeom();
            float w = rect.getWidth();
            float h = rect.getHeight();
            data.addMember("cardId", sc.getId());
            data.addMember("cardWidth", w);
            data.addMember("cardHeight", h);
            return data;
        }
    }
}