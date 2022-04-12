using System.Collections.Generic;
using System.Linq;
using JSI.Msg;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JSI {
    public class JSIEventListener {
        // fields
        private JSIApp mJSI = null;

        // constructor
        public JSIEventListener(JSIApp jsi) {
            this.mJSI = jsi;
        }

        // methods
        //keyboard
        public void keyPressed(Key k) {
            //Debug.Log("keyPressed: " + kc);
            JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                getCurScene();
            curscene.handleKeyDown(k);
        }
        public void keyReleased(Key k) {
            //Debug.Log("keyReleased: " + kc);
            JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                getCurScene();
            curscene.handleKeyUp(k);
        }
        // pen
        public void penDown(Vector2 pt) {
            if (this.mJSI.getPenMarkMgr().penDown(pt)) {
                // activate pen cursor
                JSICursor2D penCursor = this.mJSI.getCursorMgr().getPenCursor();
                penCursor.getGameObject().transform.position = pt;
                penCursor.getGameObject().SetActive(true);

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handlePenDown(pt);
            }
        }
        public void penDragged(Vector2 pt) {
            if (this.mJSI.getPenMarkMgr().penDrag(pt)) {
                // update pen cursor
                JSICursor2D penCursor = this.mJSI.getCursorMgr().getPenCursor();
                penCursor.getGameObject().transform.position = pt;

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                   getCurScene();
                curscene.handlePenDrag(pt);
            }
        }
        public void penUp(Vector2 pt) {
            if (this.mJSI.getPenMarkMgr().penUp(pt)) {
                // update pen cursor
                JSICursor2D penCursor = this.mJSI.getCursorMgr().getPenCursor();
                penCursor.getGameObject().transform.position = pt;

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handlePenUp(pt);

                // deactivate pen cursor
                penCursor.getGameObject().SetActive(false);
            }
        }
        // eraser
        public void eraserDown(Vector2 pt) {
            if (this.mJSI.getPenMarkMgr().eraserDown(pt)) {
                // activate pen cursor
                JSICursor2D eraserCursor = this.mJSI.getCursorMgr().getPenCursor();
                eraserCursor.getGameObject().transform.position = pt;
                eraserCursor.getGameObject().SetActive(true);

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handleEraserDown(pt);
            }
        }
        public void eraserDragged(Vector2 pt) {
            if (this.mJSI.getPenMarkMgr().eraserDrag(pt)) {
                // update pen cursor
                JSICursor2D eraserCursor = this.mJSI.getCursorMgr().getPenCursor();
                eraserCursor.getGameObject().transform.position = pt;

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                   getCurScene();
                curscene.handleEraserDrag(pt);
            }
        }
        public void eraserUp(Vector2 pt) {
            if (this.mJSI.getPenMarkMgr().eraserUp(pt)) {
                // update pen cursor
                JSICursor2D eraserCursor = this.mJSI.getCursorMgr().getPenCursor();
                eraserCursor.getGameObject().transform.position = pt;

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handleEraserUp(pt);

                // deactivate pen cursor
                eraserCursor.getGameObject().SetActive(false);
            }
        }
        // touch
        public void touchDown(JSITouchPacket tp) {
            if (this.mJSI.getTouchMarkMgr().touchDown(tp)) {
                //activate touch cursor
                JSICursor2D tc = new JSICursor2D(this.mJSI, tp.getId(),
                    "TouchCursor", JSICursorMgr.TOUCH_RADIUS, tp.getPt());
                this.mJSI.getCursorMgr().getTouchCursors().Add(tc);

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // set current touch state
                this.mJSI.getTouchMarkMgr().setTouchDownWasJustNow(true);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handleTouchDown();

                // reset current touch state
                this.mJSI.getTouchMarkMgr().setTouchDownWasJustNow(false);
            }
        }
        public void touchDragged(List<JSITouchPacket> tps) {
            if (this.mJSI.getTouchMarkMgr().touchDrag(tps)) {
                //update touch cursor
                foreach (JSITouchMark tm in this.mJSI.getTouchMarkMgr().
                    getDraggedTouchMarks()) {

                    JSICursor2D tc = this.mJSI.getCursorMgr().findTouchCursor(tm);
                    if (tc != null) {
                        tc.getGameObject().transform.position = tm.getPts().Last();
                    }
                }

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // set current touch state
                this.mJSI.getTouchMarkMgr().setTouchDragWasJustNow(true);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handleTouchDrag();

                // reset current touch state
                this.mJSI.getTouchMarkMgr().setTouchDragWasJustNow(false);
            }
        }
        public void touchUp(JSITouchPacket tp) {
            if (this.mJSI.getTouchMarkMgr().touchUp(tp)) {
                // update touch cursor
                JSICursor2D tc = this.mJSI.getCursorMgr().findTouchCursor(tp);
                if (tc != null) {
                    tc.getGameObject().transform.position = tp.getPt();
                }

                // update collider physics
                Physics2D.Simulate(Time.fixedDeltaTime);

                // set current touch state
                this.mJSI.getTouchMarkMgr().setTouchUpWasJustNow(true);

                // call current scene
                JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                    getCurScene();
                curscene.handleTouchUp();

                // reset current touch state
                this.mJSI.getTouchMarkMgr().setTouchUpWasJustNow(false);

                // deactivate touch cursor
                if (tc != null) {
                    this.mJSI.getCursorMgr().getTouchCursors().Remove(tc);
                    tc.destroyGameObject();
                }
            }
        }
        // msg
        public void msgReceived(JSIMsg msg) {
            JSIScene curscene = (JSIScene) this.mJSI.getScenarioMgr().
                getCurScene();
            curscene.handleMsg(msg);
        }
        // VR
        public void VRheadsetMounted() {
            JSIScene curscene = (JSIScene)this.mJSI.getScenarioMgr().
                getCurScene();
            curscene.handleVRHeadsetMount();
        }
        public void VRheadsetUnmounted() {
            JSIScene curscene = (JSIScene)this.mJSI.getScenarioMgr().
                getCurScene();
            curscene.handleVRHeadsetUnmount();
        }
    }
}