using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image fillImage;
    [SerializeField] protected Color lowColor = Color.red;
    [SerializeField] protected Color highColor = Color.green;
    [SerializeField] protected Vector3 offset = new Vector3(0, 2f, 0);

    protected virtual void Awake()
    {
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }
        if (fillImage == null && slider != null)
        {
            fillImage = slider.fillRect.GetComponentInChildren<Image>();
        }
    }

    protected virtual void Update()
    {
        if (slider == null) return;
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public virtual void SetHealth(float health, float maxHealth)
    {
        if (slider == null || fillImage == null) return;

        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;
        fillImage.color = Color.Lerp(lowColor, highColor, slider.normalizedValue);
    }
}