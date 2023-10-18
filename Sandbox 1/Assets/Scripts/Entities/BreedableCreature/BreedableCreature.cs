using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BreedableCreatureMoveComponent))]
public class BreedableCreature : MonoBehaviour, IInteractable, iBreedable
{
    public Color skinColor;
    public Color sphereCastColor;
    public Material skinColorMaterial;
    BreedableCreatureBreeder breeder;
    public float matingCheckInterval = 10f;
    [field: SerializeField]
    public float breedingRange { get; set; }
    [field: SerializeField]
    public bool availableToMate { get; set; }
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void Awake()
    {
        skinColor = Random.ColorHSV();
        breedingRange = 6;
        ApplyMaterial();
        breeder = gameObject.GetComponent<BreedableCreatureBreeder>();
        if (breeder == null)
        {
            breeder = gameObject.AddComponent<BreedableCreatureBreeder>();
        }
    }

    public void onCreate()
    {   
        breedingRange = 5f; // The range of the SphereCast
        availableToMate = false;
    }

    public void Interact(Interactor interactor, out bool interactSuccessful)
    {
        //if player holding love fruit, eat it then attempt breed
        breeder.AttemptToBreed(this);
        interactSuccessful = true;
    }

    /**
     * Create a new material, Apply the material to the Sphere object, 
     * Change the color of the material
     */
    void ApplyMaterial()
    {
        skinColorMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        GetComponent<Renderer>().material = skinColorMaterial;
        skinColorMaterial.color = skinColor;
    }

    public void UpdateSkinColor(Color color)
    {
        skinColorMaterial.color = color;
    }

    void OnDrawGizmos()
    {
        Vector3 sphereCastOrigin = transform.position;
        Gizmos.color = sphereCastColor;
        Gizmos.DrawWireSphere(sphereCastOrigin, breedingRange);
    }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }
}
