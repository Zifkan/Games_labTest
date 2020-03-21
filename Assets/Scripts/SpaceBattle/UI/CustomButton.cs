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

        private UnityAction _action;
        
        public void Init(string text,UnityAction action)
        {
            _action = action;
            _text.text = text;
            _button.onClick.AddListener(_action);
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