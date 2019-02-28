using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using JetBrains.Annotations;
using Noodlepop.GameEvents;
using Noodlepop.Systems;

namespace SumNeighbours
{
    public class LevelSelectMenuController : MonoBehaviour
    {
        [Tooltip(tooltip: "The scroll views contents game object")]
        [SerializeField] private Transform _contentTransform;
        [SerializeField] private GameObject _scrollViewTransform;
        [SerializeField] private GameObject _levelButtonPrefab;
        [Space(10)]
        [SerializeField] private List<LevelAsset> _levels = new List<LevelAsset>();

        [SerializeField] private GameEvent _loadCompleteEvent;

        private bool _loading = false;

        private void Start()
        {
            LoadLevelButtons();
        }
      
        private void LoadLevelButtons()
        {
            foreach (LevelAsset level in _levels)
            {
                GameObject button = Instantiate(_levelButtonPrefab, _contentTransform, false);
                button.GetComponentInChildren<Text>().text = string.IsNullOrEmpty(level.LevelName) ? level.name : level.LevelName;
                button.GetComponent<Button>().onClick.AddListener(() => LoadLevel(level));
            }
            
            _loadCompleteEvent.Raise();
        }

        private void LoadLevel(LevelAsset levelAsset)
        {
            GameManager.StartGame(levelAsset);
        }
    }
}