using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform testTransform;
    public RectTransform testRectTransform;
    public Bounds bounds;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.LogWarning(bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(testRectTransform));
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {

        }
    }

   
}
