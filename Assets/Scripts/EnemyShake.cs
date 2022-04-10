using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShake : MonoBehaviour
{
    public CombatManager combatManager;
    Vector3 initialPosition;
    float shakeMagnitude = 0.7f;
    private Transform transform;

    void Awake()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (combatManager.enemyMove)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
        }
        else
        {
            transform.localPosition = initialPosition;
        }
    }
}
