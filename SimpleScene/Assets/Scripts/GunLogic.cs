using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLogic : MonoBehaviour
{
    [SerializeField]
    GameObject m_bulletPrefab;

    [SerializeField]
    Transform m_bulletSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(m_bulletPrefab && m_bulletSpawnPoint)
            {
                Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, transform.rotation * m_bulletPrefab.transform.rotation);
            }
        }

    }
}
