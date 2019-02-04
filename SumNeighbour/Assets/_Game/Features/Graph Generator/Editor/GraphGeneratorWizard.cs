using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Noodlepop.VariableAssets;

namespace SumNeighbours.EditorTools
{


    public class GraphGeneratorWizard : ScriptableWizard
    {
        private const string LEVEL_FOLDER_PATH = "Assets/LevelAssets";
        [SerializeField] private string _fileName = "Folder Name";
        [SerializeField] private int _height = 3;
        [SerializeField] private int _width = 3;
        [SerializeField] private int _heightVariance = 0;
        [SerializeField] private int _widthVariance = 0;
        [SerializeField] private int _numberOfLevelsToGenerate = 1;

        [MenuItem("Level Tools/Generate Level batch", priority = 0)]
        static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<GraphGeneratorWizard>("Generate Levels", "Create");
        }

        void OnWizardCreate()
        {
            if (_height < 3 || _width < 3 || _widthVariance < 0 || _heightVariance < 0)
            {
                EditorUtility.DisplayDialog("Error!", "Height and Width must be at least 3.  Variance must be at least zero.", "OK");
                return;
            }

            GraphGenerator graphGenerator = new GraphGenerator(_fileName, _height, _width, _heightVariance, _widthVariance);

            StringListVariable levelNames = ScriptableObject.CreateInstance<StringListVariable>();
            levelNames.Init(new List<string>());
            levelNames.name = _fileName + " Level Names";

            string directoryPath = Path.Combine(LEVEL_FOLDER_PATH, _fileName);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            for (int i = 0; i < _numberOfLevelsToGenerate; i++)
            {
                LevelAsset levelAsset = ScriptableObject.CreateInstance<LevelAsset>();
                levelAsset.NodeGraph = graphGenerator.GenerateGraph();

                if (levelAsset.NodeGraph == null)
                    continue;
                
                levelAsset.AverageNumberOfNeighbours = levelAsset.NodeGraph.GetAverageNumberOfNeighbours();
                levelAsset.NumberOfStartingNodes = levelAsset.NodeGraph.GetNumberOfStartingNodes();

                string levelName = _fileName + i.ToString();
                levelNames.DefaultValue.Add(levelName);

                AssetDatabase.CreateAsset(levelAsset, Path.Combine(directoryPath, levelName + ".asset"));
            }

            AssetDatabase.CreateAsset(levelNames, Path.Combine(directoryPath, levelNames.name + ".asset"));

        }

        void OnWizardUpdate()
        {
            helpString = "Generate a graph dude!";
        }

        void OnWizardOtherButton()
        {

        }
    }
}