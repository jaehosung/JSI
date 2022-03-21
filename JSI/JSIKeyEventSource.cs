using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JSI {
    public class JSIKeyEventSource {
        // constants
        private static readonly List<Key> WATCHING_KEYS =
            new List<Key>() {
                Key.LeftCtrl, // for rotating
                Key.LeftAlt, // for translating
                Key.Enter, // for creating a standing card
                Key.Z, // undo
                Key.Y, // redo
                Key.S, // for saving file
                Key.O, // for opening file
            };

        // fields
        private JSIEventListener mEventListener = null;
        public void setEventListener(JSIEventListener eventListener) {
            this.mEventListener = eventListener;
        }

        // constructor
        public JSIKeyEventSource() {
        }

        // methods
        public void update() {
            foreach (Key k in JSIKeyEventSource.WATCHING_KEYS) {
                if (Keyboard.current[k].wasPressedThisFrame) {
                    this.mEventListener.keyPressed(k);
                }
                if (Keyboard.current[k].wasReleasedThisFrame) {
                    this.mEventListener.keyReleased(k);
                }
            }
        }
    }
}