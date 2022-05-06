using System;
using System.Text;
using JSI.Geom;
using JSI.Msg;
using JSI.Scenario;
using UnityEngine;
using X;

namespace JSI.Cmd {
    public class JSICmdToSendTransformStandingCardMsg : XLoggableCmd {
        // fields
        private string mFrom = string.Empty;
        private string mTo = string.Empty;
        private string mCardId = string.Empty;
        private Vector3 mPos = JSIUtil.VECTOR3_NAN;
        private Quaternion mRot = JSIUtil.QUATERNION_NAN;
        private float mWidth = float.NaN;
        private float mHeight = float.NaN;

        // constructor
        private JSICmdToSendTransformStandingCardMsg(XApp app) : base(app) {
            JSIApp jsi = (JSIApp)this.mApp;
            JSIEditStandingCardScenario scenario =
                JSIEditStandingCardScenario.getSingleton();
            JSIStandingCard sc = scenario.getSelectedStandingCard();
            this.mFrom = jsi.getUsername();
            this.mTo = "Everyone";
            this.mCardId = sc.getId();
            this.mPos = sc.getGameObject().transform.position;
            this.mRot = sc.getGameObject().transform.rotation;
            JSIRect3D rect = (JSIRect3D)sc.getCard().getGeom();
            this.mWidth = rect.getWidth();
            this.mHeight = rect.getHeight();
        }

        public static bool execute(XApp app) {
            JSICmdToSendTransformStandingCardMsg cmd =
                new JSICmdToSendTransformStandingCardMsg(app);
            return cmd.execute();
        }

        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp) this.mApp;

            JSIMsgToTransformStandingCard msg =
                new JSIMsgToTransformStandingCard(this.mFrom, this.mTo,
                this.mCardId, this.mPos, this.mRot, this.mWidth, this.mHeight);
            JSISerializableMsgToTransformStandingCard sMsg =
                new JSISerializableMsgToTransformStandingCard(msg);
            // jsi.getDeliveryPerson().sendSerializableMsg(sMsg);

            return true;
        }

        protected override XJson createLogData() {
            JSIApp jsi = (JSIApp)this.mApp;
            XJson data = new XJson();
            data.addMember("from", this.mFrom);
            data.addMember("to", this.mTo);
            data.addMember("cardId", this.mCardId);
            data.addMember("pos", this.mPos);
            data.addMember("rot", this.mRot);
            data.addMember("width", this.mWidth);
            data.addMember("height", this.mHeight);
            return data;
        }
    }
}