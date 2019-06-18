using UnityEngine;
using UnityEngine.UI;

public class RotateCanvas : MonoBehaviour
{
    public float rotateSpeed = 10.0f;
    public float rotateInAngle = 45.0f;
    public float rotateOutAngle = 91.0f;
    public float stopRotatingThreshold = 0.1f;

    private bool m_becomingDisabled = false;
    private Button[] buttons;
    private RectTransform rectTransform;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        rectTransform = GetComponent<RectTransform>();
        m_becomingDisabled = false;
    }

    private void Update()
    {
        if (m_becomingDisabled)
        {
            RotateOut();
        }
        else
        {
            RotateIn();
        }
    }

    public void RotateIn()
    {
        Quaternion rotation = Quaternion.identity;
        rotation.x = rotateInAngle;
        rectTransform.rotation = Quaternion.RotateTowards(rectTransform.rotation, rotation, rotateSpeed * Time.deltaTime);
    }

    public void RotateOut()
    {
        Quaternion rotation = Quaternion.Euler(rotateOutAngle, 0.0f, 0.0f);
        rectTransform.rotation = Quaternion.RotateTowards(rectTransform.rotation, rotation, rotateSpeed * Time.deltaTime);
        if (rectTransform.rotation.eulerAngles.x < rotation.eulerAngles.x + stopRotatingThreshold &&
            rectTransform.rotation.eulerAngles.x > rotation.eulerAngles.x - stopRotatingThreshold)
        {

        }
    }
}