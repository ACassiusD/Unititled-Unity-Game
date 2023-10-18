using UnityEditor;
using UnityEngine;
/// <summary>
/// This class contains custom drawer for ReadOnly attribute. So it cannot be edited in the UI
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    /// <summary>
    /// Unity method for drawing GUI in Editor
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="property">Property.</param>
    /// <param name="label">Label.</param>

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Saving previous GUI enabled value
        var previousGuiState = GUI.enabled;
        // Disabling edit for property
        GUI.enabled = false;
        // Drawing Property
        EditorGUI.PropertyField(position, property, label);
        // Setting old GUI enabled value
        GUI.enabled = previousGuiState;
    }
}