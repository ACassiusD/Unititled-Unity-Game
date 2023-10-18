using System;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string _id = Guid.NewGuid().ToString();

    //Static field persists through all instances of this class, has a collection of all unique IDs
    [SerializeField]
    private static SerializableDictionary<string, GameObject> idDatabase =
        new SerializableDictionary<string, GameObject>();

    //Public getter
    public string ID => _id;


    private void OnValidate()
    {
        if (idDatabase.ContainsKey(_id)) Generate();
        else idDatabase.Add(_id, this.gameObject);
    }

    //Remove self from ID Database if destoryed.
    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(_id)) idDatabase.Remove(_id);
    }

    //Generate new id for self.
    private void Generate()
    {
        _id = Guid.NewGuid().ToString();
        idDatabase.Add(_id, this.gameObject);
    }
}
