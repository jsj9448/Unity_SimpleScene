using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    Rigidbody m_rigidBody;
    float m_bulletSpeed = 8.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        if(m_rigidBody)
        {
            m_rigidBody.velocity = transform.up * m_bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Target")
        {
            // Destroys the target
            Destroy(other.gameObject);

            //Destroy the bullet
            Destroy(gameObject);
        }
    }
}
