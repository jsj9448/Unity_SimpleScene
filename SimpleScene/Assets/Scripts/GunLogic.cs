using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunLogic : MonoBehaviour
{
    [SerializeField]
    GameObject m_bulletPrefab;

    [SerializeField]
    Transform m_bulletSpawnPoint;

    const float MAX_COOLDOWN = 0.5f;
    float m_currentCooldown = 0.0f;

    const int MAX_AMMO = 5;
    int m_ammoCount = MAX_AMMO;               // number of bullets

    [SerializeField]
    Text m_ammoText;

    [SerializeField]
    AudioClip m_pistolShot;

    [SerializeField]
    AudioClip m_pistolEmpty;

    [SerializeField]
    AudioClip m_pistolReload;

    AudioSource m_audioSource;

    bool m_isEquipped = false;

    Rigidbody m_rigidBody;

    Collider m_collider;

    // Start is called before the first frame update
    void Start()
    {
        SetAmmoText();

        m_audioSource = GetComponent<AudioSource>();

        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    void SetAmmoText()
    {
        if(m_ammoText)
        {
            m_ammoText.text = "Ammo: " + m_ammoCount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isEquipped)
        {
            return;
        }

        if(m_currentCooldown > 0.0f)
        {
            m_currentCooldown -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Fire1") && m_currentCooldown <= 0.0f)
        {
            if(m_ammoCount > 0)
            {
                if (m_bulletPrefab && m_bulletSpawnPoint)
                {
                    Instantiate(m_bulletPrefab, m_bulletSpawnPoint.position, transform.rotation * m_bulletPrefab.transform.rotation);

                    m_currentCooldown = MAX_COOLDOWN;
                    --m_ammoCount;

                    SetAmmoText();

                    PlaySound(m_pistolShot);
                }
            }
            else
            {
                PlaySound(m_pistolEmpty);
            }
        }
    }

    void PlaySound(AudioClip sound)
    {
        if(m_audioSource && sound)
        {
            m_audioSource.PlayOneShot(sound);
        }
    }

    public void RefillAmmo()
    {
        PlaySound(m_pistolReload);
        m_ammoCount = MAX_AMMO;
        SetAmmoText();
    }

    public void EquipGun()
    {
        m_isEquipped = true;

        if(m_rigidBody)
        {
            m_rigidBody.useGravity = false;
        }

        if(m_collider)
        {
            m_collider.enabled = false;
        }
    }

    public void UnequipGun()
    {
        m_isEquipped = false;

        if (m_rigidBody)
        {
            m_rigidBody.useGravity = true;
        }

        if (m_collider)
        {
            m_collider.enabled = true;
        }
    }
}
