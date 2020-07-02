// Script by Jakob Elkjær Husted
namespace SaveLoadSystem
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.SceneManagement;

    public class SaveLoadManager
    {
        private const string SAVE_SEPERATOR = "#SAVE-VALUE#";
        public bool newGame = true;
        public int firstSceneIndex = 1;
        private Vector3 _savePointPos;
        private bool _useSavePointPos = false;

        public void NewGame()
        {
            PlayerPrefs.SetInt("currentScene", firstSceneIndex);
            PlayerPrefs.SetInt("loadGameOnAwake", 0);

            Scene startScene = SceneManager.GetSceneAt(firstSceneIndex);
            SceneManager.LoadScene(startScene.buildIndex);
        }

        public void ContinueGame()
        {
            if (PlayerPrefs.GetInt("previousGame") == 1)
            {
                PlayerPrefs.SetInt("loadGameOnAwake", 1);
                SceneManager.LoadScene(PlayerPrefs.GetInt("currentScene"));
            }
            else if (Application.isEditor)
                Debug.Log("Save/Load Manager: No previous games to load");
        }

        public void OpenLevel(int atBuildIndex)
        {
            if (atBuildIndex < SceneManager.sceneCountInBuildSettings && atBuildIndex >= 0)
            {
                SceneManager.LoadScene(atBuildIndex);
            }
        }

        public void NextLevel()
        {
            if (SceneManager.GetActiveScene().buildIndex != SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                Debug.Log("Error: There is no next scene!");
            }
        }

        public void SaveGame(Vector3 position)
        {
            _savePointPos = position;
            _useSavePointPos = true;

            SaveGame();
        }

        public void SaveGame()
        {
            string saveString = "";

            //// Object saving: ////
            Saveable[] saveables = Resources.FindObjectsOfTypeAll<Saveable>();

            foreach (var saveable in saveables)
            {
                saveable.ConstructContainer();

                saveable.GetContainer().Name = saveable.gameObject.name;

                if (saveable.savePosition)
                    saveable.GetContainer().Position = saveable.transform.position;

                if (saveable.saveRotation)
                    saveable.GetContainer().Rotation = saveable.transform.rotation;

                saveable.GetContainer().IsActive = saveable.gameObject.activeSelf;

                saveString += saveable.ThisContainerToString() + SAVE_SEPERATOR;
            }

            PlayerPrefs.SetString("objectSave", saveString);


            //// SavePoint State: ////
            SavePoint[] tempSavePoints = GameObject.FindObjectsOfType<SavePoint>();
            saveString = "";

            if (tempSavePoints != null)
            {
                List<SavePointContainer> tempSavePointContainerList = new List<SavePointContainer>();

                for (int i = 0; i < tempSavePoints.Length; i++)
                {
                    SavePointContainer tempSavePointContainer = new SavePointContainer();

                    tempSavePointContainer.savePointUsed = tempSavePoints[i].savePointUsed;
                    tempSavePointContainer.thisID = tempSavePoints[i].thisID;

                    tempSavePointContainerList.Add(tempSavePointContainer);

                    saveString += SAVE_SEPERATOR + JsonUtility.ToJson(tempSavePointContainerList[i]);
                }

                PlayerPrefs.SetString("savePointStateSave", saveString);
            }

            //// Scene save: ////
            PlayerPrefs.SetInt("currentScene", SceneManager.GetActiveScene().buildIndex);

            PlayerPrefs.SetInt("previousGame", 1);

            if (Application.isEditor)
                Debug.Log("Saved the game!");
        }

        /// <summary>
        /// Load game to savepoint (checkpoint reached in scene). This does not load to scene, use 'Continue' for that.
        /// </summary>
        /// <param name="loadSavePointState">Only set true if loading using 'continue' button</param>
        public void LoadGame(bool loadSavePointState)
        {
            if (loadSavePointState)
            {
                // SavePoint State load:
                SavePoint[] tempSavePoints = GameObject.FindObjectsOfType<SavePoint>();
                string loadString = PlayerPrefs.GetString("savePointStateSave");

                if (tempSavePoints != null)
                {
                    string[] dataString = loadString.Split(new[] { SAVE_SEPERATOR }, System.StringSplitOptions.None);

                    for (int i = 1; i < dataString.Length; i++) // Must start at 1
                    {
                        SavePointContainer tempSavePointContainer =
                            JsonUtility.FromJson<SavePointContainer>(dataString[i]);

                        for (int j = 0; j < tempSavePoints.Length; j++)
                        {
                            if (tempSavePointContainer.thisID == tempSavePoints[j].thisID)
                                tempSavePoints[j].savePointUsed = tempSavePointContainer.savePointUsed;
                        }
                    }
                }

                LoadGame();
            }
            else
            {
                LoadGame();
            }
        }

        /// <summary>
        /// Load game to savepoint (checkpoint reached in scene). This does not load to scene, use 'Continue' for that.
        /// </summary>
        public void LoadGame()
        {
            string loadString = "";

            if (PlayerPrefs.GetInt("previousGame") != 1)
                return;

            //// Object loading: ////
            Saveable[] saveables = Resources.FindObjectsOfTypeAll<Saveable>(); // No need for IDs as saveables will be found according to scene hierarchy

            loadString = PlayerPrefs.GetString("objectSave");
            string[] dataString = loadString.Split(new[] { SAVE_SEPERATOR }, System.StringSplitOptions.None);



            for (int i = 0; i < dataString.Length - 1; i++)
            {
                if (dataString[i] != null)
                {
                    Saveable.ContainerClass tempContainer =
                        JsonUtility.FromJson<Saveable.ContainerClass>(dataString[i]);

                    for (int j = 0; j < saveables.Length; j++)
                    {
                        if (tempContainer.Name != saveables[j].gameObject.name)
                            continue;

                        saveables[j].ConstructContainer(tempContainer);
                        break;
                    }
                }
            }

            foreach (var saveable in saveables)
            {
                if (saveable.savePosition)
                    saveable.transform.position = saveable.GetContainer().Position;

                if (saveable.saveRotation)
                    saveable.transform.rotation = saveable.GetContainer().Rotation;

                saveable.gameObject.SetActive(saveable.GetContainer().IsActive);
            }

            if (Application.isEditor)
                Debug.Log("Loaded the game!");
        }

        public class SavePointContainer
        {
            public bool savePointUsed;
            public int thisID;
        }
    }
}