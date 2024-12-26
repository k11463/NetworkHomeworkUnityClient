using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GroundBehavior : MonoBehaviour
{
    private float m_maxWidth = 10f;
    private Vector2 m_startSize;
    private Vector2 m_nowSize;
    private SpriteRenderer m_spriteRenderer;

    public float Velocity = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_startSize = new Vector2(m_spriteRenderer.size.x, m_spriteRenderer.size.y);
        m_nowSize = new Vector2(m_startSize.x, m_startSize.y);
    }

    // Update is called once per frame
    void Update()
    {
        m_nowSize.x += Velocity * Time.deltaTime;
        m_spriteRenderer.size = m_nowSize;

        if (m_spriteRenderer.size.x > m_maxWidth)
        {
            m_spriteRenderer.size = m_startSize;
            m_nowSize.x = m_startSize.x;
        }
    }
}
