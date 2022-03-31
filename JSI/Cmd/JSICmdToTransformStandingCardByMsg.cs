using JSI.Geom;
using JSI.Msg;
using X;

namespace JSI.Cmd {
    public class JSICmdToTransformStandingCardByMsg : XLoggableCmd {
        // fields
        private JSIMsgToTransformStandingCard mMsg = null;

        // constructor
        private JSICmdToTransformStandingCardByMsg(XApp app, JSIMsg msg) :
            base(app) {

            this.mMsg = (JSIMsgToTransformStandingCard) msg;
        }

        // static method to construct and execute this method
        public static bool execute(XApp app, JSIMsg msg) {
            JSICmdToTransformStandingCardByMsg cmd =
                new JSICmdToTransformStandingCardByMsg(app, msg);
            return cmd.execute();
        }

        // private constructor
        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp) this.mApp;
            JSIStandingCard standingCard = jsi.getStandingCardMgr().findById(
                mMsg.content.cardId);

            if (standingCard == null) {
                return false;
            }

            if (mMsg.content.pos != standingCard.getGameObject().transform.
                position) {

                standingCard.getGameObject().transform.position =
                    mMsg.content.pos;
            }
            if (mMsg.content.rot !=  standingCard.getGameObject().transform.
                rotation) {

                standingCard.getGameObject().transform.rotation =
                    mMsg.content.rot;
            }

            // TODO: use only width to compare size?
            JSIRect3D rect = (JSIRect3D)standingCard.getCard().getGeom();
            float width = rect.getWidth();
            if (mMsg.content.width != width) {
                float scaleFactor = mMsg.content.width / width;
                JSICmdToScaleStandingCardWithPen.scaleStandingCardByScaleFactor(
                    jsi, standingCard, scaleFactor);
            }

            return true;
        }

        protected override XJson createLogData() {
            JSIApp jsi = (JSIApp)this.mApp;
            XJson data = new XJson();
            data.addMember("cardId", this.mMsg.content.cardId);
            data.addMember("pos", this.mMsg.content.pos);
            data.addMember("rot", this.mMsg.content.rot);
            data.addMember("width", this.mMsg.content.width);
            data.addMember("height", this.mMsg.content.height);
            return data;
        }
    }
}