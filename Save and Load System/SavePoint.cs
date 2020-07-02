// Script by Jakob Elkjær Husted
namespace SaveLoadSystem
{
    using System.Linq;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

#endif

    [ExecuteInEditMode]
    public class SavePoint : MonoBehaviour
    {
        // References:
        [HideInInspector] public SavePointManager thisSavePointManager;

        // Public:
        public string saveTriggerTag = "None";
        public bool useSavePointPosition = true, savingDisabled, savePointUsed;
        [HideInInspector] public int thisID;

        public enum colliderTypes
        {
            None,
            Box,
            Capsule,
            Mesh,
            Sphere,
            Wheel
        }

        [HideInInspector] public colliderTypes attachCollider;

        private void Awake()
        {
            if (Application.isPlaying)
                if (transform.parent.gameObject.GetComponent<SavePointManager>() != null)
                    thisSavePointManager = transform.parent.gameObject.GetComponent<SavePointManager>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (Application.isPlaying && !savingDisabled && !savePointUsed)
            {
                if (col.CompareTag(saveTriggerTag))
                {
                    if (useSavePointPosition)
                        thisSavePointManager.SaveGame(gameObject.transform.position);
                    else
                        thisSavePointManager.SaveGame();

                    thisSavePointManager.previousCheckPoint = thisID;
                    savePointUsed = true;
                }
            }
        }

        public void AttachCollider(int enumIndex)
        {
            Debug.Log("Attaching " + attachCollider + " collider to " + gameObject.name);
            switch (enumIndex)
            {
                case 0:
                    break;
                case 1:
                    gameObject.AddComponent<BoxCollider>();
                    break;
                case 2:
                    gameObject.AddComponent<CapsuleCollider>();
                    break;
                case 3:
                    gameObject.AddComponent<MeshCollider>();
                    break;
                case 4:
                    gameObject.AddComponent<SphereCollider>();
                    break;
                case 5:
                    gameObject.AddComponent<WheelCollider>();
                    break;
                default:
                    break;
            }

            GetComponent<Collider>().isTrigger = true;
        }

#if UNITY_EDITOR
        void OnDestroy()
        {
            if (thisSavePointManager != null && Application.isEditor)
            {
                if (thisSavePointManager.savePoints.Count > 0 &&
                    thisSavePointManager.savePoints.ElementAtOrDefault(thisID - 1))
                {
                    thisSavePointManager.savePoints.RemoveAt(thisID - 1);

                    for (int i = 0; i < thisSavePointManager.savePoints.Count; i++)
                    {
                        if (thisSavePointManager.savePoints[i].GetComponent<SavePoint>() != null)
                        {
                            thisSavePointManager.savePoints[i].GetComponent<SavePoint>().thisID = i + 1;
                            thisSavePointManager.savePoints[i].name = "SavePoint" + (i + 1);
                        }
                    }
                }
            }
        }
#endif
    }
}