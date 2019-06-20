using UnityEngine;

public class VRRaycaster : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float damageCooldown = 1.0f;
    public float blindDuration = 1.5f;
    public float blindSpeed = 10.0f;
    public float blindStopThreshold = 1.05f;
    public float maxIntensity = 70.0f;
    public Light directionalLight;

    private float m_damageTimer = 0.0f;
    private float m_bilndTimer;
    private RaycastHit m_lastHitObject;
    private Light m_mirror;

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

    private void Awake()
    {
        m_bilndTimer = blindDuration;
    }

    private void Update()
    {
        if (m_bilndTimer < blindDuration ||
            (directionalLight == null && m_mirror != null && m_mirror.intensity > blindStopThreshold) ||
            (directionalLight != null && directionalLight.intensity > blindStopThreshold))
        {
            m_bilndTimer += Time.deltaTime;

            if (directionalLight == null)
            {
                m_mirror.intensity = (maxIntensity * -0.5f) * Mathf.Cos(m_bilndTimer * blindSpeed * Time.deltaTime) + (maxIntensity * 0.5f);
            }
            else
            {
                directionalLight.intensity = ((maxIntensity - 1) * -0.5f) * Mathf.Cos(m_bilndTimer * blindSpeed) + ((maxIntensity + 1) * 0.5f);
            }
        }
        else
        {
            m_bilndTimer = blindDuration;
        }

        if (Physics.Raycast(transform.position, transform.forward, out m_lastHitObject))
        {
            Vector3 offset = (transform.right * 0.5f) - (transform.up * 0.5f);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, m_lastHitObject.point);
            lineRenderer.enabled = true;
            ValidHit = true;
            if (m_lastHitObject.collider.tag == "Mirror")
            {
                if (m_bilndTimer == blindDuration)
                {
                    m_mirror = m_lastHitObject.collider.transform.parent.GetComponentInChildren<Light>();
                    m_bilndTimer = 0.0f;
                }
            }
            else if (m_lastHitObject.collider.tag == "Enemy")
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
            m_damageTimer = 0.0f;
        }
    }
}