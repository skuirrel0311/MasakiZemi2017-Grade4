using UnityEngine;

public class BaseGauge : MonoBehaviour
{
    [SerializeField]
    protected int maxValue = 10;
    public int value { get; protected set; }

    public virtual void Init()
    {
        value = maxValue;
    }

    public virtual void SetValue(int point)
    {
        value = point;
        value = Mathf.Clamp(value, 0, maxValue);

        UpdateGaugeImage();
    }

    protected virtual void UpdateGaugeImage() { }
}
