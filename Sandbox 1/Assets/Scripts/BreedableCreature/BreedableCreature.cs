using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BreedableCreature : MonoBehaviour, InteractableInterface, iBreedable
{
    public Color skinColor = Random.ColorHSV();
    public Color sphereCastColor;
    public Material skinColorMaterial;
    BetaCharacter playerScript;
    BreedableCreatureBreeder breeder;
    public float matingCheckInterval = 10f;
    [field: SerializeField]
    public float breedingRange { get; set; }
    [field: SerializeField]
    public bool availableToMate { get; set; }

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
