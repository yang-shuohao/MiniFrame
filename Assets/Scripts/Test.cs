using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab;
    private GameObject go;
    // Start is called before the first frame update
    void Start()
    {
        go = Instantiate(prefab);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(go)
            {
                Debug.LogWarning("go is not null");
            }
            else
            {
                Debug.LogWarning("go is null");
            }
        }
        else if(Input.GetKeyDown(KeyCode.Q))
        {
            Destroy(go);
        }
    }
}
