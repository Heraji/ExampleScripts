// Use this script on wall objects for easy mesh and material switching through scripting. Preferably make wall object a prefab. 
using UnityEditor;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Mesh[] meshes;
    public Material[] materials;
    public bool resizeCollider = true;
    [HideInInspector] public int currentMeshIndex, currentMaterialIndex;

    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private BoxCollider collider;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        collider = GetComponent<BoxCollider>();
    }

    public void SetMesh(int index) // Call to set mesh at meshes index
    {
        if (meshFilter == null)
            meshFilter = GetComponent<MeshFilter>();

        if (index < meshes.Length && index >= 0)
            meshFilter.sharedMesh = meshes[index];
        else
            return;

        if (resizeCollider)
        {
            if (collider == null)
                collider = GetComponent<BoxCollider>();

            if (collider == null) // if still null
            {
                Debug.LogError("Wall Error: trying to resize a collider that is null on " + gameObject.name);
                return;
            }

            collider.center = meshes[index].bounds.center;
            collider.size = meshes[index].bounds.size;
        }
    }

    public void SetMaterial(int index) // Call to set material of wall from index
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();

        if (index < materials.Length && index >= 0)
            meshRenderer.material = materials[index];
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Wall))]
public class Wall_Editor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = target as Wall;

        if (script.meshes != null)
            script.currentMeshIndex = EditorGUILayout.IntSlider("Mesh:", script.currentMeshIndex, 0, script.meshes.Length - 1);

        if (GUILayout.Button("Change Mesh"))
        {
            script.SetMesh(script.currentMeshIndex);
        }

        if (script.materials != null)
            script.currentMaterialIndex = EditorGUILayout.IntSlider("Material:", script.currentMaterialIndex, 0, script.materials.Length - 1);

        if (GUILayout.Button("Change Material"))
        {
            script.SetMaterial(script.currentMaterialIndex);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }
}
#endif