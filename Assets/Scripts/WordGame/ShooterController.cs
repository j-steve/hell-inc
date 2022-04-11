using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    [SerializeField] GameObject bulletEjectionPoint;
    [SerializeField] GameObject bulletContainer;
    [SerializeField] BulletController bulletPrefab;
    [SerializeField] float shooterMoveSpeed = 50;

    public void Initialize()
    {
        // Reset the scene.
        foreach(BulletController bullet in bulletContainer.GetComponentsInChildren<BulletController>()) {
            Destroy(bullet.gameObject);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.localPosition;
        newPosition.y += Input.GetAxis("Vertical") * Time.deltaTime * shooterMoveSpeed;
        transform.localPosition = newPosition;
        if (Input.GetButtonDown("Jump")) {
            BulletController bullet = Instantiate(bulletPrefab, bulletEjectionPoint.transform.position, Quaternion.identity, bulletContainer.transform);
            bullet.GetComponent<Rigidbody>().AddForce(new Vector3(100, 0, 0), ForceMode.Impulse);
        }
    }

}