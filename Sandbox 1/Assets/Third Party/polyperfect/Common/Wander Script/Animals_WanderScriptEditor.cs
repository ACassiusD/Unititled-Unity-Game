using Polyperfect.Common;
using UnityEditor;

#if UNITY_EDITOR
namespace Polyperfect.Animals
{
    [CustomEditor(typeof(Animal_WanderScript))]
    [CanEditMultipleObjects]
    public class Animals_WanderScriptEditor : Common_WanderScriptEditor { }
}
#endif