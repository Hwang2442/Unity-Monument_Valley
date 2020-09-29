using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Item : MonoBehaviour
{
    public GameObject target;

    bool targetActive;

    int maxChild;
    int curChild;

    void Start()
    {
        targetActive = false;

        maxChild = target.transform.childCount;
        curChild = 0;

        for (int i = 0; i < maxChild; i++)
        {
            target.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.GetChild(0).gameObject.SetActive(false);

        StartCoroutine(targetCreate());
    }

    IEnumerator targetCreate()
    {
        while(curChild < maxChild)
        {
            target.transform.GetChild(curChild).gameObject.SetActive(true);

            yield return new WaitForSeconds(1.0f);

            curChild++;
        }

        Destroy(gameObject);
    }
}
