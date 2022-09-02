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

    public class WaitForAction : CustomYieldInstruction
    {
        private bool _actionTriggered;
        private Action _action;
        public override bool keepWaiting => !_actionTriggered;

        public WaitForAction(Action action)
        {
            _action = action;
            _action += OnActionTriggered;
        }

        private void OnActionTriggered()
        {
            _action -= OnActionTriggered;
            _actionTriggered = true;
        }
    }
}