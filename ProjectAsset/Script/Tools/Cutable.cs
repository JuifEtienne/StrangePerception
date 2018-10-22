using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cutable : MonoBehaviour {


    List<GameObject> objectsToCut;


    Vector4 equation = new Vector4();
    int[] indexTriangles;
    List<Vector3> vertices;
    List<Vector2> uvs;
    List<Vector3> intersectionPoint;


    List<Vector3> verticesLeft;
    List<Vector3> normalesLeft;
    List<Vector2> uvLeft;
    List<Vector3> verticesRight;
    List<Vector3> normalesRight;
    List<Vector2> uvRight;
    List<int> indexLeft;
    List<int> indexRight;

    private enum Side { LEFT = 1, RIGHT };

    public Valve.VR.InteractionSystem.Hand hand;
    public GameObject handsManager;
    Vector3[] points;
    Vector3 normale;

    public GameObject testPlane;

    public GameObject toInstantiate;
    public GameObject lineInstantiate;

    GameObject toCut;

    bool started = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "cutable")
        {
            if (!objectsToCut.Contains(other.gameObject))
            {
                objectsToCut.Add(other.gameObject);
                Debug.Log("Done");
            }
            
        }
    }

    private void Awake()
    {
        objectsToCut = new List<GameObject>();
        points = new Vector3[3];
        gameObject.GetComponent<MeshCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update () {
        if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            gameObject.GetComponent<MeshCollider>().enabled = true;
            objectsToCut = new List<GameObject>();
            started = true;
            points[0] = transform.position;
            points[1] = transform.position + 0.05f * transform.forward;
        }

        if (hand.controller.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && started)
        {
            gameObject.GetComponent<MeshCollider>().enabled = false;
            points[2] = transform.position;
            started = false;
            Debug.Log(objectsToCut.Count);
            equation = equationCalcul();
            //testForPlane();
            for(int i = 0; i < objectsToCut.Count; ++i)
            {
                if(objectsToCut[i].GetComponent<LineRenderer>() == null)
                {
                    cut(objectsToCut[i]);
                }
                else
                {
                    Debug.Log("HERE");
                    cutLine(objectsToCut[i]);
                }
            }
            objectsToCut = new List<GameObject>();
        }

        /*if (wasHere && isEnded)
        {
            Debug.Log("Here");

            points = blade.GetComponent<Valve.VR.InteractionSystem.CreationPlane>().points;

            wasHere = false;
            isEnded = false;
            //Information of the object
            
        }*/
    }
    
    //Return if the point is on the left or the right of the plane
    Side pointSide(Vector3 point)
    {
        if(point.x * equation.x + point.y * equation.y + point.z * equation.z + equation.w >= 0)
        {
            return Side.LEFT;
        }
        else
        {
            return Side.RIGHT;
        }
    }

    //Return the coefficient k which will give the intersection between a line and the plane
    float coeffPlaneLine(Vector3 begin, Vector3 end)
    {
        return (-begin.x * equation.x - begin.y * equation.y - begin.z * equation.z - equation.w) / (equation.x * (end.x - begin.x) + equation.y * (end.y - begin.y) + equation.z * (end.z - begin.z));
    }

    //Return the intersection point
    Vector3 getIntersectionPoint(Vector3 begin, Vector3 end, float k)
    {
        return new Vector3(k * end.x + (1 - k) * begin.x, k * end.y + (1 - k) * begin.y, k * end.z + (1 - k) * begin.z);
    }

    //Return the uv of the intersection point
    Vector2 getUVIntersectionPoint(Vector2 uvBegin, Vector2 uvEnd, float k)
    {
        return new Vector2(k * uvEnd.x + (1 - k) * uvBegin.x, k * uvEnd.y + (1 - k) * uvBegin.y);
    }


    //________________________________ADD TRIANGLE LEFT_________________________________///
    void addTriangleLeft(Vector3 first, Vector3 second, Vector3 third)
    {
        int size = verticesLeft.Count;
        indexLeft.Add(size);
        indexLeft.Add(size + 1);
        indexLeft.Add(size + 2);
        verticesLeft.Add(toCut.transform.InverseTransformPoint(first));
        verticesLeft.Add(toCut.transform.InverseTransformPoint(second));
        verticesLeft.Add(toCut.transform.InverseTransformPoint(third));

        Vector3 normale = Vector3.Cross(third - second, first - second);
        normalesLeft.Add(normale);
        normalesLeft.Add(normale);
        normalesLeft.Add(normale);
    }

    //WITH UV
    void addTriangleLeft(Vector3 first, Vector3 second, Vector3 third, Vector2[] uv)
    {
        int size = verticesLeft.Count;
        indexLeft.Add(size);
        indexLeft.Add(size + 1);
        indexLeft.Add(size + 2);
        verticesLeft.Add(toCut.transform.InverseTransformPoint(first));
        uvLeft.Add(uv[0]);
        verticesLeft.Add(toCut.transform.InverseTransformPoint(second));
        uvLeft.Add(uv[1]);
        verticesLeft.Add(toCut.transform.InverseTransformPoint(third));
        uvLeft.Add(uv[2]);

        Vector3 normale = Vector3.Cross(third - second, first - second);
        normalesLeft.Add(normale);
        normalesLeft.Add(normale);
        normalesLeft.Add(normale);
    }

    //WITH NORMALS
    void addTriangleLeft(Vector3 first, Vector3 second, Vector3 third, Vector3 normal)
    {
        Vector3 normal2 = Vector3.Cross(first - second, third - second);

        int size = verticesLeft.Count;
        indexLeft.Add(size);
        indexLeft.Add(size + 1);
        indexLeft.Add(size + 2);
        
        if (Vector3.Dot(normal2, normal) >= 0)
        {
            verticesLeft.Add(toCut.transform.InverseTransformPoint(first));
            verticesLeft.Add(toCut.transform.InverseTransformPoint(second));
            verticesLeft.Add(toCut.transform.InverseTransformPoint(third));

            Vector3 normale = Vector3.Cross(third - second, first - second);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
        }
        else
        {
            verticesLeft.Add(toCut.transform.InverseTransformPoint(third));
            verticesLeft.Add(toCut.transform.InverseTransformPoint(second));
            verticesLeft.Add(toCut.transform.InverseTransformPoint(first));

            Vector3 normale = Vector3.Cross(first - second, third - second);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
        }
    }

    //WITH UV AND NORMALS
    void addTriangleLeft(Vector3 first, Vector3 second, Vector3 third, Vector3 normal, Vector2[] uv)
    {
        Vector3 normal2 = Vector3.Cross(first - second, third - second);

        int size = verticesLeft.Count;
        indexLeft.Add(size);
        indexLeft.Add(size + 1);
        indexLeft.Add(size + 2);

        if (Vector3.Dot(normal2, normal) >= 0)
        {
            verticesLeft.Add(toCut.transform.InverseTransformPoint(first));
            uvLeft.Add(uv[0]);
            verticesLeft.Add(toCut.transform.InverseTransformPoint(second));
            uvLeft.Add(uv[1]);
            verticesLeft.Add(toCut.transform.InverseTransformPoint(third));
            uvLeft.Add(uv[2]);

            Vector3 normale = Vector3.Cross(third - second, first - second);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
        }
        else
        {
            verticesLeft.Add(toCut.transform.InverseTransformPoint(third));
            uvLeft.Add(uv[2]);
            verticesLeft.Add(toCut.transform.InverseTransformPoint(second));
            uvLeft.Add(uv[1]);
            verticesLeft.Add(toCut.transform.InverseTransformPoint(first));
            uvLeft.Add(uv[0]);

            Vector3 normale = Vector3.Cross(first - second, third - second);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
            normalesLeft.Add(normale);
        }
    }


    //________________________________ADD TRIANGLE RIGHT_________________________________///

    void addTriangleRight(Vector3 first, Vector3 second, Vector3 third)
    {
        int size = verticesRight.Count;
        indexRight.Add(size);
        indexRight.Add(size + 1);
        indexRight.Add(size + 2);
        verticesRight.Add(toCut.transform.InverseTransformPoint(first));
        verticesRight.Add(toCut.transform.InverseTransformPoint(second));
        verticesRight.Add(toCut.transform.InverseTransformPoint(third));

        Vector3 normale = Vector3.Cross(third - second, first - second);
        normalesRight.Add(normale);
        normalesRight.Add(normale);
        normalesRight.Add(normale);
    }

    //WITH UV
    void addTriangleRight(Vector3 first, Vector3 second, Vector3 third, Vector2[] uv)
    {
        int size = verticesRight.Count;
        indexRight.Add(size);
        indexRight.Add(size + 1);
        indexRight.Add(size + 2);
        verticesRight.Add(toCut.transform.InverseTransformPoint(first));
        uvRight.Add(uv[0]);
        verticesRight.Add(toCut.transform.InverseTransformPoint(second));
        uvRight.Add(uv[1]);
        verticesRight.Add(toCut.transform.InverseTransformPoint(third));
        uvRight.Add(uv[2]);

        Vector3 normale = Vector3.Cross(third - second, first - second);
        normalesRight.Add(normale);
        normalesRight.Add(normale);
        normalesRight.Add(normale);
    }

    //WITH NORMALS
    void addTriangleRight(Vector3 first, Vector3 second, Vector3 third, Vector3 normal)
    {
        Vector3 normal2 = Vector3.Cross(first - second, third - second);

        int size = verticesRight.Count;
        indexRight.Add(size);
        indexRight.Add(size + 1);
        indexRight.Add(size + 2);

        if (Vector3.Dot(normal2, normal) >= 0)
        {
            
            verticesRight.Add(toCut.transform.InverseTransformPoint(first));
            verticesRight.Add(toCut.transform.InverseTransformPoint(second));
            verticesRight.Add(toCut.transform.InverseTransformPoint(third));

            Vector3 normale = Vector3.Cross(third - second, first - second);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
        }
        else
        {
            verticesRight.Add(toCut.transform.InverseTransformPoint(third));
            verticesRight.Add(toCut.transform.InverseTransformPoint(second));
            verticesRight.Add(toCut.transform.InverseTransformPoint(first));

            Vector3 normale = Vector3.Cross(first - second, third - second);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
        }
    }

    //WITH NORMALS AND UVS
    void addTriangleRight(Vector3 first, Vector3 second, Vector3 third, Vector3 normal, Vector2[] uv)
    {
        Vector3 normal2 = Vector3.Cross(first - second, third - second);

        int size = verticesRight.Count;
        indexRight.Add(size);
        indexRight.Add(size + 1);
        indexRight.Add(size + 2);

        if (Vector3.Dot(normal2, normal) >= 0)
        {

            verticesRight.Add(toCut.transform.InverseTransformPoint(first));
            uvRight.Add(uv[0]);
            verticesRight.Add(toCut.transform.InverseTransformPoint(second));
            uvRight.Add(uv[1]);
            verticesRight.Add(toCut.transform.InverseTransformPoint(third));
            uvRight.Add(uv[2]);

            Vector3 normale = Vector3.Cross(third - second, first - second);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
        }
        else
        {
            verticesRight.Add(toCut.transform.InverseTransformPoint(third));
            uvRight.Add(uv[2]);
            verticesRight.Add(toCut.transform.InverseTransformPoint(second));
            uvRight.Add(uv[1]);
            verticesRight.Add(toCut.transform.InverseTransformPoint(first));
            uvRight.Add(uv[0]);

            Vector3 normale = Vector3.Cross(first - second, third - second);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
            normalesRight.Add(normale);
        }
    }



    //________________________________SEPARATE TRIANGLES_________________________________///

    //To create differents triangles, first and second are the points which are on the same side of the plane. The opposite one is the other. 
    //PositionBase is here to help the function to know where are the two points which are on the same side of the plane.
    void separateTriangles(int firstInter, int secondInter, Vector3 first, Vector3 second, Vector3 opposite, Side positionBase, Vector3[] intersectionPoints)
    {
        Vector3 middle = new Vector3((first.x + second.x) / 2, (first.y + second.y) / 2, (first.z + second.z) / 2);

        if(positionBase == Side.LEFT)
        {
            addTriangleLeft(first, intersectionPoints[firstInter], middle);
            addTriangleLeft(middle, intersectionPoints[firstInter], intersectionPoints[secondInter]);
            addTriangleLeft(intersectionPoints[secondInter], middle, second);


            //TEST
            addTriangleLeft(first, middle, intersectionPoints[firstInter]);
            addTriangleLeft(middle, intersectionPoints[secondInter] , intersectionPoints[firstInter]);
            addTriangleLeft(intersectionPoints[secondInter], second, middle);

            //The other side
            addTriangleRight(intersectionPoints[firstInter], intersectionPoints[secondInter], opposite);

            //TEST
            addTriangleRight(intersectionPoints[firstInter], opposite, intersectionPoints[secondInter]);
        }
        if (positionBase == Side.RIGHT)
        {
            addTriangleRight(first, intersectionPoints[firstInter], middle);
            addTriangleRight(middle, intersectionPoints[firstInter], intersectionPoints[secondInter]);
            addTriangleRight(intersectionPoints[secondInter], middle, second);

            //TEST
            addTriangleRight(first, middle, intersectionPoints[firstInter]);
            addTriangleRight(middle, intersectionPoints[secondInter], intersectionPoints[firstInter]);
            addTriangleRight(intersectionPoints[secondInter], second, middle);

            //The other side
            addTriangleLeft(intersectionPoints[firstInter], intersectionPoints[secondInter], opposite);

            //TEST
            addTriangleLeft(intersectionPoints[firstInter], opposite, intersectionPoints[secondInter]);
        }

    }


    //same but with the normals
    void separateTriangles(int firstInter, int secondInter, Vector3 first, Vector3 second, Vector3 opposite, Side positionBase, Vector3[] intersectionPoints, Vector3 normal)
    {
        Vector3 middle = new Vector3((first.x + second.x) / 2, (first.y + second.y) / 2, (first.z + second.z) / 2);

        if (positionBase == Side.LEFT)
        {
            addTriangleLeft(first, intersectionPoints[firstInter], middle, normal);
            addTriangleLeft(middle, intersectionPoints[firstInter], intersectionPoints[secondInter], normal);
            addTriangleLeft(intersectionPoints[secondInter], middle, second, normal);

            //The other side
            addTriangleRight(intersectionPoints[firstInter], intersectionPoints[secondInter], opposite, normal);

        }
        if (positionBase == Side.RIGHT)
        {
            addTriangleRight(first, intersectionPoints[firstInter], middle, normal);
            addTriangleRight(middle, intersectionPoints[firstInter], intersectionPoints[secondInter], normal);
            addTriangleRight(intersectionPoints[secondInter], middle, second, normal);

            //The other side
            addTriangleLeft(intersectionPoints[firstInter], intersectionPoints[secondInter], opposite, normal);
        }

    }

    //same but with the normals and the uvs
    void separateTriangles(int firstInter, int secondInter, Vector3 first, Vector3 second, Vector3 opposite, Side positionBase, Vector3[] intersectionPoints, Vector3 normal, Vector2[] uvPoint, Vector2[] uvInter)
    {
        Vector3 middle = new Vector3((first.x + second.x) / 2, (first.y + second.y) / 2, (first.z + second.z) / 2);
        Vector2 uvMiddle = new Vector2((uvPoint[0].x + uvPoint[1].x) / 2, (uvPoint[0].y + uvPoint[1].y) / 2);

        if (positionBase == Side.LEFT)
        {
            addTriangleLeft(first, intersectionPoints[firstInter], middle, normal, new Vector2[] {uvPoint[0], uvInter[firstInter], uvMiddle});
            addTriangleLeft(middle, intersectionPoints[firstInter], intersectionPoints[secondInter], normal, new Vector2[] { uvMiddle, uvInter[firstInter], uvInter[secondInter] });
            addTriangleLeft(intersectionPoints[secondInter], middle, second, normal, new Vector2[] { uvInter[secondInter], uvMiddle, uvPoint[1]});

            //The other side
            addTriangleRight(intersectionPoints[firstInter], intersectionPoints[secondInter], opposite, normal, new Vector2[] { uvInter[firstInter], uvInter[secondInter], uvPoint[2] });

        }
        if (positionBase == Side.RIGHT)
        {
            addTriangleRight(first, intersectionPoints[firstInter], middle, normal, new Vector2[] { uvPoint[0], uvInter[firstInter], uvMiddle });
            addTriangleRight(middle, intersectionPoints[firstInter], intersectionPoints[secondInter], normal, new Vector2[] { uvMiddle, uvInter[firstInter], uvInter[secondInter] });
            addTriangleRight(intersectionPoints[secondInter], middle, second, normal, new Vector2[] { uvInter[secondInter], uvMiddle, uvPoint[1] });

            //The other side
            addTriangleLeft(intersectionPoints[firstInter], intersectionPoints[secondInter], opposite, normal, new Vector2[] { uvInter[firstInter], uvInter[secondInter], uvPoint[2] });
        }

    }


    //________________________________CLOSE THE OBJECTS_________________________________///

    private float GridRanking(Vector3 Pos)
    {
        return Pos.x + 100 * Pos.z;
    }

  

    void closeObjects()
    {
        Vector3 center = new Vector3();

        for (int i = 0; i < intersectionPoint.Count; i++)
        {
            center += intersectionPoint[i];
        }
        center = center / intersectionPoint.Count;

        List<Vector3> triangles = new List<Vector3>();


        Debug.Log(intersectionPoint.Count);
        
        for (int i = 0; i < intersectionPoint.Count; i++)
        {
            triangles.Add(center);
            triangles.Add(intersectionPoint[i]);
            triangles.Add(intersectionPoint[getIndexLittleAngle(center, i, intersectionPoint.ToArray())]);
            
        }

        for (int i = 0; i < intersectionPoint.Count; i++)
        {
            triangles.Add(center);
            
            triangles.Add(intersectionPoint[getIndexLittleAngle(center, i, intersectionPoint.ToArray())]);
            triangles.Add(intersectionPoint[i]);
        }

        for (int i = 0; i < triangles.Count; i = i + 3)
        {
            addTriangleLeft(triangles[i], triangles[i + 1], triangles[i + 2], normale, new Vector2[] {new Vector2(0,0), new Vector2(0, 0), new Vector2(0, 0) });
            addTriangleRight(triangles[i], triangles[i + 1], triangles[i + 2], -normale, new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0)});
        }
    }





    //________________________________VERIF_________________________________///

    bool verif(Vector3 test)
    {
        
        int i = 0;
        bool check = true;
        //Debug.Log("HERE MOTHAFUCKA");

        for (i = 0; i < intersectionPoint.Count; ++i)
        {
            if (Mathf.Approximately(intersectionPoint[i].x, test.x) &&
                Mathf.Approximately(intersectionPoint[i].y, test.y) &&
                Mathf.Approximately(intersectionPoint[i].z, test.z))
            {
                check = false;
            }
        }
        
        return check;
    }

    //________________________________ORDERING ELEMENTS_________________________________///

    float getAngle(Vector3 one, Vector3 two, Vector3 center)
    {
        Vector3 targetDir = one - center;
        Vector3 forward = two - center;
        float temp = Vector3.SignedAngle(targetDir, forward, normale);
        if (temp < 0)
        {
            return 180 + (180 + temp);
        }
        else
        {
            return temp;
        }
    }

    int getIndexLittleAngle(Vector3 center, int currentIndex, Vector3[] tab)
    {
        float angleMin = 360;
        int index = 0;
        if(currentIndex == 0)
        {
            index = 1;
        }
        int i;
        for(i=0; i<tab.Length; ++i)
        {
            if(i != currentIndex)
            {
                if (getAngle(tab[currentIndex], tab[i], center) <= angleMin && 0 <= getAngle(tab[currentIndex], tab[i], center))
                {
                    angleMin = getAngle(tab[currentIndex], tab[i], center);
                    index = i;
                }
            }
        }
        //Debug.Log("CURRENT AND INDEX");
        //Debug.Log(currentIndex);
        //Debug.Log(index);
        return index;
    }


    //________________________________EQUATION FROM THREE POINTS_________________________________///

    Vector4 equationCalcul()
    {
        Vector4 equation = new Vector4();
        equation.x = (points[1].y - points[0].y)*(points[2].z - points[0].z) - (points[1].z - points[0].z) * (points[2].y - points[0].y);
        equation.y = (points[2].x - points[0].x) * (points[1].z - points[0].z) - (points[1].x - points[0].x) * (points[2].z - points[0].z);
        equation.z = (points[1].x - points[0].x) * (points[2].y - points[0].y) - (points[2].x - points[0].x) * (points[1].y - points[0].y);
        equation.w = -points[0].x*equation.x - points[0].y*equation.y - points[0].z*equation.z;

        normale.x = equation.x;
        normale.y = equation.y;
        normale.z = equation.z;
        return equation;
    }

    //________________________________TEST PLANE A DELETE_________________________________///

    void testForPlane()
    {
        equation = equationCalcul();
        //Vector3 A = new Vector3(10, 10, (-equation.w -10*equation.x -10*equation.y) / equation.z);
        //Vector3 B = new Vector3(-10, (-equation.w+10*equation.x+10 * equation.z) / equation.y, -10);
        //Vector3 C = new Vector3((-equation.w+10 * equation.z- 10 * equation.y) / equation.x, 10, -10);
        Vector3 A = points[0];
        Vector3 B = points[1];
        Vector3 C = points[2];
        GameObject test = Instantiate(testPlane) as GameObject;
        test.AddComponent<MeshFilter>();
        Mesh TEST = new Mesh();

        List<Vector3> vertex = new List<Vector3>();
        List<int> index = new List<int>();
        vertex.Add(A);
        vertex.Add(B);
        vertex.Add(C);
        vertex.Add(A);
        vertex.Add(C);
        vertex.Add(B);
        index.Add(0);
        index.Add(1);
        index.Add(2);
        index.Add(3);
        index.Add(4);
        index.Add(5);


        TEST.SetVertices(vertex);
        TEST.SetTriangles(index, 0);
        test.GetComponent<MeshFilter>().mesh = TEST;
    }


    //________________________________CUT ALGORITHM_________________________________///

    void cut(GameObject cutObject)
    {
        toCut = cutObject;
        indexTriangles = toCut.GetComponent<MeshFilter>().mesh.GetTriangles(0);
        vertices = new List<Vector3>();
        toCut.GetComponent<MeshFilter>().mesh.GetVertices(vertices);
        intersectionPoint = new List<Vector3>();
        uvs = new List<Vector2>();
        toCut.GetComponent<MeshFilter>().mesh.GetUVs(0, uvs);

        //Element for the new objects
        verticesLeft = new List<Vector3>();
        verticesRight = new List<Vector3>();
        normalesLeft = new List<Vector3>();
        normalesRight = new List<Vector3>();
        indexLeft = new List<int>();
        indexRight = new List<int>();
        uvLeft = new List<Vector2>();
        uvRight = new List<Vector2>();


        //Initialisation
        //LEFT
        GameObject leftPart = Instantiate(toInstantiate) as GameObject;
        leftPart.transform.position = toCut.transform.position - 0.2f * toCut.transform.up;
        leftPart.transform.localScale = toCut.transform.localScale;
        leftPart.transform.localRotation = toCut.transform.localRotation;
        leftPart.AddComponent<MeshFilter>();
        leftPart.AddComponent<MeshRenderer>();
        leftPart.GetComponent<Renderer>().material = toCut.GetComponent<Renderer>().material;
        

        //RIGHT
        GameObject rightPart = Instantiate(toInstantiate) as GameObject;
        rightPart.transform.position = toCut.transform.position + 0.2f * toCut.transform.up;
        rightPart.transform.localScale = toCut.transform.localScale;
        rightPart.transform.localRotation = toCut.transform.localRotation;
        rightPart.AddComponent<MeshFilter>();
        rightPart.AddComponent<MeshRenderer>();
        rightPart.GetComponent<Renderer>().material = toCut.GetComponent<Renderer>().material;
        

        //Collider
        if (toCut.GetComponent<SphereCollider>() != null)
        {
            leftPart.AddComponent<SphereCollider>();
            leftPart.GetComponent<isInteractableMesh>().enabled = false;
            leftPart.GetComponent<IsInteractibleSphere>().enabled = true;
            leftPart.GetComponent<IsInteractibleSphere>().handsManager = handsManager;
            leftPart.GetComponent<IsInteractibleSphere>().blade = gameObject;
            rightPart.AddComponent<SphereCollider>();
            rightPart.GetComponent<isInteractableMesh>().enabled = false;
            rightPart.GetComponent<IsInteractibleSphere>().enabled = true;
            rightPart.GetComponent<IsInteractibleSphere>().handsManager = handsManager;
            rightPart.GetComponent<IsInteractibleSphere>().blade = gameObject;
        }
        else
        {
            leftPart.AddComponent<MeshCollider>();
            leftPart.GetComponent<isInteractableMesh>().handsManager = handsManager;
            leftPart.GetComponent<isInteractableMesh>().blade = gameObject;
            rightPart.AddComponent<MeshCollider>();
            rightPart.GetComponent<isInteractableMesh>().handsManager = handsManager;
            rightPart.GetComponent<isInteractableMesh>().blade = gameObject;
        }


        Debug.Log(indexTriangles.Length);

        
        //Loop for the procution of the two meshs, look each triangle of the current mesh
        for (int i = 0; i < indexTriangles.Length; i = i + 3)
        {
            //Three points of my triangle
            Vector3 A = toCut.transform.TransformPoint(vertices[indexTriangles[i]]);
            Vector3 B = toCut.transform.TransformPoint(vertices[indexTriangles[i + 1]]);
            Vector3 C = toCut.transform.TransformPoint(vertices[indexTriangles[i + 2]]);


            Vector2 uvA = uvs[indexTriangles[i]];
            Vector2 uvB = uvs[indexTriangles[i + 1]];
            Vector2 uvC = uvs[indexTriangles[i + 2]];

            Side[] sideTriangle = new Side[3];
            sideTriangle[0] = pointSide(A);
            sideTriangle[1] = pointSide(B);
            sideTriangle[2] = pointSide(C);

            if (sideTriangle[0] == Side.LEFT && sideTriangle[1] == Side.LEFT && sideTriangle[2] == Side.LEFT)
            {
                addTriangleLeft(A, B, C, new Vector2[] { uvA, uvB, uvC });
            }
            else if (sideTriangle[0] == Side.RIGHT && sideTriangle[1] == Side.RIGHT && sideTriangle[2] == Side.RIGHT)
            {
                addTriangleRight(A, B, C, new Vector2[] { uvA, uvB, uvC });
            }
            else
            {

                //Check where are the two intersection points
                //AB
                float k1 = coeffPlaneLine(A, B);
                //BC
                float k2 = coeffPlaneLine(B, C);
                //CA
                float k3 = coeffPlaneLine(C, A);

                /*Debug.Log("K");
                Debug.Log(k1);
                Debug.Log(k2);
                Debug.Log(k3);*/

                Vector3[] pointsInter = new Vector3[3]; //to know which segment has a point
                Vector2[] uvInter = new Vector2[3];
                int[] which = new int[3];
                which[0] = 0;
                which[1] = 0;
                which[2] = 0;

                //Initialize the 2 intersection points, the other one will not change
                if (k1 < 1 && 0 < k1) //AB
                {
                    pointsInter[0] = getIntersectionPoint(A, B, k1);
                    uvInter[0] = getUVIntersectionPoint(uvA, uvB, k1);
                    which[0] = 1;
                }
                if (k2 < 1 && 0 < k2) //BC 
                {
                    pointsInter[1] = getIntersectionPoint(B, C, k2);
                    uvInter[1] = getUVIntersectionPoint(uvB, uvC, k2);
                    which[1] = 1;
                }
                if (k3 < 1 && 0 < k3) //CA
                {
                    pointsInter[2] = getIntersectionPoint(C, A, k3);
                    uvInter[2] = getUVIntersectionPoint(uvC, uvA, k3);
                    which[2] = 1;
                }

                //Add them to the list
                for (int k = 0; k < 3; ++k)
                {
                    if (which[k] == 1)
                    {
                        if (verif(pointsInter[k]))
                        {
                            intersectionPoint.Add(pointsInter[k]);
                        }
                    }
                }

                //Creation of the two Mesh
                //AB and BC so A and C are on the same side
                if (which[0] == 1 && which[1] == 1)
                {
                    separateTriangles(0, 1, A, C, B, sideTriangle[0], pointsInter, Vector3.Cross(A - B, C - B), new Vector2[] { uvA, uvC, uvB }, uvInter);
                }

                //BC and CA so A and B are on the same side
                if (which[1] == 1 && which[2] == 1)
                {
                    separateTriangles(2, 1, A, B, C, sideTriangle[0], pointsInter, Vector3.Cross(A - B, C - B), new Vector2[] { uvA, uvB, uvC }, uvInter);
                }
                //AB and CA so B and C are on the same side
                if (which[0] == 1 && which[2] == 1)
                {
                    separateTriangles(0, 2, B, C, A, sideTriangle[1], pointsInter, Vector3.Cross(A - B, C - B), new Vector2[] { uvB, uvC, uvA }, uvInter);
                }
            }

        }
        closeObjects();

        Mesh meshLeft = new Mesh();
        Mesh meshRight = new Mesh();

        meshLeft.SetVertices(verticesLeft);
        meshLeft.SetTriangles(indexLeft, 0);
        leftPart.GetComponent<MeshFilter>().mesh = meshLeft;
        leftPart.GetComponent<MeshFilter>().mesh.SetUVs(0, uvLeft);
        leftPart.GetComponent<MeshFilter>().mesh.normals = normalesLeft.ToArray();

        meshRight.SetVertices(verticesRight);
        meshRight.SetTriangles(indexRight, 0);
        rightPart.GetComponent<MeshFilter>().mesh = meshRight;
        rightPart.GetComponent<MeshFilter>().mesh.SetUVs(0, uvRight);
        rightPart.GetComponent<MeshFilter>().mesh.normals = normalesRight.ToArray();

        if (toCut.GetComponent<SphereCollider>() != null)
        {
            leftPart.GetComponent<SphereCollider>().radius = toCut.GetComponent<SphereCollider>().radius;
            leftPart.GetComponent<SphereCollider>().isTrigger = true;
            rightPart.GetComponent<SphereCollider>().radius = toCut.GetComponent<SphereCollider>().radius;
            rightPart.GetComponent<SphereCollider>().isTrigger = true;
        }
        else
        {
            leftPart.GetComponent<MeshCollider>().convex = true;
            leftPart.GetComponent<MeshCollider>().isTrigger = true;
            leftPart.GetComponent<MeshCollider>().sharedMesh = meshLeft;

            rightPart.GetComponent<MeshCollider>().convex = true;
            rightPart.GetComponent<MeshCollider>().isTrigger = true;
            rightPart.GetComponent<MeshCollider>().sharedMesh = meshRight;
        }

        Destroy(toCut);
    }

    //________________________________FOR LINE RENDERER_________________________________///

    float distancePointEquation(Vector3 point)
    {
        return Mathf.Abs(equation.x * point.x + equation.y * point.y + equation.z * point.z + equation.w) / Mathf.Sqrt(equation.x * equation.x + equation.y * equation.y + equation.z * equation.z);
    }

    void cutLine(GameObject cutObject)
    {
        toCut = cutObject;
        LineRenderer line = toCut.GetComponent<LineRenderer>();
        Vector3[] listPoint = new Vector3[line.positionCount];
        line.GetPositions(listPoint);

        //Get the closest point to the plane
        float min = distancePointEquation(listPoint[0]);
        int index = 0;
        for(int i=0; i < line.positionCount; ++i)
        {
            if(distancePointEquation(listPoint[i]) < min)
            {
                min = distancePointEquation(listPoint[i]);
                index = i;
            }
        }

        Debug.Log(index);

        GameObject leftPart = Instantiate(lineInstantiate) as GameObject;
        leftPart.GetComponent<isInteractibleLine>().handsManager = handsManager;
        leftPart.GetComponent<isInteractibleLine>().blade = gameObject;
        GameObject rightPart = Instantiate(lineInstantiate) as GameObject;
        rightPart.GetComponent<isInteractibleLine>().handsManager = handsManager;
        rightPart.GetComponent<isInteractibleLine>().blade = gameObject;

        List<Vector3> posLeft = new List<Vector3>();
        List<Vector3> posRight = new List<Vector3>();

        for (int i = 0; i < index; ++i)
        {
            posLeft.Add(listPoint[i]);
        }

        for (int i = index; i < line.positionCount; ++i)
        {
            posRight.Add(listPoint[i]);
        }

        leftPart.GetComponent<LineRenderer>().positionCount = posLeft.Count;
        leftPart.GetComponent<LineRenderer>().SetPositions(posLeft.ToArray());
        leftPart.GetComponent<LineColliderInstanciate>().enabled = false;
        leftPart.GetComponent<LineColliderInstanciate>().enabled = true;

        rightPart.GetComponent<LineRenderer>().positionCount = posRight.Count;
        rightPart.GetComponent<LineRenderer>().SetPositions(posRight.ToArray());
        rightPart.GetComponent<LineColliderInstanciate>().enabled = false;
        rightPart.GetComponent<LineColliderInstanciate>().enabled = true;
        rightPart.transform.position = rightPart.transform.position + 0.1f*gameObject.transform.forward;
        Destroy(toCut);
    }

}


    