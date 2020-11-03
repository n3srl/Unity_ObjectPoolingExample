using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class F16vsTRexGameMode : MonoBehaviour
{

    public float PTrex = 33;

    private GameObject[] m_Instances;
    public int maximumInstanceCount = 6;

    public Vector3 DropVelocity;
    public Vector3 DropPosition;
    public Vector3 DropRotation;


    public float Speed;

    public GameObject TRexPrefab;

    // Start is called before the first frame update
    void Start()
    {
        m_Instances = new GameObject[maximumInstanceCount];
        for (int i = 0; i < maximumInstanceCount; i++)
        {
            // place the pile of unused objects somewhere far off the map
            m_Instances[i] = Instantiate(TRexPrefab, DropPosition, Quaternion.identity);
            m_Instances[i].transform.Rotate(DropRotation);
            // disable by default, these objects are not active yet.
            m_Instances[i].active = false;
        }
    }

    public GameObject GetNextAvailiableInstance() {
        for (int i = 0; i < maximumInstanceCount; i++)
        {
            if (!m_Instances[i].active)
            {
                return m_Instances[i];
            }
        }
        return null;
    }

    public bool AreObjectsIntersecting(GameObject object1, GameObject[] objects)
    {

        BoxCollider _collidersObject1 = object1.GetComponentInChildren<BoxCollider>();

        for (int i = 0; i < maximumInstanceCount; i++) {
            if (objects[i].active && object1!= objects[i])
            {
                BoxCollider _collidersObject2 = objects[i].GetComponentInChildren<BoxCollider>();
                if (_collidersObject1.bounds.Intersects(_collidersObject2.bounds))
                    return true;
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        float spawn_p = Random.Range(0,100) * 0.1f;

        if (spawn_p <= PTrex)
        {
            GameObject instance = GetNextAvailiableInstance();

            if (instance != null)
            {

                do
                {
                    float x_range = Random.Range(-5000, 5000) / 100.0f;
                    float z_range = Random.Range(-5000, 5000) / 100.0f;

                    Vector3 offset = new Vector3(x_range, 0, z_range);

                    instance.transform.position = DropPosition + offset;
                } while (AreObjectsIntersecting(instance, m_Instances));

                instance.GetComponent<Rigidbody>().velocity = DropVelocity;
                instance.active = true;

            }
        }
    }
}
