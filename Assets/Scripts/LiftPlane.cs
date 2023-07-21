using System.Collections;
using DG.Tweening;
using mazing.common.Runtime;
using mazing.common.Runtime.Utils;
using UnityEngine;

public class LiftPlane : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float     duration = 1f;
    [SerializeField] private float     stayOnFloorPause = 2f;

    private void Start()
    {
        transform.DOMove(target.position, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .OnStepComplete(AnimationComplete);
    }

    private void AnimationComplete()
    {
        StartCoroutine(PauseOnFloorCoroutine());
        Dbg.Log("Пауза на этаже");
    }

    private IEnumerator PauseOnFloorCoroutine()
    {
        transform.DOPause();
        yield return new WaitForSeconds(stayOnFloorPause);
        transform.DOPlay();
    }
}
