using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationSprite : MonoBehaviour {
    public RenderTexture WithLine;
    public RenderTexture WithoutLine;
    public GameObject plane;
    public Material line;

    List<Texture2D> textures;
    int numOfText = 0;
    int imin = 1980;
    int imax = 0;
    int jmin = 1080;
    int jmax = 0;

    // Use this for initialization
    void Start () {
        textures = new List<Texture2D>();
    }
	
	// Update is called once per frame
	void Update () {

        
    }

    public void creation()
    {
        ++numOfText;

        Texture2D text = new Texture2D(1980, 1080, TextureFormat.ARGB32, false);

        Texture2D withLine = toTexture2D(WithLine);
        Texture2D withoutLine = toTexture2D(WithoutLine);

        int i;
        int j;
        int k;

        Color verif = line.GetColor("_Color");
        Debug.Log(withLine.GetPixel(960, 530).ToString());
        Debug.Log(verif.ToString());
        if (compareColor(withLine.GetPixel(960, 530), verif))
        {

            Debug.Log("Here success !!");
        }

        //Creation of a ref texture
        for (i = 0; i < 1980; ++i)
        {
            for (j = 0; j < 1080; ++j)
            {
                if (!compareColor(withLine.GetPixel(i, j), verif))
                {
                    text.SetPixel(i, j, Color.clear);
                }
                else
                {
                    if(imin > i)
                    {
                        imin = i;
                    }
                    if(imax < i)
                    {
                        imax = i;
                    }
                    if (jmin > j)
                    {
                        jmin = j;
                    }
                    if (jmax < j)
                    {
                        jmax = j;
                    }
                    text.SetPixel(i, j, new Color(withoutLine.GetPixel(i, j).r, withoutLine.GetPixel(i, j).g, withoutLine.GetPixel(i, j).b, 1f));
                }
            }
        }
        text.Apply();

        Texture2D finalText = new Texture2D(imax - imin, jmax-jmin, TextureFormat.ARGB32, false);
        for(i=0; i < imax-imin; i++)
        {
            for (j = 0; j < jmax - jmin; j++)
            {
                finalText.SetPixel(i, j, text.GetPixel(i+imin, j+jmin));
            }
        }
        finalText.Apply();

        textures.Add(finalText);

        GameObject go;
        go = Instantiate(plane) as GameObject;
        //go.transform.parent = transform;
        go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, go.transform.localScale.z * (jmax - jmin)/(imax - imin)) ;

        go.GetComponent<MeshRenderer>().material.mainTexture = textures[numOfText - 1];

        Debug.Log("End");
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1980, 1080, TextureFormat.ARGB32, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    bool compareColor(Color color1, Color color2)
    {
        return (color1.r <= color2.r + 0.003f && color1.r >= color2.r - 0.003f && color1.g <= color2.g + 0.003f && color1.g >= color2.g - 0.003f && color1.b <= color2.b + 0.003f && color1.b >= color2.b - 0.003f);
    }
}
