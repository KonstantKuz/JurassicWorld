﻿using System;
using System.Collections.Generic;
using System.Linq;
using Logger;
using Logger.Extension;
using UnityEngine;

namespace Dino.Analytics.Wrapper
{
    public class AppMetricaAnalyticsWrapper : IAnalyticsImpl
    {
        public void Init()
        {
        }
        
        public void ReportTest()
        {
            ReportEvent("Test", new Dictionary<string, object>());
        }
        
        private void ReportEvent(string message, Dictionary<string, object> parameters)
        {
            AppMetrica.Instance.ReportEvent(message, parameters);
        }

        private void ReportEvent(string message) {
            AppMetrica.Instance.ReportEvent(message);
        }

        public void ReportEventWithParams(string eventName, 
            Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            ReportEvent(eventName, eventParams);
            UpdateProfileParams(eventName, eventParams, eventParamProvider);
        }

        private static void UpdateProfileParams(string eventName, Dictionary<string, object> eventParams, IEventParamProvider eventParamProvider)
        {
            var additionalParams = RequestAdditionalParams(eventName, eventParams, eventParamProvider);
            var profile = new YandexAppMetricaUserProfile();
            var updates = new List<YandexAppMetricaUserProfileUpdate>
            {
                BuildStringAttribute("last_event", BuildLastEventName(eventName, eventParams)), 
                BuildStringAttribute("level_id", (string) eventParams[EventParams.LEVEL_ID]),
                BuildFloatAttribute("wins", additionalParams[EventParams.WINS]),
                BuildFloatAttribute("defeats", additionalParams[EventParams.DEFEATS]),
                BuildFloatAttribute("levels", additionalParams[EventParams.LEVEL_NUMBER]),
                BuildFloatAttribute("level_retry", additionalParams[EventParams.PASS_NUMBER]),
                BuildFloatAttribute("craft_count", additionalParams[EventParams.CRAFT_COUNT]),
                BuildFloatAttribute("loot_count", additionalParams[EventParams.LOOT_COUNT]),
                BuildStringAttribute("ab_test_id", (string) additionalParams[EventParams.AB_TEST_ID])
            };
            var profileParams = updates.ToDictionary(it => it.Key, it => string.Join("-", it.Values));
            LoggerFactory.GetLogger<AppMetricaAnalyticsWrapper>().Debug($"Update profile params := {string.Join(":", profileParams)}");
            profile.ApplyFromArray(updates);
            AppMetrica.Instance.ReportUserProfile(profile);
        }

        private static Dictionary<string, object> RequestAdditionalParams(string eventName, Dictionary<string, object> eventParams,
            IEventParamProvider eventParamProvider)
        {
            var additionalParams = eventParamProvider.GetParams(new[]
            {
                EventParams.WINS,
                EventParams.DEFEATS,
                EventParams.PASS_NUMBER,
                EventParams.LEVEL_NUMBER,
                EventParams.CRAFT_COUNT,
                EventParams.LOOT_COUNT,
                EventParams.AB_TEST_ID
            });
            if (eventName == Events.LEVEL_FINISHED)
            {
                AddLevelResultToWinDefeatCount(eventParams, additionalParams);
            }

            return additionalParams;
        }

        private static void AddLevelResultToWinDefeatCount(IReadOnlyDictionary<string, object> eventParams, IDictionary<string, object> additionalParams)
        {
            additionalParams[EventParams.WINS] = Convert.ToInt32(additionalParams[EventParams.WINS]) +
                                                 ((string)eventParams[EventParams.LEVEL_RESULT] == LevelResult.WIN ? 1 : 0);
            
            additionalParams[EventParams.DEFEATS] = Convert.ToInt32(additionalParams[EventParams.DEFEATS]) +
                                                    ((string)eventParams[EventParams.LEVEL_RESULT] == LevelResult.LOSE
                                                        ? 1
                                                        : 0);
        }

        private static YandexAppMetricaUserProfileUpdate BuildFloatAttribute(string name, object value)
        {
            return new YandexAppMetricaNumberAttribute(name).WithValue(Convert.ToDouble(value));
        }
        
        private static YandexAppMetricaUserProfileUpdate BuildStringAttribute(string name, string value)
        {
            return new YandexAppMetricaStringAttribute(name).WithValue(value);
        }

        private static string BuildLastEventName(string eventName, Dictionary<string,object> eventParams)
        {
            return eventName switch
            {
                Events.LEVEL_START => $"level_start_{eventParams[EventParams.LEVEL_ID]}",
                Events.LEVEL_FINISHED => $"level_finished_{eventParams[EventParams.LEVEL_ID]}_{eventParams[EventParams.LEVEL_RESULT]}",
                Events.PICKUP => $"pickup_item_{eventParams[EventParams.ITEM_ID]}",
                Events.CRAFT => $"craft_item_{eventParams[EventParams.ITEM_ID]}",
                _ => throw new ArgumentOutOfRangeException(nameof(eventName), eventName, null)
            };
        }
    }
}