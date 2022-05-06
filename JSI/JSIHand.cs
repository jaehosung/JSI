using JSI.AppObject;
using UnityEngine;

/*
Reference: https://developer.oculus.com/documentation/unity/unity-handtracking/

(Left hand)

pt   rt   mt   it

p3   r3   m3   i3

p2   r2   m2   i2

p1   r1   m1    i1    tht

                      th3

p0                    th2

                  th1

              th0
        s

        fs

fs: forearm stub
s: start
th: thumb
i: index
m: middle
r: ring
p: pinky
t: tip
*/
namespace JSI {
    public class JSIHand : JSIAppObject3D {
        // constants
        public enum Handedness { LEFT, RIGHT };

        // fields
        private Handedness mHandedness;
        public Handedness getHandedness() {
            return this.mHandedness;
        }
        private OVRHand mHand;
        private OVRSkeleton mSkeleton;
        public OVRSkeleton getSkeleton() {
            return this.mSkeleton;
        }

        // constructor
        public JSIHand(GameObject handPrefab) : base("Hand") {
            // destroy default game object and replace it with prefab
            // this game object's pos and rot is that of 'start'
            GameObject.Destroy(this.mGameObject);
            this.mGameObject = handPrefab;
            this.addComponents();

            // set fields
            this.mHand = this.mGameObject.GetComponent<OVRHand>();
            this.mSkeleton = this.mGameObject.GetComponent<OVRSkeleton>();

            if (this.mSkeleton.GetSkeletonType() == OVRSkeleton.SkeletonType.
                HandLeft) {
                this.mHandedness = JSIHand.Handedness.LEFT;
            } else if (this.mSkeleton.GetSkeletonType() == OVRSkeleton.
                SkeletonType.HandRight) {
                this.mHandedness = JSIHand.Handedness.RIGHT;
            }
        }

        protected override void addComponents() {}

        // methods
        public OVRBone findBone(OVRSkeleton.BoneId boneId) {
            foreach (OVRBone bone in this.mSkeleton.Bones) {
                if (bone.Id == boneId) {
                    return bone;
                }
            }
            return null;
        }
        public OVRBoneCapsule findBoneCapsule(OVRSkeleton.BoneId boneId) {
            foreach (OVRBoneCapsule boneCapsule in this.mSkeleton.Capsules) {
                if (boneCapsule.BoneIndex == (short) boneId) {
                    return boneCapsule;
                }
            }
            return null;
        }
        public Vector3 calcPalmPos() {
            if (this.findBone(OVRSkeleton.BoneId.Hand_Thumb1) == null ||
                this.findBone(OVRSkeleton.BoneId.Hand_Index1) == null ||
                this.findBone(OVRSkeleton.BoneId.Hand_Pinky1) == null) {
                return Vector3.zero;
            }
            Vector3 thumb1Pos = this.findBone(OVRSkeleton.BoneId.Hand_Thumb1).
                Transform.position;
            Vector3 index1Pos = this.findBone(OVRSkeleton.BoneId.Hand_Index1).
                Transform.position;
            Vector3 pinky1Pos = this.findBone(OVRSkeleton.BoneId.Hand_Pinky1).
                Transform.position;
            return (thumb1Pos + index1Pos + pinky1Pos) / 3f;
        }
        public Vector3 calcPalmUpDir() {
            if (this.mHandedness == JSIHand.Handedness.LEFT) {
                return this.getGameObject().transform.up;
            } else {
                return -this.getGameObject().transform.up;
            }
        }
        public bool isPinching() {
            return this.mHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        }
        public Vector3 calcPinchPos() {
            Vector3 indexTipPos = this.findBone(OVRSkeleton.BoneId.Hand_IndexTip).
                Transform.position;
            Vector3 thumbTipPos = this.findBone(OVRSkeleton.BoneId.Hand_ThumbTip).
                Transform.position;
            return (indexTipPos + thumbTipPos) / 2f;
        }
        
        public bool intersects(JSIStandingCard sc){
            Vector3 pinchPos = this.calcPinchPos();
            Vector3 pos = sc.getGameObject().transform.position;
            Quaternion rot = sc.getGameObject().transform.rotation;
            Plane groundPlane = new Plane(rot * Vector3.forward, pos);
            if(Mathf.Abs(groundPlane.GetDistanceToPoint(pinchPos))<0.03){
                return true;
            }else{
                return false;
            }
        }
    }
}