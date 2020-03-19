using System;
using SpaceBattle.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceBattle.UI
{
    public class CustomButton : MonoBehaviour, IPoolable
    {
        [SerializeField]
        private Text _text;

        [SerializeField] 
        private Button _button;
        
        public void Init(string text,UnityAction action)
        {
            _text.text = text;
            _button.onClick.AddListener(action);
        }

        public void PrepareToUse()
        {
            gameObject.SetActive(true);
        }

        public void ReturnToPool()
        {
        }
    }
}