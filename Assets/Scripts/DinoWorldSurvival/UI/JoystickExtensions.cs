﻿using UnityEngine;

namespace DinoWorldSurvival.UI
{
    public static class JoystickExtensions
    {
        public static void Attach(this Joystick joystick, Transform parent)
        {
            joystick.transform.SetParent(parent);
            joystick.transform.SetAsFirstSibling();
        }
    }
}