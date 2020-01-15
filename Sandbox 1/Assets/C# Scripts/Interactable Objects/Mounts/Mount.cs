using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mount : PlayableCharacters, InteractableInterface
{
    PlayerCharacter playerScript;

    public override void onCreate()
    {
        playerScript = PlayerManager.Instance.getPlayerScript();
        speed = 150;
        gravityScale = .25f;
        jumpForce = 70f;
    }


    public void interact()
    {
        Debug.Log("Interacted with mount");

        if (!playerScript.isRiding) //Character isn't riding. Call default movement
        {
            isBeingControlled = true;
            playerScript.setActiveMount(this);
        }
        else
        {
            isBeingControlled = false;
            playerScript.unMount();
        }
    }
}
