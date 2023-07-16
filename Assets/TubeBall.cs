using UnityEngine;

public class TubeBall : MonoBehaviour
{
    [SerializeField] private Collider finishTrigger;

    private Rigidbody m_Rb;
    private Vector3   m_StartPosition;

    private void Start()
    {
        m_Rb = GetComponent<Rigidbody>();
        m_StartPosition = m_Rb.position;
    }

    private void OnTriggerEnter(Collider _Other)
    {
        if (_Other == finishTrigger)
            RestartBall();
    }

    private void RestartBall()
    {
        m_Rb.velocity = default;
        m_Rb.position = m_StartPosition;
    }
}
