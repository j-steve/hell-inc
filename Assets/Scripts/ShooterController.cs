using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField] GameObject bulletEjectionPoint;
    [SerializeField] GameObject bulletContainer;
    [SerializeField] BulletController bulletPrefab;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.localPosition; 
        newPosition.y += Input.GetAxis("Vertical") * Time.deltaTime * 20;
        transform.localPosition = newPosition;

        if (Input.GetButtonDown("Jump")) {
        Debug.Log("Firing");

        //Camera cam = Camera.main;
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 start = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        //Vector3 aim = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.fieldOfView));
        BulletController bullet = Instantiate(bulletPrefab,  bulletEjectionPoint.transform.position, Quaternion.identity, bulletContainer.transform);


            bullet.GetComponent<Rigidbody>().AddForce(new Vector3(100, 0, 0), ForceMode.Impulse);

    }
    }

}