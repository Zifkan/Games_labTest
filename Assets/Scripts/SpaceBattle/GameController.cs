using System;
using System.Collections.Generic;
using SpaceBattle.Enums;
using SpaceBattle.SpaceShips;
using SpaceBattle.UI;
using SpaceBattle.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceBattle
{
    [Serializable]
    struct SceneLoadStateInfo
    {
        [SerializeField] 
        private GameStage _gameStage;
        
        [SerializeField] 
        private int _sceneId;

        public GameStage Stage => _gameStage;

        public int SceneId => _sceneId;
    }
    
    public class GameController : SingletonBehaviour<GameController> 
    {
        [SerializeField]
        private SelectShipMenu _shipMenu;
        
        [SerializeField]
        private ShipPlacer _placer;

        [SerializeField] 
        private List<SceneLoadStateInfo> _sceneLoadStateInfo;
        
        private readonly ShipConstructor _shipConstructor = new ShipConstructor();
        private readonly List<BaseSpaceShip> _ships = new List<BaseSpaceShip>();
        private GameStage _gameStage;
        
        
        public List<BaseSpaceShip> Ships => _ships; // TODO: Possible remove 
       
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            var shipResources = Resources.LoadAll<BaseSpaceShip>("SpaceBattle/Ships/");
            
            foreach (var ship in shipResources)
            {
                var instantiatedShip = Instantiate(ship, new Vector3(0, 0, 0), Quaternion.identity);
                _ships.Add(instantiatedShip);
            }
            
            SceneManager.sceneLoaded+=SceneManagerOnsceneLoaded;
            
            _gameStage = GameStage.ShipConstruct;
            _shipConstructor.Init(_ships,_shipMenu,_placer);
        }

        private void SceneManagerOnsceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            _shipConstructor.Ships.ForEach(ship => ship.Reset());
        }

        public void StartFight()
        {
            _sceneLoadStateInfo.ForEach(info =>
            {
                if (info.Stage == GameStage.Battle)
                {
                    _gameStage = GameStage.Battle;
                    SceneManager.LoadScene(info.SceneId);
                }
            });
        }

        public void EndFight()
        {
            Debug.Log("End fight");
            _sceneLoadStateInfo.ForEach(info =>
            {
                if (info.Stage == GameStage.ShipConstruct)
                {
                    SceneManager.LoadScene(info.SceneId);
                }
            });
        }

        private void Update()
        {
            if (_gameStage == GameStage.Battle && Input.GetKeyUp(KeyCode.Escape))
            {
                _gameStage = GameStage.ShipConstruct;
                EndFight();
            }
        }
    }
}