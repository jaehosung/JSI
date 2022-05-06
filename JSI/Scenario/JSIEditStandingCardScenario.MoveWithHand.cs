using System;
using System.Collections.Generic;
using JSI.Cmd;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using X;

namespace JSI.Scenario {
    public partial class JSIEditStandingCardScenario : XScenario {
        public class MoveWithHand : JSIScene {
            // singleton pattern
            private static MoveWithHand mSingleton = null;
            public static MoveWithHand getSingleton() {
                Debug.Assert(MoveWithHand.mSingleton != null);
                return MoveWithHand.mSingleton;
            }
            public static MoveWithHand createSingleton(
                XScenario scenario) {
                Debug.Assert(MoveWithHand.mSingleton == null);
                MoveWithHand.mSingleton = new MoveWithHand(
                    scenario);
                return MoveWithHand.mSingleton;
            }
            private MoveWithHand(XScenario scenario) :
                base(scenario) {
            }

            // event handling methods
            public override void handleKeyDown(Key k) {
            }

            public override void handleKeyUp(Key k) {
            }

            public override void handlePenDown(Vector2 pt) {
            }

            public override void handlePenDrag(Vector2 pt) {
            }

            public override void handlePenUp(Vector2 pt) {
            }

            public override void handleEraserDown(Vector2 pt) {
            }

            public override void handleEraserDrag(Vector2 pt) {
            }

            public override void handleEraserUp(Vector2 pt) {
            }

            public override void handleTouchDown() {
              
            }

            public override void handleTouchDrag() {
     
            }

            public override void handleTouchUp() {
    
            }

            public override void handleLeftPinchStart() {
            }

            public override void handleLeftPinchEnd() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                XCmdToChangeScene.execute(jsi, this.mReturnScene, null);

            }

            public override void handleRightPinchStart() {
            }

            public override void handleRightPinchEnd() {
                Debug.Log("Not implement handleRightPinchEnd");
            }

            public override void handleHandsMove() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;
                //#TODO left Only
                JSIHand hand = jsi.getHandMgr().getLeftHand();
                JSICmdToMoveStandingCardWithHand.execute(jsi,hand);
                JSIHand leftHand = jsi.getHandMgr().getLeftHand();
                Debug.Log(leftHand.calcPinchPos());
                
            }

            public override void getReady() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSIEditStandingCardScenario scenario =
                    (JSIEditStandingCardScenario)this.mScenario;

                // activate and highlight only the selected stand.
                JSIStandingCard selectedSC =
                    JSIEditStandingCardScenario.getSingleton().
                    getSelectedStandingCard();
                selectedSC.getStand().getGameObject().SetActive(true);
                selectedSC.highlightStand(true);
            }

            public override void wrapUp() {
                JSIApp jsi = (JSIApp)this.mScenario.getApp();
                JSICmdToTakeSnapshot.execute(jsi);
            }
        }
    }
}