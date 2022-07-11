﻿using System.Linq;
using Dino.Units;
using Dino.Units.Service;
using TMPro;
using UnityEngine;
using Zenject;

namespace Dino.UI.Screen.World
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DebugView : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        [Inject] private UnitService _unitService;
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            var enemies = _unitService.GetAllUnitsOfType(UnitType.ENEMY).ToList();
            _text.text = $"enemies: {enemies.Count}\nhealth: {enemies.Select(it => it as Unit).Select(it => it.Health).Sum(it => it.CurrentValue.Value)}";
        }
    }
}