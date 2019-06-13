using UnityEngine;

public class VRRaycaster : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private RaycastHit m_lastHitObject;

    public bool ValidHit { get; private set; } = false;

    public Vector3 GetHitPosition()
    {
        if (ValidHit)
        {
            return m_lastHitObject.point;
        }

        return Vector3.zero;
    }

    public GameObject GetHitObject()
    {
        if (ValidHit)
        {
            return m_lastHitObject.collider.gameObject;
        }

        return null;
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out m_lastHitObject))
        {
            Vector3 offset = (transform.right * 0.5f) - (transform.up * 0.5f);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, m_lastHitObject.point);
            lineRenderer.enabled = true;
            ValidHit = true;
            if (m_lastHitObject.collider.tag == "Enemy")
            {
                Destroy(m_lastHitObject.collider.gameObject);
            }
        }
        else
        {
            ValidHit = false;
            lineRenderer.enabled = false;
        }
    }
}