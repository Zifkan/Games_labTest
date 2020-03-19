using SpaceBattle.SpaceShips;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBattle.UI
{
    public class SelectShipMenu : MonoBehaviour
    {
        [SerializeField] 
        private Button _button;

        [SerializeField] 
        private GameObject _modulesPanel;

        private ShipConstructor _shipConstructor;

        private void Start()
        {
           _shipConstructor = GetComponent<ShipConstructor>();

           
           for (int i = 0; i < _shipConstructor.Modules.Count; i++)
           {
              var btn =  Instantiate(_button,_modulesPanel.transform);
              btn.GetComponentInChildren<Text>().text = _shipConstructor.Modules[i].name;
              btn.onClick.AddListener( () => OnButtonPressed( i ) );
           }
        }

        private void OnButtonPressed(int i)
        {
            _shipConstructor.SetupModule(i);
        }
    }
}