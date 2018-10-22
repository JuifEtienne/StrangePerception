using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColliderInstanciate : MonoBehaviour {

    //BoxCollider[] boxs;
    //int currentBox = -1;

	void OnEnable () {
        LineRenderer points = GetComponent<LineRenderer>();
        Vector3[] tab = new Vector3[points.positionCount];
        int s = points.GetPositions(tab);

        for (int i = 0; i < s-1; i++) {
            //create the collider for the line
            //BoxCollider lineCollider = new GameObject("LineCollider").AddComponent<BoxCollider>();

            BoxCollider lineCollider = (BoxCollider) gameObject.AddComponent<BoxCollider>();
            lineCollider.isTrigger = true;
            //set the collider as a child of your line
            //lineCollider.transform.parent = points.transform;
            // get width of collider from line 
            float lineWidth = points.endWidth;
            // get the length of the line using the Distance method
            float lineLength = Vector3.Distance(tab[i], tab[i + 1]);
            // size of collider is set where X is length of line, Y is width of line
            //z will be how far the collider reaches to the sky
            lineCollider.size = new Vector3(lineLength, lineWidth, 0.04f);
            // get the midPoint
            Vector3 midPoint = (tab[i] + tab[i + 1]) / 2;
            // move the created collider to the midPoint
            lineCollider.center = midPoint;


            //heres the beef of the function, Mathf.Atan2 wants the slope, be careful however because it wants it in a weird form
            //it will divide for you so just plug in your (y2-y1),(x2,x1)
            //float angle = Mathf.Atan2((tab[i + 1].z - tab[i].z), (tab[i + 1].x - tab[i].x));

            // angle now holds our answer but it's in radians, we want degrees
            // Mathf.Rad2Deg is just a constant equal to 57.2958 that we multiply by to change radians to degrees
            //angle *= Mathf.Rad2Deg;

            //were interested in the inverse so multiply by -1
            //angle *= -1;
            // now apply the rotation to the collider's transform, carful where you put the angle variable
            // in 3d space you don't wan't to rotate on your y axis
            //lineCollider.transform.Rotate(0, angle, 0);

        }

        gameObject.GetComponent<isInteractibleLine>().box = gameObject.GetComponents<BoxCollider>();
    }

}
