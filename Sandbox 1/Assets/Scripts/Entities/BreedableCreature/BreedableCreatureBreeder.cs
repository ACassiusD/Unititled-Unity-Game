using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A Breeder class, to breed breedable creatures. All breeding functionallity for breedable creatures should exist within this file.
public class BreedableCreatureBreeder : MonoBehaviour
{
    public LayerMask layerMask = 8; // The layer mask to check for colliders
    public GameObject breedableCreaturePrefab;

    void Start()
    {
        layerMask = LayerMask.NameToLayer("Interactive");
    }

    /**
     * Spawn a circle collider to find potential BreedableCreatures to breed with,
     * if found, select one randomly and attemp to breed with it.
     * Roll for success 
     */
    public void AttemptToBreed(BreedableCreature initiator)
    {
        if (!initiator.availableToMate) { return; }

        //Debug.Log("Looking for mate");

        List<BreedableCreature> potentialMates = new List<BreedableCreature> { };
        Vector3 origin = initiator.transform.position;

        RaycastHit[] hits = Physics.SphereCastAll(origin, initiator.breedingRange, Vector3.up, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            BreedableCreature creature = hits[i].collider.GetComponent<BreedableCreature>();
            if (creature != null && initiator != creature)
            {
                potentialMates.Add(creature);
               // Debug.Log("Detected creature: " + creature.name);
            }
        }

        //Select one randomly and attempt breed.
        if (potentialMates.Count > 0)
        {
            BreedableCreature randomMate = potentialMates[UnityEngine.Random.Range(0, potentialMates.Count)];

            //TODO: Roll for success
            Breed(initiator, randomMate);
        }
    }

    /**
     * get the inbetween color of the two parents, 
     * spawn a new child between the 2 paretns, and set child color.
     */
    private bool Breed(BreedableCreature initiator, BreedableCreature matingTarget)
    {
        // Instantiate the new BreedableCreature
        Vector3 spawnPosition = (transform.position + matingTarget.transform.position) / 2;

        //Set values.
        BreedableCreature newBreedableCreature = Instantiate(breedableCreaturePrefab, spawnPosition, Quaternion.identity).GetComponent<BreedableCreature>();
        Color childColor = Color.Lerp(initiator.skinColor, matingTarget.skinColor, 0.5f);
        newBreedableCreature.UpdateSkinColor(childColor);
        newBreedableCreature.availableToMate = false;
        return true;
    }


}
