using System.Collections;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Ticker;
using mazing.common.Runtime.Utils;
using UnityEngine;

public class TubeBall : MonoBehaviour
{
    private static readonly int Color1 = Shader.PropertyToID("_Color");
    
    [SerializeField] private Collider finishTrigger;

    private Rigidbody    m_Rb;
    private Vector3      m_StartPosition;
    private MeshRenderer m_Renderer;
    private Collider     m_Collider;

    private IViewGameTicker m_Ticker;

    
    public void Init(IViewGameTicker _ViewGameTicker)
    {
        m_Ticker = _ViewGameTicker;
    }

    private void Awake()
    {
        m_Renderer      = GetComponent<MeshRenderer>();
        m_Rb            = GetComponent<Rigidbody>();
        m_Collider      = GetComponent<Collider>();
        m_StartPosition = transform.position;
    }

    private void OnTriggerEnter(Collider _Other)
    {
        if (_Other == finishTrigger)
            Cor.Run(RestartBallCoroutine());
    }
    
    private IEnumerator RestartBallCoroutine()
    {
        var color = m_Renderer.sharedMaterial.GetColor(Color1);
        yield return Cor.Lerp(m_Ticker, 2f, 1f, 0f, _P =>
        {
            m_Renderer.material.SetColor(Color1, color.SetA(_P));
        }, () =>
        {
            m_Rb.position = m_StartPosition;
            m_Rb.velocity = default;
            m_Rb.angularVelocity = default;
            Cor.Run(Cor.WaitNextFrame(
                () => m_Renderer.material.SetColor(Color1, color.SetA(1f))));
        });
    }
}
