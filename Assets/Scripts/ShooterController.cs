using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public BulletController bulletPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            Debug.Log("Firing");

            Camera cam = Camera.main;
            Vector3 mousePos = Input.mousePosition;
            //Vector3 start = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
            Vector3 aim = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.fieldOfView));

            BulletController bullet = Instantiate(bulletPrefab,  transform, true);
            
            bullet.transform.position = cam.transform.position + new Vector3(0, 0, 1.5f);
            bullet.transform.LookAt(aim + new Vector3(0, 0, 50));

            bullet.GetComponent<Rigidbody>().AddForce(new Vector3(10, 10, 20) +  aim, ForceMode.Impulse);

        }
    }

}