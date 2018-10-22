using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLineAndTrian : MonoBehaviour
{
    public Material mat;
    //CreationSprite spr;
    public LineRenderer points;
    public bool capture = false;

    private void Start()
    {
        //spr = GetComponent<CreationSprite>();

        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();
        MeshRenderer meshRend = GetComponent<MeshRenderer>();
        meshRend.material = mat;
    }

    private void Update()
    {
        if (points.positionCount > 3)
        {



            //if (!mat)
            //{
            //    Debug.LogError("Please Assign a material on the inspector");
            //    return;
            //}

            List<int> indice = new List<int>();
            List<Vector3> trian = new List<Vector3>();


            Vector3[] tab = new Vector3[points.positionCount];
            int s = points.GetPositions(tab);

            Debug.Log(s);

            Vector3[] triangles = trianguler_polygone(tab);


            for (int i = 0; i < triangles.Length; i = i + 3)
            {
                trian.Add(triangles[i]);
                trian.Add(triangles[i+1]);
                trian.Add(triangles[i+2]);
                trian.Add(triangles[i]);
                trian.Add(triangles[i + 2]);
                trian.Add(triangles[i + 1]);
            }

            int size = trian.ToArray().Length;

            Mesh mesh = GetComponent<MeshFilter>().mesh;

            mesh.Clear();

            // make changes to the Mesh by creating arrays which contain the new values
            mesh.vertices = trian.ToArray();

            Vector2[] uv = new Vector2[size];
            for(int i = 0; i < uv.Length; ++i)
            {
                uv[i] = new Vector2(0, 0);
            }
            mesh.uv = uv;
        
            int[] index = new int[size];

            for (int i = 0; i < size; i = i + 3)
            {
                index[i] = i;
                index[i+1] = i+1;
                index[i+2] = i+2;
            }


            mesh.triangles = index;
           

            mesh.SetTriangles(index, mesh.subMeshCount);



            if (capture)
            {
                //spr.creation();
                capture = false;
            }
        }
    }





    /*
     * TRIANGULATION SYSTEM
     * 
     * 
     */

    int voisin_sommet(int n, int i, int di)
    {
        return ((i+di) % n + n) % n;
    }

    float equation_droite(Vector3 P0, Vector3 P1, Vector3 M)
    {
        return (P1.x - P0.x) * (M.y - P0.y) - (P1.y - P0.y) * (M.x - P0.x);
    }

    bool point_dans_triangle(Vector3[] triangle, Vector3 M)
    {
        Vector3 P0 = triangle[0];
        Vector3 P1 = triangle[1];
        Vector3 P2 = triangle[2];

        return (equation_droite(P0, P1, M) > 0) && (equation_droite(P1, P2, M) > 0) && (equation_droite(P2, P0, M) > 0);
    }

    /*indices correspond aux indices de P0 P1 et P2 dans le tableau Polygone. Il est donc de taille 3
     * 
     */
    int sommet_distance_maximale(Vector3[] polygone, Vector3 P0, Vector3 P1, Vector3 P2, int[] indices)
    {
        int n = polygone.Length;
        float distance = 0f;
        int j = -1;
        Vector3[] tab = new Vector3[3];
        tab[0] = P0;
        tab[1] = P1;
        tab[2] = P2;
        for (int i = 0; i < n; ++i)
        {
            if (i != indices[0] && i != indices[1] && i != indices[2])
            {
                Vector3 M = polygone[i];
                if (point_dans_triangle(tab, M))
                {
                    float d = Mathf.Abs(equation_droite(P1, P2, M));
                    if (d > distance)
                    {
                        distance = d;
                        j = i;
                    }
                }
            }
        }
        return j;
    }

    int sommet_gauche(Vector3[] polygone)
    {
        int n = polygone.Length;
        float x = polygone[0].x;
        int j = 0;
        for (int i = 0; i < n; ++i)
        {
            if (polygone[i].x < x)
            {
                x = polygone[i].x;
                j = i;
            }
        }
        return j;
    }

    Vector3[] nouveau_polygone(Vector3[] polygone, int i_debut, int i_fin)
    {
        int n = polygone.Length;
        List<Vector3> p = new List<Vector3>();
        int i = i_debut;
        while (i != i_fin)
        {
            p.Add(polygone[i]);
            i = voisin_sommet(n, i, 1);
        }
        p.Add(polygone[i_fin]);
        return p.ToArray();
    }

    void trianguler_polygone_recursif(Vector3[] polygone, List<Vector3> liste_triangles)
    {
        int n = polygone.Length;
        int j0 = sommet_gauche(polygone);
        int j1 = voisin_sommet(n, j0, 1);
        int j2 = voisin_sommet(n, j0, -1);
        Vector3 P0 = polygone[j0];
        Vector3 P1 = polygone[j1];
        Vector3 P2 = polygone[j2];
        int[] indices = { j0, j1, j2 };
        int j = sommet_distance_maximale(polygone, P0, P1, P2, indices);
        if (j == -1)
        {
            liste_triangles.Add(P0);
            liste_triangles.Add(P1);
            liste_triangles.Add(P2);
            Vector3[] polygone_1 = nouveau_polygone(polygone, j1, j2);
            if (polygone_1.Length == 3)
            {
                liste_triangles.Add(polygone_1[0]);
                liste_triangles.Add(polygone_1[1]);
                liste_triangles.Add(polygone_1[2]);
            }
            else
            {
                trianguler_polygone_recursif(polygone_1, liste_triangles);
            }
        }
        else
        {
            Vector3[] polygone_1 = nouveau_polygone(polygone, j0, j);
            Vector3[] polygone_2 = nouveau_polygone(polygone, j, j0);
            if (polygone_1.Length == 3)
            {
                liste_triangles.Add(polygone_1[0]);
                liste_triangles.Add(polygone_1[1]);
                liste_triangles.Add(polygone_1[2]);
            }
            else
            {
                trianguler_polygone_recursif(polygone_1, liste_triangles);
            }
            if (polygone_2.Length == 3)
            {
                liste_triangles.Add(polygone_2[0]);
                liste_triangles.Add(polygone_2[1]);
                liste_triangles.Add(polygone_2[2]);
            }
            else
            {
                trianguler_polygone_recursif(polygone_2, liste_triangles);
            }
        }
    }


    Vector3[] trianguler_polygone(Vector3[] polygone)
    {
        List<Vector3> liste_triangles = new List<Vector3>();
        trianguler_polygone_recursif(polygone, liste_triangles);
        return liste_triangles.ToArray();
    }
}