using System.Collections.Generic;
using System.Linq;
using mazing.common.Runtime.Extensions;
using mazing.common.Runtime.Ticker;
using UnityEngine;
using Zenject;

public class TubeWithBalls : MonoBehaviour
{
    [Inject] private IViewGameTicker ViewGameTicker { get; }

    private List<TubeBall> m_TubeBalls;

    private void Start()
    {
        m_TubeBalls = transform
            .GetChildren(true)
            .Select(_C => _C.GetComponent<TubeBall>())
            .Where(_Tb => _Tb.IsNotNull())
            .ToList();
        foreach (var tubeBall in m_TubeBalls)
        {
            // var clonedMaterial = Instantiate(tubeBall.Renderer.sharedMaterial);
            tubeBall.Init(ViewGameTicker);
        }
    }
}
