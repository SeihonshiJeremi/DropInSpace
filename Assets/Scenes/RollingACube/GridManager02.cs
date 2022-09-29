using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager02 : MonoBehaviour
{
    public int columnLength, forwardLength;
    public float x_Space, z_Space;
    public GameObject prefab;

    void Start()
    {
        for(int i = 0; i < columnLength * forwardLength; i++)
        {
            Instantiate(prefab, new Vector3(x_Space * (i % columnLength), 0, z_Space * (i / columnLength)), Quaternion.identity);
        }
    }

}
