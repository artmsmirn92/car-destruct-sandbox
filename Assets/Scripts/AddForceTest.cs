using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AddForceTest : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float     force;
    [SerializeField] private Transform target;

    public void Throw()
    {
        var dir = target.transform.position - rb.transform.position;
        dir.Normalize();
        rb.AddForce(force * dir);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AddForceTest))]
public class AddForceTestEditor : Editor
{
    private AddForceTest m_O;
    
    private void OnEnable()
    {
        m_O = (AddForceTest) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Throw"))
            m_O.Throw();
    }
}

#endif
