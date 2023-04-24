using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNotRotatateWithParent : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}
