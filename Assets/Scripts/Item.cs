using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Item : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        target.GetComponent<RotObject>().autoRotate(90.0f, false);

        Destroy(gameObject);
    }
}
