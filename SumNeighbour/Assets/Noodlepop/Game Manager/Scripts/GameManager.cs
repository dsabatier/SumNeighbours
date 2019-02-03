using System.Collections.Generic;
using Noodlepop.GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using Noodlepop.SingletonPattern;
using Noodlepop.VariableAssets;
using SumNeighbours;

namespace Noodlepop.Systems
{
    /// <summary>
    /// Lives as long as the game is running and manages game state and scene loading/unloading
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private string _startScene;
        [SerializeField] private GameObject[] _systemPrefabs;
        [SerializeField] private GameStateMachine _gameStateMachine = new GameStateMachine();
     
        public static GameState CurrentGameState => Instance._gameStateMachine.GameState;
        public static LevelAsset CurrentLevel => Instance._currentLevel;
        private LevelAsset _currentLevel;
        private readonly List<GameObject> _systemInstances = new List<GameObject>();
        private List<AsyncOperation> _loadOperations;

        private string _sceneToBeLoadedAfterTransition;

        [SerializeField] private List<string> _loadedScenes = new List<string>();
        
        #region Unity Lifecycle
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            _loadOperations = new List<AsyncOperation>();
            InstantiateSystemPrefabs();
            LoadScene_Internal(_startScene);
            // Start any required boot sequence, API inits, etc
        }

        protected override void OnDestroy()
        {
            for (int i = 0; i < _systemInstances.Count; ++i)
            {
                Destroy(_systemInstances[i]);
            }

            _systemInstances.Clear();

            base.OnDestroy();
        }
        #endregion Unity Lifecycle

        #region Static wrapper methods

        /// <summary>
        /// Starts game and loads specified level by name
        /// </summary>
        /// <param name="levelAssetName"></param>
        /// <param name="levelAsset"></param>
        public static void StartGame(LevelAsset levelAsset)
        {
            Instance._currentLevel = levelAsset;
            //Instance.LoadScene_Internal("Game");
        }

        /// <summary>
        /// Restarts game, returns to main menu
        /// </summary>
        public static void RestartGame()
        {
            Instance._gameStateMachine.UpdateState(GameState.MainMenu);
            Instance.UnloadScene_Internal("Game");
        }

        #endregion Static wrapper methods

        public void LoadScene(string sceneName)
        {
            LoadScene_Internal("Transition");
        }
        
        public void LoadSceneWithTransition(string sceneName)
        {
            LoadScene_Internal("Transition");
            _sceneToBeLoadedAfterTransition = sceneName;
        }

        /// <summary>
        /// Don't ever do this again
        /// </summary>
        public void LoadQueuedScene()
        {
            if (_loadedScenes.Contains("Splash"))
                UnloadScene_Internal("Splash");
            
            if(_sceneToBeLoadedAfterTransition == "Game" && _loadedScenes.Contains("Level Select"))
                UnloadScene_Internal("Level Select");

            if (_sceneToBeLoadedAfterTransition == "Level Select" && _loadedScenes.Contains("Game"))
            {
                UnloadScene_Internal("Game");
                UnloadScene_Internal("Game UI");
            }

            if(_sceneToBeLoadedAfterTransition != string.Empty)
                LoadScene_Internal(_sceneToBeLoadedAfterTransition);
            
            if(_sceneToBeLoadedAfterTransition == "Game")
                LoadScene_Internal("Game UI");
           
            _sceneToBeLoadedAfterTransition = string.Empty;
        }

        public void UnloadTransitionScene()
        {
            if (_loadedScenes.Contains("Transition"))
                UnloadScene_Internal("Transition");
        }
        
        private void LoadScene_Internal(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (asyncOperation == null)
            {
                Debug.LogError("GameManager :: Unable to load scene: " + sceneName);
                return;
            }

            _loadedScenes.Add(sceneName);
            _loadOperations.Add(asyncOperation);
            asyncOperation.completed += OnLoadSceneComplete;
        }

        private void UnloadScene_Internal(string sceneName)
        {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            if (asyncOperation == null)
            {
                Debug.LogError("GameManager :: Unable to unload scene: " + sceneName);
                return;
            }

            _loadedScenes.Remove(sceneName);
            _loadOperations.Add(asyncOperation);
            asyncOperation.completed += OnUnloadSceneComplete;
        }

        private void OnLoadSceneComplete(AsyncOperation asyncOperation)
        {
            if (_loadOperations.Contains(asyncOperation))
            {
                _loadOperations.Remove(asyncOperation);

                if (_loadOperations.Count == 0)
                {
                    Debug.Log("Load complete.");
                }
            }
        }

        private void OnUnloadSceneComplete(AsyncOperation asyncOperation)
        {
            Debug.Log("Unload complete.");
            if (_loadOperations.Contains(asyncOperation)) _loadOperations.Remove(asyncOperation);
        }

        private void InstantiateSystemPrefabs()
        {
            for (int i = 0; i < _systemPrefabs.Length; i++)
            {
                GameObject prefabInstance = Instantiate(_systemPrefabs[i]);
                _systemInstances.Add(prefabInstance);
            }
        }
    }
}