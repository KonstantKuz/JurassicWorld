using System;
using System.Collections.Generic;
using Dino.Session.Model;
using JetBrains.Annotations;
using Logger.Extension;
using UnityEngine;
using Zenject;

namespace Dino.Analytics
{
    [PublicAPI]
    public class Analytics
    {
        public const char SEPARATOR = '_';

        [Inject] 
        private IEventParamProvider _eventParamProvider;
        
        
        private readonly ICollection<IAnalyticsImpl> _impls;
        
        public Analytics(ICollection<IAnalyticsImpl> impls)
        {
            _impls = impls;
        }

        public void Init()
        {
            this.Logger().Info("Initializing Analytics");
            foreach (var impl in _impls)
            {
                impl.Init();
            }
        }
        public void ReportTest()
        {
            ReportEventToAllImpls(Events.TEST_EVENT, null);
        }

        public void ReportLevelStart()
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_ID,
                EventParams.PASS_NUMBER,
            });
            ReportEventToAllImpls(Events.LEVEL_START, eventParams);
        }

        public void ReportLevelFinish(SessionResult result)
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_ID,
                EventParams.PASS_NUMBER,
                EventParams.TIME_SINCE_LEVEL_START,
                EventParams.ENEMY_KILLED
            });
            eventParams[EventParams.LEVEL_RESULT] = result == SessionResult.Win ? LevelResult.WIN : LevelResult.LOSE;

            ReportEventToAllImpls(Events.LEVEL_FINISHED, eventParams);
        }

        public void ReportLootItem(string itemId)
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_ID,
                EventParams.PASS_NUMBER,
                EventParams.TIME_SINCE_LEVEL_START,
            });
            eventParams[EventParams.ITEM_ID] = itemId;
            
            ReportEventToAllImpls(Events.PICKUP, eventParams);
        }
        
        public void ReportCraftItem(string itemId)
        {
            var eventParams = _eventParamProvider.GetParams(new[]
            {
                EventParams.LEVEL_NUMBER,
                EventParams.LEVEL_ID,
                EventParams.TIME_SINCE_LEVEL_START,
            });
            eventParams[EventParams.ITEM_ID] = itemId;
            
            ReportEventToAllImpls(Events.CRAFT, eventParams);
        }

        private void ReportEventToAllImpls(string eventName, Dictionary<string, object> eventParams)
        {
            this.Logger().Debug($"Send event {eventName} with params {string.Join(":", eventParams)}");
            foreach (var impl in _impls)
            {
                impl.ReportEventWithParams(eventName, eventParams, _eventParamProvider);
            }
        }
    }
}