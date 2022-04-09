using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPixelController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(-1000, 1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("Click Detected!", gameObject);
    }

}
