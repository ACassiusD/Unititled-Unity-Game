using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BreedableCreature : MonoBehaviour, InteractableInterface
{
    public Color skinColor = Color.red;
    public float breedingRange = 5f; // The range of the SphereCast
    public LayerMask layerMask = 8; // The layer mask to check for colliders
    public Material skinColorMaterial;
    public GameObject breedableCreaturePrefab;
    public Color sphereCastColor = Color.red;
    public float matingCheckInterval = 10f;
    public bool canMate = false; 
    BetaCharacter playerScript;

    public void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
    }

    private void Start()
    {
        layerMask = LayerMask.NameToLayer("Interactive");
        InvokeRepeating("LookForMate", matingCheckInterval, matingCheckInterval);
    }

    void Awake()
    {
        ApplyMaterial();
    }

    public void interact()
    {
        //if player holding love fruit, eat it then attempt breed
        Debug.Log("iNTERACTED WITH " + this.name);
        AttemptToMate();

    }

    /**
     * Spawn a circle collider to find potential BreedableCreatures to breed with,
     * if found, select one randomly and attemp to breed with it.
     * Roll for success 
     */
    private void AttemptToMate()
    {
        if (!canMate) { return; }

        Debug.Log("Looking for mate");

        List<BreedableCreature> potentialMates = new List<BreedableCreature> {};
        Vector3 origin = transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(origin, breedingRange, Vector3.up, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            BreedableCreature creature = hits[i].collider.GetComponent<BreedableCreature>();
            if (creature != null && this != creature)
            {
                potentialMates.Add(creature);
                Debug.Log("Detected creature: " + creature.name);
            }
        }

        //Select one randomly and attempt breed.
        if (potentialMates.Count > 0)
        {
            BreedableCreature randomMate = potentialMates[UnityEngine.Random.Range(0, potentialMates.Count)];
            
            //TODO: Roll for success
            Breed(randomMate);
        }
    }

    /**
     * get the inbetween color of the two parents, 
     * spawn a new child between the 2 paretns, and set child color.
     */
    private bool Breed(BreedableCreature matingTarget)
    {
        // Instantiate the new BreedableCreature
        Vector3 spawnPosition = (transform.position + matingTarget.transform.position) / 2;

        //Set values.
        BreedableCreature newBreedableCreature = Instantiate(breedableCreaturePrefab, spawnPosition, Quaternion.identity).GetComponent<BreedableCreature>();
        Color childColor = Color.Lerp(skinColor, matingTarget.skinColor, 0.5f);
        newBreedableCreature.UpdateSkinColor(childColor);
        newBreedableCreature.canMate = false;
        return true;
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

    void OnDrawGizmos()
    {
        Vector3 sphereCastOrigin = transform.position;
        Gizmos.color = sphereCastColor;
        Gizmos.DrawWireSphere(sphereCastOrigin, breedingRange);
    }

    void UpdateSkinColor(Color color)
    {
        skinColorMaterial.color = color;
    }

    void Feed()
    {

    }


}
