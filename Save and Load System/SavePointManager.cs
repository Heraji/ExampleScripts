// Script by Jakob Elkjær Husted - Place this script on a gameobject or prefab that you include in any scene you want save/load functionality.
namespace SaveLoadSystem
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine.SceneManagement;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

#endif

    public class SavePointManager : MonoBehaviour
    {
        // References:
        private SaveLoadManager saveLoadManager;

        // Public variables
        public string saveTriggerTag = "Player";
        public int firstSceneBuildIndex = 0;
        public bool drawGizmos = true, setupAudioSave;
        [HideInInspector] public List<GameObject> savePoints;
        [HideInInspector] public int previousCheckPoint = 1;

        public void Awake()
        {
            saveLoadManager = new SaveLoadManager();
            saveLoadManager.firstSceneIndex = firstSceneBuildIndex;

            Saveable[] saveables = Resources.FindObjectsOfTypeAll<Saveable>();
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("loadGameOnAwake") == 1)
            {
                PlayerPrefs.SetInt("loadGameOnAwake", 0);
                //// NOTE: You can also raise a cam fade in event here if needed. 

                saveLoadManager.LoadGame(true);
            }

            //// NOTE: Subscribe the functions of this class to menu events like continue or new game. It can be done similarly to the commented code below: 
            //UIMenu[] menuObjects = Resources.FindObjectsOfTypeAll<UIMenu>();
            //if (menuObjects != null)
            //{
            //    for (int i = 0; i < menuObjects.Length; i++)
            //    {
            //        // NOTE: Additionally you can check if (PlayerPrefs.GetInt("previousGame") == 1) somewhere else to disable the continue button if no previous save game is present.
            //        menuObjects[i].continueGameEvent += Continue;
            //        menuObjects[i].startGameEvent += NewGame;
            //    }
            //}
        }

        public void DisableSavingOnSavePoints()
        {
            if (savePoints != null && Application.isPlaying) // Should be called when playing (for debugging)
                for (int i = 0; i < savePoints.Count; i++)
                {
                    if (savePoints[i].GetComponent<SavePoint>() != null)
                    {
                        savePoints[i].GetComponent<SavePoint>().savingDisabled = true;
                    }
                }
        }

        public void TeleportToSavePoint(int savePointNumber, GameObject objToTeleport)
        {
            if (savePoints.ElementAtOrDefault(savePointNumber - 1))
            {
                if (objToTeleport != null)
                {
                    objToTeleport.transform.position =
                        savePoints[savePointNumber - 1].transform.position + transform.up;
                }
            }
        }

        public void LoadToPreviousCheckpoint()
        {
            if (savePoints.ElementAtOrDefault(previousCheckPoint - 1))
            {
                if (GameObject.FindGameObjectWithTag("Player") != null)
                {
                    GameObject tempPlayer = GameObject.FindGameObjectWithTag("Player");

                    LoadGame();
                }
            }
        }

        public void NewGame()
        {
            if (PlayerPrefs.GetInt("previousGame") == 1)
            {
                PlayerPrefs.SetInt("previousGame", 0);
            }
        }

        public void Continue()
        {
            if (PlayerPrefs.GetInt("previousGame") == 1)
            {
                saveLoadManager?.ContinueGame();
            }
        }

        public void SaveGame()
        {
            saveLoadManager?.SaveGame();
        }

        public void SaveGame(Vector3 pos)
        {
            saveLoadManager?.SaveGame(pos);
        }

        public void LoadGame()
        {
            saveLoadManager?.LoadGame(true);
        }

        public void NextLevel()
        {
            saveLoadManager?.NextLevel();
        }

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void DeleteSave()
        {
            PlayerPrefs.SetInt("previousGame", 0);
        }


#if UNITY_EDITOR
        public void AddSavePoint()
        {
            if (Application.isEditor)
            {
                GameObject tempSavePoint;

                if (savePoints == null)
                    savePoints = new List<GameObject>();

                tempSavePoint = new GameObject("SavePoint" + (savePoints.Count + 1));
                SavePoint savePointRef = tempSavePoint.AddComponent<SavePoint>();
                tempSavePoint.transform.position = gameObject.transform.position;
                tempSavePoint.transform.parent = transform;
                tempSavePoint.layer = 2;

                savePoints.Add(tempSavePoint);
                savePointRef.saveTriggerTag = saveTriggerTag;
                savePointRef.thisID = savePoints.Count;
                savePointRef.thisSavePointManager =
                    gameObject.GetComponent<SavePointManager>();
            }
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos && Application.isEditor)
                if (savePoints != null)
                {
                    Gizmos.color = Color.magenta;
                    Handles.color = Color.red;

                    for (int i = 0; i < savePoints.Count; i++)
                    {
                        Gizmos.DrawWireSphere(savePoints[i].transform.position, 1.0f);
                        Handles.Label(savePoints[i].transform.position + (Vector3.up * 1.0f), "SavePoint " + (i + 1));

                        if (savePoints[i].GetComponent<Collider>() != null)
                        {
                            Gizmos.color = Color.white;
                            Collider tempCollider = savePoints[i].GetComponent<Collider>();
                            Gizmos.DrawWireCube(tempCollider.bounds.center,
                                savePoints[i].GetComponent<Collider>().bounds.size);
                        }
                    }
                }
        }
#endif
    }
}