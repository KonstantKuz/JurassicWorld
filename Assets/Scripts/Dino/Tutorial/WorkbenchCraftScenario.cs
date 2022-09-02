using System;
using System.Collections;
using Codice.Client.BaseCommands;
using Dino.Location;
using Dino.Loot.Messages;
using Dino.Tutorial.WaitConditions;
using SuperMaxim.Messaging;
using Zenject;

namespace Dino.Tutorial
{
    public class WorkbenchCraftScenario : TutorialScenario
    {
        [Inject] private IMessenger _messenger;
        [Inject] private World _world;
        
        public override void Init()
        {
            
        }

        private IEnumerator RunCraftScenario()
        {
            yield return new WaitForMessage<LootCollectedMessage>(_messenger);
        }
    }
}