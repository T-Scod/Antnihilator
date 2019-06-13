using UnityEngine;

public class VRRaycaster : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float damageCooldown = 1.0f;

    private float m_damageTimer = 0.0f;
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
                m_damageTimer += Time.deltaTime;
                if (m_damageTimer > damageCooldown)
                {
                    m_lastHitObject.collider.GetComponentInParent<Insect>().TakeDamage();
                    m_damageTimer = 0.0f;
                }
            }
            else
            {
                m_damageTimer = 0.0f;
            }
        }
        else
        {
            ValidHit = false;
            lineRenderer.enabled = false;
        }
    }
}