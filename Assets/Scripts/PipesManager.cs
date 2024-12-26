using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PipesManager : MonoBehaviour
{
    private float m_timer = 0.0f;

    public float SpawnTime = 1.5f;
    public float SpawnHightRange = 0.45f;
    public GameObject PipePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer > SpawnTime)
        {
            SpawnPipe();

            m_timer -= SpawnTime;
        }

        m_timer += Time.deltaTime;
    }

    void SpawnPipe()
    { 
        // random the PosY of spawn position.
        Vector3 spawnPos = this.transform.position + new Vector3(0 , Random.Range(-SpawnHightRange, SpawnHightRange), 0);
        GameObject newPipe = GameObject.Instantiate(PipePrefab, spawnPos, Quaternion.identity);

        // delete pipe after 5 secs.
        GameObject.Destroy(newPipe, 5);
    }
}
