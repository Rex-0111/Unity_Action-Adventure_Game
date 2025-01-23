using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DisableCanvas",5f);
    }

    void DisableCanvas()
    {
        gameObject.SetActive(false);
    }
}
