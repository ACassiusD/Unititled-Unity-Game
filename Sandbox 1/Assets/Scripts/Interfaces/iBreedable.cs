using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iBreedable
{  
    bool availableToMate{ get; set; }  
    float breedingRange{ get; set; }
}
