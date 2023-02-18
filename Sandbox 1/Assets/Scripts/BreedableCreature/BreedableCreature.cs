using UnityEngine;

[RequireComponent(typeof(BreedableCreatureMoveComponent))]
public class BreedableCreature : MonoBehaviour, InteractableInterface, iBreedable
{
    public Color skinColor;
    public Color sphereCastColor;
    public Material skinColorMaterial;
    BetaCharacter playerScript;
    BreedableCreatureBreeder breeder;
    public float matingCheckInterval = 10f;
    [field: SerializeField]
    public float breedingRange { get; set; }
    [field: SerializeField]
    public bool availableToMate { get; set; }
    BreedableCreatureMoveComponent moveComponent; 

    void Awake()
    {
        moveComponent = GetComponent<BreedableCreatureMoveComponent>();
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
        playerScript = PlayerManager.Instance.getPlayerScript();
    }

    public void interact()
    {
        //if player holding love fruit, eat it then attempt breed
        Debug.Log("iNTERACTED WITH " + this.name);
        breeder.AttemptToBreed(this);
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


}