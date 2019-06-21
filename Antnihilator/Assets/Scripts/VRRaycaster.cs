using UnityEngine;

/// <summary>
/// Allows the player to interact with the game.
/// </summary>
public class VRRaycaster : MonoBehaviour
{
    /// <summary>
    /// Reference to the object that has the pointer.
    /// </summary>
    [Tooltip("Reference to the object that has the pointer.")]
    public LineRenderer lineRenderer;
    public Transform magnifyingGlass;
    /// <summary>
    /// The duration between the enemy taking damage.
    /// </summary>
    [Tooltip("The duration between the enemy taking damage.")]
    public float damageCooldown = 1.0f;
    /// <summary>
    /// The duration the player is blinded for when the mirror is hit.
    /// </summary>
    [Tooltip("The duration the player is blinded for when the mirror is hit.")]
    public float blindDuration = 1.5f;
    /// <summary>
    /// The speed at which the blind effect appears.
    /// </summary>
    [Tooltip("The speed at which the blind effect appears.")]
    public float blindSpeed = 10.0f;
    /// <summary>
    /// The threshold slightly above the default value to allow for the blind affect to stop.
    /// </summary>
    [Tooltip("The threshold slightly above the default value to allow for the blind affect to stop.")]
    public float blindStopThreshold = 1.05f;
    /// <summary>
    /// The maximum intensity of the light.
    /// </summary>
    [Tooltip("The maximum intensity of the light.")]
    public float maxIntensity = 70.0f;
    /// <summary>
    /// Reference to the light in the scene that will get brighter.
    /// </summary>
    [Tooltip("Reference to the light in the scene that will get brighter.")]
    public Light directionalLight;

    /// <summary>
    /// Used to time the duration between enemies being damaged.
    /// </summary>
    private float m_damageTimer = 0.0f;
    /// <summary>
    /// Used to time the duration of the blind effect.
    /// </summary>
    private float m_bilndTimer;
    /// <summary>
    /// Used to get information about the hit object.
    /// </summary>
    private RaycastHit m_hitObject;
    /// <summary>
    /// Reference the object that was last hit.
    /// </summary>
    private GameObject m_lastHitObject;
    /// <summary>
    /// Reference to the mirror that was hit.
    /// </summary>
    private Light m_mirror;

    /// <summary>
    /// Initialises the timer.
    /// </summary>
    private void Awake()
    {
        m_bilndTimer = blindDuration;
    }

    /// <summary>
    /// Creates a pointer that interacts with the scene.
    /// </summary>
    private void Update()
    {
        // checks if the blind effect should occur
        if (m_bilndTimer < blindDuration ||
            (directionalLight == null && m_mirror != null && m_mirror.intensity > blindStopThreshold) ||
            (directionalLight != null && directionalLight.intensity > blindStopThreshold))
        {
            // increments the blind timer
            m_bilndTimer += Time.deltaTime;

            // checks if the light source is the mirror
            if (directionalLight == null)
            {
                // uses a cos function to vary the intesity over time
                m_mirror.intensity = (maxIntensity * -0.5f) * Mathf.Cos(m_bilndTimer * blindSpeed * Time.deltaTime) + (maxIntensity * 0.5f);
            }
            // uses the directional light as the light source
            else
            {
                // uses a cos function to vary the intesity over time
                directionalLight.intensity = ((maxIntensity - 1) * -0.5f) * Mathf.Cos(m_bilndTimer * blindSpeed) + ((maxIntensity + 1) * 0.5f);
            }
        }
        else
        {
            // resets the timer
            m_bilndTimer = blindDuration;
        }

        // raycasts from the pointer into the scene
        if (Physics.Raycast(transform.position, transform.forward, out m_hitObject))
        {
            // sets the positions of the ends of the pointer to the controller and the hit object
            lineRenderer.SetPosition(0, magnifyingGlass.position);
            lineRenderer.SetPosition(1, m_hitObject.point);
            lineRenderer.enabled = true;

            // checks if a mirror was hit
            if (m_hitObject.collider.tag == "Mirror")
            {
                // stops audio for enemies
                StopEnemy();
                // checks if the blind effect has not started
                if (m_bilndTimer == blindDuration)
                {
                    // stores the light component of the object
                    m_mirror = m_hitObject.collider.transform.parent.GetComponentInChildren<Light>();
                    m_bilndTimer = 0.0f;
                }
            }
            // checks if a enemy was hit
            else if (m_hitObject.collider.tag == "Enemy")
            {
                // checks if the last hit object was not the same enemy
                if (m_lastHitObject != null && m_lastHitObject.tag == "Enemy" && m_lastHitObject != m_hitObject.collider.gameObject)
                {
                    // stops the damage audio on the other enemy
                    m_lastHitObject.GetComponentInParent<Insect>().StopAudio();
                }
                // increments the amont of time this enemy has been targeted
                m_damageTimer += Time.deltaTime;
                // starts the damage audio
                m_hitObject.collider.GetComponentInParent<Insect>().StartAudio();
                // if the damage timer exceeds the cooldown time then damage the enemy
                if (m_damageTimer > damageCooldown)
                {
                    m_hitObject.collider.GetComponentInParent<Insect>().TakeDamage();
                    m_damageTimer = 0.0f;
                }
            }
            // stops any enemy audio if no enemy was hit
            else
            {
                StopEnemy();
                m_damageTimer = 0.0f;
            }

            // stores the hit object as the last hit object
            m_lastHitObject = m_hitObject.collider.gameObject;
        }
        // does not draw a line
        else
        {
            StopEnemy();
            lineRenderer.enabled = false;
            m_damageTimer = 0.0f;
            m_lastHitObject = null;
        }
    }

    /// <summary>
    /// Checks if the last hit enemy was an enemy.
    /// </summary>
    private void StopEnemy()
    {
        if (m_lastHitObject != null && m_lastHitObject.tag == "Enemy")
        {
            // stops the enemy's audio
            m_lastHitObject.GetComponentInParent<Insect>().StopAudio();
        }
    }
}