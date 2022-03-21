using JSI.AppObject;
using UnityEngine;

namespace JSI {
    public class JSICursor2D : JSIAppCircle2D {
        // constants
        public static readonly float RADIUS = 2f;
        //public static readonly Color COLOR = Color.clear;
        public static readonly Color COLOR = Color.red;

        // fields
        private JSIApp mJSI = null;
        private string mId = string.Empty;
        public string getId() {
            return this.mId;
        }

        // constructor
        public JSICursor2D(JSIApp jsi, string id, string name, float radius,
            Vector2 pt) : base($"{ name }({ id })/Cursor2D", radius,
            JSICursor2D.COLOR) {

            this.mJSI = jsi;
            this.mId = id;
            this.mGameObject.transform.position = pt;
        }

        // methods
        public bool intersects(JSIAppGeom2D appGeom) {
            return this.getCollider().IsTouching(appGeom.getCollider());
        }

        public bool hits(JSIAppGeom3D appGeom3D) {
            Vector2 ctr = this.mGameObject.transform.position;
            JSIPerspCameraPerson cp = this.mJSI.getPerspCameraPerson();
            Ray ray = cp.getCamera().ScreenPointToRay(ctr);
            RaycastHit hit;
            Collider collider = appGeom3D.getCollider();
            if (collider.Raycast(ray, out hit, Mathf.Infinity)) {
                //JSIUtil.createDebugSphere(hit.point);
                return true;
            } else {
                return false;
            }
        }

        public RaycastHit calcHit(JSIAppGeom3D appGeom3D) {
            Vector2 ctr = this.mGameObject.transform.position;
            JSIPerspCameraPerson cp = this.mJSI.getPerspCameraPerson();
            Ray ray = cp.getCamera().ScreenPointToRay(ctr);
            RaycastHit hit;
            Collider collider = appGeom3D.getCollider();
            collider.Raycast(ray, out hit, Mathf.Infinity);
            return hit;
        }
    }
}