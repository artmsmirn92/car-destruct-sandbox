using System.Collections.Generic;
using UnityEngine;

public class MenuCarsInstancer : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private List<Transform>  spawnTrs;
    [SerializeField] private List<GameObject> carPrefabs;
    [SerializeField] private float            spawnPeriod;

    #endregion

    #region private

    private float m_SpawnTimer;
    private int   m_SpawnTrId;
    private int   m_CarPrefabId;

    #endregion

    #region engine methods

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
        var carGo = Instantiate(carPrefabs[m_CarPrefabId++]);
        carGo.transform.position = spawnTrs[m_SpawnTrId++].position;
    }

    #endregion

}
