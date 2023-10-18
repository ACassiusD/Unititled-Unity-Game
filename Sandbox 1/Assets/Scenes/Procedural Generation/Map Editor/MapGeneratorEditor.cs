using UnityEditor;
using UnityEngine;

//Add custom map generator button to the GUI
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {//If any value was changed
            if (mapGen.autoUpdate)
            {
                mapGen.DrawMapInEditor(); //Regenrate the map the map
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.DrawMapInEditor();
        }
    }
}
