using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : PlayableCharacters, InteractableInterface
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
        throw new System.NotImplementedException();
    }
}