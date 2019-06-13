using UnityEngine;
using PathCreation;

public class Insect : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5.0f;
    public int health = 3;
    public int attackAmount = 1;

    private float m_distanceTravelled;

    private void Start()
    {
        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    private void Update()
    {
        Debug.Log(health);
        if (pathCreator != null)
        {
            m_distanceTravelled += speed * Time.deltaTime;
            float t = m_distanceTravelled / pathCreator.path.length;
            t = Mathf.Clamp01(t);
            if (t != 1)
            {
                transform.position = pathCreator.path.GetPointAtDistance(m_distanceTravelled, EndOfPathInstruction.Stop);
                transform.rotation = pathCreator.path.GetRotationAtDistance(m_distanceTravelled, EndOfPathInstruction.Stop);
            }
            else
            {
                UnityEngine.UI.Text healthText = FindObjectOfType<UnityEngine.UI.Text>();
                int playerHealth = int.Parse(healthText.text) - attackAmount;
                healthText.text = playerHealth.ToString();
                Destroy(gameObject);
            }
        }
    }

    private void OnPathChanged()
    {
        m_distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    public void TakeDamage()
    {
        health--;
        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}