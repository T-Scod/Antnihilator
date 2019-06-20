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
    private RaycastHit m_hitObject;
    private GameObject m_lastHitObject;
    private Light m_mirror;

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

        if (Physics.Raycast(transform.position, transform.forward, out m_hitObject))
        {
            Vector3 offset = (transform.right * 0.5f) - (transform.up * 0.5f);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, m_hitObject.point);
            lineRenderer.enabled = true;
            if (m_hitObject.collider.tag == "Mirror")
            {
                StopEnemy();
                if (m_bilndTimer == blindDuration)
                {
                    m_mirror = m_hitObject.collider.transform.parent.GetComponentInChildren<Light>();
                    m_bilndTimer = 0.0f;
                }
            }
            else if (m_hitObject.collider.tag == "Enemy")
            {
                if (m_lastHitObject != null && m_lastHitObject.tag == "Enemy" && m_lastHitObject != m_hitObject.collider.gameObject)
                {
                    m_lastHitObject.GetComponentInParent<Insect>().StopAudio();
                }
                m_damageTimer += Time.deltaTime;
                m_hitObject.collider.GetComponentInParent<Insect>().StartAudio();
                if (m_damageTimer > damageCooldown)
                {
                    m_hitObject.collider.GetComponentInParent<Insect>().TakeDamage();
                    m_damageTimer = 0.0f;
                }
            }
            else
            {
                StopEnemy();
                m_damageTimer = 0.0f;
            }

            m_lastHitObject = m_hitObject.collider.gameObject;
        }
        else
        {
            StopEnemy();
            lineRenderer.enabled = false;
            m_damageTimer = 0.0f;
            m_lastHitObject = null;
        }
    }

    private void StopEnemy()
    {
        if (m_lastHitObject != null && m_lastHitObject.tag == "Enemy")
        {
            m_lastHitObject.GetComponentInParent<Insect>().StopAudio();
        }
    }
}