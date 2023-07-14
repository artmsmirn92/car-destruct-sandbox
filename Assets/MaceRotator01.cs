using UnityEngine;

public class MaceRotator01 : MonoBehaviour
{
    #region serialized fields

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Vector3   torqueVec;
    [SerializeField] private float     torque;
    [SerializeField] private bool      usePhysics = true;
    
    #endregion

    private void FixedUpdate()
    {
        if (usePhysics)
            rb.AddTorque(torqueVec * torque, ForceMode.Force);
        else
        {
             transform.Rotate(torqueVec, torque);
        }
    }
}
