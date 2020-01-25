using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedHighlight : MonoBehaviour
{

    public Renderer myRenderer;
    public int redCol = 255;
    public int greenCol = 255;
    public int blueCol = 255;
    public bool lookingAtObject = false;
    public bool flashingIn = true;
    public bool startedFlashing = false;
    Color startColor;


    private void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        startColor = myRenderer.material.color;
    }





    public void StartHighlight()
    {
     //   lookingAtObject = true;
      //  myRenderer.material.color = Color.yellow;

    }

    public void EndHightlight()
    {
      //  lookingAtObject = false; 
     //   myRenderer.material.color = startColor;
        //startedFlashing = false;  
        //lookingAtObject = false;
        //StopCoroutine(FlashObject());
        //myRenderer.material.color = new Color32(255, 255, 255, 255);
    }

    //IEnumerator FlashObject()
    //{
    //    while(lookingAtObject == true)
    //    {
    //        yield return new WaitForSeconds(0.05f);

    //        if(flashingIn == true)
    //        {
    //            if(blueCol  <= 30)
    //            {
    //                flashingIn = false;
    //            }
    //            else
    //            {
    //                blueCol -= 10;
    //               // greenCol -= 25;
    //            }
    //        }

    //        if(flashingIn == false)
    //        {
    //            if(blueCol >= 250)
    //            {
    //                flashingIn = true;
    //            }
    //            else
    //            {
    //                blueCol += 10;
    //                //greenCol += 25;
    //            }
    //        }
    //    }
    //}
}
