using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using JSI.Msg;
using UnityEngine;

namespace JSI {
    public class JSIDeliveryPerson {
        // constants
        private static int BUFFER_SIZE = 64; // bytes

        // fields
        private ClientWebSocket mWebSocket = null;
        private JSIEventListener mEventListener = null;

        // constructor
        public JSIDeliveryPerson() {
            this.mWebSocket = new ClientWebSocket();
        }

        // methods
        public void setEventListener(JSIEventListener eventListener) {
            this.mEventListener = eventListener;
        }

        public void connectTo(string webSocketUri) {
            try {
                this.mWebSocket.ConnectAsync(new Uri(webSocketUri),
                    CancellationToken.None).Wait();
                Debug.Log($"Connected to: { webSocketUri }");
                this.startReceivingText();
            } catch {
                Debug.LogWarning($"Couldn't connect.");
            }
        }

        public void disconnect() {
            this.mWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                string.Empty, CancellationToken.None).Wait();
            Debug.Log("Disconnected.");
        }

        public async void startReceivingText() {
            byte[] bytes = new byte[JSIDeliveryPerson.BUFFER_SIZE];
            ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);

            do {
                WebSocketReceiveResult result;
                using (MemoryStream ms = new MemoryStream()) {
                    do {
                        result = await this.mWebSocket.ReceiveAsync(buffer,
                            CancellationToken.None);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close) {
                        break;
                    }

                    ms.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(ms,
                        Encoding.UTF8)) {

                        string text = await reader.ReadToEndAsync();
                        this.receiveAsSerializableMsg(text);
                    }
                }
            } while (true);
        }

        private void receiveAsSerializableMsg(string text) {
            // We don't know how to translate text into msg,
            // because msg's subject is not yet readable.
            // So, for now, assume that its a message to "do something."
            JSISerializableMsg sMsg = null;
            try {
                sMsg = JsonUtility.
                    FromJson<JSISerializableMsgToDoSomething>(text);
            } catch {
                return;
            }

            // Parse the real subject.
            JSIMsg.Subject subject =
                (JSIMsg.Subject)Enum.Parse(typeof(JSIMsg.Subject), sMsg.subject);

            // Re-read the message, now knowing the subject.
            switch (subject) {
                case JSIMsg.Subject.HELLO:
                    sMsg = JsonUtility.
                        FromJson<JSISerializableMsgToSayHello>(text);
                    break;
                case JSIMsg.Subject.ADD_STANDING_CARD:
                    sMsg = JsonUtility.
                        FromJson<JSISerializableMsgToAddStandingCard>(text);
                    break;
                case JSIMsg.Subject.TRANSFORM_STANDING_CARD:
                    sMsg = JsonUtility.
                        FromJson<JSISerializableMsgToTransformStandingCard>(text);
                    break;
                case JSIMsg.Subject.GOOD_BYE:
                    sMsg = JsonUtility.
                        FromJson<JSISerializableMsgToSayGoodBye>(text);
                    break;
            }

            // Handle the message received event.
            JSIMsg msg = sMsg.toMsg();
            this.mEventListener.msgReceived(msg);
        }

        public void sendSerializableMsg(JSISerializableMsg sMsg) {
            string json = JsonUtility.ToJson(sMsg);
            this.sendText(json);
        }

        private void sendText(string text) {
            try {
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                ArraySegment<byte> buffer = new ArraySegment<byte>(bytes);
                this.mWebSocket.SendAsync(buffer, WebSocketMessageType.Text, true,
                    CancellationToken.None).Wait();
            } catch {
                Debug.LogWarning($"Couldn't send: { text }");
            }
        }
    }
}