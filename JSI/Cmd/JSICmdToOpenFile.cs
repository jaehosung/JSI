using System;
using JSI.File;
using UnityEditor;
using UnityEngine;
using X;

namespace JSI.Cmd {
    public class JSICmdToOpenFile : XLoggableCmd {
        //fields
        private string mFilePath = string.Empty;

        // constructor
        private JSICmdToOpenFile(XApp app) : base(app) {}

        // static method to construct and execute this method
        public static bool execute(XApp app) {
            JSICmdToOpenFile cmd = new JSICmdToOpenFile(app);
            return cmd.execute();
        }

        // private constructor
        protected override bool defineCmd() {
            JSIApp jsi = (JSIApp) this.mApp;

            // open dialog at Desktop
            this.mFilePath = EditorUtility.OpenFilePanel("Open",
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "jsi3d");

            // pressed 'OPEN' button
            if (this.mFilePath != string.Empty) {
                if (this.readFile(this.mFilePath)) {
                    jsi.getSnapshotMgr().takeSnapshot();
                    // prev undo by unlinking the snapshot to prev snapshot
                    jsi.getSnapshotMgr().getCurSnapshot().setPrevSnapshot(null);
                    return true;
                } else {
                    return false;
                }
            // pressed 'CANCEL' button
            } else {
                return false;
            }
        }

        protected override XJson createLogData() {
            XJson data = new XJson();
            // "\" is a JSON escape character, so replace it with "/"
            data.addMember("filePath", this.mFilePath.Replace('\\', '/'));
            return data;
        }

        private bool readFile(string filePath) {
            JSIApp jsi = (JSIApp)this.mApp;

            // parse json file (it may fail if format is incorrect)
            try {
                string json = System.IO.File.ReadAllText(filePath);
                JSISerializableSaveData sSaveData = JsonUtility.
                    FromJson<JSISerializableSaveData>(json);
                JSISaveData saveData = sSaveData.toSaveData();

                // clear existing standing cards
                foreach (JSIStandingCard sc in jsi.getStandingCardMgr().
                    getStandingCards()) {

                    sc.destroyGameObject();
                }
                jsi.getStandingCardMgr().getStandingCards().Clear();

                // load file
                jsi.getPerspCameraPerson().setEye(saveData.getEye());
                jsi.getPerspCameraPerson().setView(saveData.getView());
                foreach (JSIStandingCard sc in saveData.getStandingCards()) {
                    jsi.getStandingCardMgr().getStandingCards().Add(sc);
                }
                return true;
            } catch {
                return false;
            }
        }
    }
}