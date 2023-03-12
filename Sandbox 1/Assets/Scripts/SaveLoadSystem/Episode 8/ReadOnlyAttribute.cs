using UnityEngine;
/// <summary>
/// Read Only attribute.
/// Attribute is used only to mark ReadOnly Properties.
/// </summary>

//This class creates a new tag similar to [SeriazlizedField] but ensures it cannot be changed. As it will be used on things like Unique ID's for entities..
public class ReadOnlyAttribute : PropertyAttribute { }