using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    CharacterController m_characterController;

    float m_movementSpeed = 5.0f;

    float m_horizontalInput;
    float m_verticalInput;

    Vector3 m_movementInput;
    Vector3 m_movement;

    float m_jumpHeight = 0.6f;
    float m_gravity = 0.05f;
    bool m_jump = false;

    GameObject m_interactiveObject = null;

    GameObject m_equippedObject = null;

    [SerializeField]
    Transform m_weaponEquipmentPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

        m_movementInput = new Vector3(m_horizontalInput, 0, m_verticalInput);

        if(!m_jump && Input.GetButtonDown("Jump"))
        {
            m_jump = true;
        }

        if(m_interactiveObject && Input.GetButtonDown("Fire2"))
        {
            if(!m_equippedObject)
            {
                GunLogic gunLogic = m_interactiveObject.GetComponent<GunLogic>();
                if (gunLogic)
                {
                    // Sets the position of the gun
                    m_interactiveObject.transform.position = m_weaponEquipmentPosition.position;
                    m_interactiveObject.transform.rotation = m_weaponEquipmentPosition.rotation;
                    m_interactiveObject.transform.parent = gameObject.transform;

                    // Deactivates gravity, deactivates collider
                    gunLogic.EquipGun();

                    m_equippedObject = m_interactiveObject;
                }
            }
            else if(m_equippedObject)
            {
                GunLogic gunLogic = m_equippedObject.GetComponent<GunLogic>();
                if (gunLogic)
                {
                    // Sets the position of the gun
                    m_equippedObject.transform.parent = null;

                    // Activates gravity, activates collider
                    gunLogic.UnequipGun();

                    m_equippedObject = null;
                }
            }
        }
    }

    void RotateCharacterTowardsMouseCursor()
    {
        // 1. 화면 공간에서 마우스 위치 감지
        // 2. 플레이어의 월드 위치를 화면 공간으로 변환
        // 3. 마우스 화면 공간 위치에서 플레이어 화면 공간 위치 빼기
        // 4. 수학을 사용하여 벡터(Atan)에서 각도를 얻음
        // 5. 플레이어에 각도 회전 적용
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 direction = mousePos - playerPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }

    private void FixedUpdate()
    {
        //m_movement.x = m_horizontalInput * m_movementSpeed * Time.deltaTime;
        //m_movement.z = m_verticalInput * m_movementSpeed * Time.deltaTime;
        m_movement = m_movementInput * m_movementSpeed * Time.deltaTime;

        RotateCharacterTowardsMouseCursor();

        // Face movement direction
        // player가 이동 후에도 마지막 방향을 계속 주시
        /*if(m_movementInput != Vector3.zero)
        {
            // 이동 방향에 맞게 player가 고개를 돌림. Quaternion.Euler로 인게임 상에서 각도가 안맞는 것을 수정할 수 있음
            transform.forward = Quaternion.Euler(0, -90, 0) * m_movementInput.normalized;
        }*/

        // Apply gravity
        if(!m_characterController.isGrounded)
        {
            // if the character is not on the ground...
            if (m_movement.y > 0)
            {
                // we are jumping!
                m_movement.y -= m_gravity;
            }
            else
            {
                // 50% increase in gravity
                m_movement.y -= m_gravity * 1.5f;
            }
            
            
        }
        else
        {
            // if it standing on the ground...
            m_movement.y = 0;
        }

        // Setting jumpheight to movement y
        if(m_jump)
        {
            m_movement.y = m_jumpHeight;
            m_jump = false;
        }

        if(m_characterController)
        {
            m_characterController.Move(m_movement);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Weapon")
        {
            m_interactiveObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon" && m_interactiveObject == other.gameObject)
        {
            m_interactiveObject = null;
        }
    }
}
