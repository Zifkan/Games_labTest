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
    
    public class GameController : MonoBehaviour
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
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            var shipResources = Resources.LoadAll<BaseSpaceShip>("SpaceBattle/Ships/");
            
            foreach (var ship in shipResources)
            {
                _ships.Add(Instantiate(ship,new Vector3(0,0,0) , Quaternion.identity));
            }
            
            SceneManager.sceneLoaded+=SceneManagerOnsceneLoaded;
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
            _gameStage = GameStage.ShipConstruct;
            _sceneLoadStateInfo.ForEach(info =>
            {
                if (info.Stage == GameStage.ShipConstruct)
                {
                    SceneManager.LoadScene(info.SceneId);
                }
            });
        }

        private void Start()
        {
            _gameStage = GameStage.ShipConstruct;
            _shipConstructor.Init(_ships,_shipMenu,_placer);
        }

        private void Update()
        {
            if (_gameStage == GameStage.Battle && Input.GetKeyUp(KeyCode.Escape))
            {
                EndFight();
            }
        }
    }
}