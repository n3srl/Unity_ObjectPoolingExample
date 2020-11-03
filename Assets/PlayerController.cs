using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //Object pooler
    public int maximumInstanceCount = 3;

    private GameObject[] m_LeftInstances;
    private GameObject[] m_RightInstances;

    private bool m_CurrentSide = false;

    public Vector3 DropPosition;
    public Vector3 DropVelocity;

    public float Speed;

    public GameObject Aircraft;
    public GameObject BombPrefab;

    private Vector3 m_CurrentPosition;
    private Camera m_Camera;

    // Start is called before the first frame update
    public void Start()
    {
        Aircraft = this.gameObject;
        m_Camera = Camera.main;

        m_LeftInstances = new GameObject[maximumInstanceCount];
        m_RightInstances = new GameObject[maximumInstanceCount];

        for (int i = 0; i < maximumInstanceCount; i++)
        {
            // place the pile of unused objects somewhere far off the map
            m_LeftInstances[i] = Instantiate(BombPrefab, DropPosition, Quaternion.identity);

            // disable by default, these objects are not active yet.
            m_LeftInstances[i].active = false;

            // place the pile of unused objects somewhere far off the map
            m_RightInstances[i] = Instantiate(BombPrefab, -DropPosition, Quaternion.identity);

            // disable by default, these objects are not active yet.
            m_RightInstances[i].active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float roll = Input.GetAxis("Horizontal");


        m_CurrentPosition = Aircraft.transform.position;

        if (roll > 0)
        {
            m_CurrentPosition.x += Speed * Time.deltaTime;
        }

        if (roll < 0)
        {
            m_CurrentPosition.x -= Speed * Time.deltaTime;
        }


        Vector3 screenPoint = m_Camera.WorldToViewportPoint(m_CurrentPosition);
        if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
        {
            Aircraft.transform.position = m_CurrentPosition;
        }


        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Drop the bomb");
            GameObject instance = GetNextAvailiableInstance();
            //Drop bomb
            if (instance != null)
            {
                if (m_CurrentSide)
                    instance.transform.position = m_CurrentPosition + DropPosition;
                else
                    instance.transform.position = m_CurrentPosition + DropPosition;

                instance.GetComponent<Rigidbody>().velocity = DropVelocity;
                instance.active = true;
            }
        }
    }

    public GameObject GetNextAvailiableInstance() {

        if (m_CurrentSide) {
            for (int i = 0; i < maximumInstanceCount; i++)
            {
                if (!m_LeftInstances[i].active)
                {
                    m_CurrentSide = !m_CurrentSide;
                    return m_LeftInstances[i];
                }
            }
        } else {
            for (int i = 0; i < maximumInstanceCount; i++)

                if (!m_RightInstances[i].active)
                {
                    m_CurrentSide = !m_CurrentSide;
                    return m_RightInstances[i];
                }
        }
        return null;
    }

}
