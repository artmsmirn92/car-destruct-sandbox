using System.Collections;
using System.Collections.Generic;
using mazing.common.Runtime.Ticker;
using mazing.common.Runtime.Utils;
using UnityEngine;
using Zenject;

public class MenuCarsInstancer : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private List<Transform>  spawnTrs;
    [SerializeField] private List<GameObject> carPrefabs;
    [SerializeField] private float            spawnPeriod;

    #endregion

    #region inject

    [Inject] private ICommonTicker CommonTicker { get; }

    #endregion

    #region private

    private float m_SpawnTimer;
    private int   m_SpawnTrId;
    private int   m_CarPrefabId;

    #endregion

    #region engine methods

    private void Start()
    {
        m_SpawnTimer = spawnPeriod;
    }
    
    private void Update()
    {
        m_SpawnTimer += Time.deltaTime;
        if (m_SpawnTimer < spawnPeriod)
            return;
        SpawnCar();
        m_SpawnTimer = 0f;
    }
    
    #endregion

    #region nonpublic methods

    private void SpawnCar()
    {
        float RandAng() => Random.value * 360f;
        var randomRotation = Quaternion.Euler(RandAng(), RandAng(), RandAng());
        var carGo = Instantiate(carPrefabs[m_CarPrefabId], spawnTrs[m_SpawnTrId].position, randomRotation);
        carGo.transform.position = spawnTrs[m_SpawnTrId].position;
        m_CarPrefabId = MathUtils.ClampInverse(m_CarPrefabId + 1, 0, carPrefabs.Count - 1);
        m_SpawnTrId   = MathUtils.ClampInverse(m_SpawnTrId   + 1, 0, spawnTrs.Count   - 1);
        Cor.Run(DestroyCarObjectAfterSomeTimeCoroutine(carGo, spawnPeriod * 3f));
    }

    private IEnumerator DestroyCarObjectAfterSomeTimeCoroutine(Object _CarGo, float _Delay)
    {
        yield return Cor.Delay(_Delay, CommonTicker);
        Destroy(_CarGo);
    }

    #endregion

}
