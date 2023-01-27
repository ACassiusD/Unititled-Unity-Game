using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Takes the noise map, turns it into a texture, and applies it to a plane in the game scene.
public class MapDisplay : MonoBehaviour
{
    //Reference of renderer of the plane, so we can set its texture
    public Renderer texureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture){
        //Set textuyre to plans renderer
        texureRender.sharedMaterial.mainTexture = texture;

        //Set size of the plane to the size of the textureMap.
        texureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
