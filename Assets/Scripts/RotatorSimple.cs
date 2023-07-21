using UnityEngine;

public class RotatorSimple : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3   torqueVec;
    [SerializeField] private float     torque;
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private bool      usePhysics = true;
    
    #endregion

    private void FixedUpdate()
    {
        if (usePhysics)
            rb.AddTorque(torqueVec * torque, forceMode);
        else
            transform.Rotate(torqueVec, torque);
    }
}
