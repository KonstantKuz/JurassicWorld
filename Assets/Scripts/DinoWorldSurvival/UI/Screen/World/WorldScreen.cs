﻿using System.Collections;
using DinoWorldSurvival.Session.Messages;
using DinoWorldSurvival.Session.Model;
using DinoWorldSurvival.Session.Service;
using DinoWorldSurvival.UI.Screen.Debriefing;
using DinoWorldSurvival.UI.Screen.Debriefing.Model;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using UnityEngine;
using Zenject;

namespace DinoWorldSurvival.UI.Screen.World
{
    public class WorldScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.World;
        public override ScreenId ScreenId => ID;
        public override string Url => ScreenName;

        [SerializeField]
        private float _afterSessionDelay = 2;

        [Inject] private SessionService _sessionService;
        [Inject] private IMessenger _messenger;
        [Inject] private ScreenSwitcher _screenSwitcher;     
        [Inject] private Location.World _world;
        [Inject] private Joystick _joystick;
        [Inject] private Analytics.Analytics _analytics;

        [PublicAPI]
        public void Init()
        {
            _world.UnPause();
            _analytics.ReportLevelStart();
            _joystick.Attach(transform);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            StartCoroutine(EndSession(evn.Result));
        }

        private IEnumerator EndSession(SessionResult result)
        {
            yield return new WaitForSeconds(_afterSessionDelay);
            _world.CleanUp();
            var debriefingModel = new DebriefingScreenModel(result, _sessionService.Session);
            _screenSwitcher.SwitchTo(DebriefingScreen.URL, debriefingModel);
        }
    }
}