using System;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Dino.Tutorial.WaitConditions
{
    public class WaitForMessage<T> : CustomYieldInstruction
    {
        private bool _messageReceived;
        private readonly IMessenger _messenger;
        public override bool keepWaiting => !_messageReceived;
        public T Message { get; private set; }

        public WaitForMessage(IMessenger messenger)
        {
            _messenger = messenger;
            messenger.Subscribe<T>(OnMessageReceived);
        }

        private void OnMessageReceived(T msg)
        {
            Message = msg;
            _messenger.Unsubscribe<T>(OnMessageReceived);
            _messageReceived = true;
        }
    }
}