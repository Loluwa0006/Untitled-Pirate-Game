using UnityEngine;
using UnityEngine.UI;

public class SquashbucklerManager : MonoBehaviour
{

    [SerializeField] int maxCharge = 10;
    [SerializeField] AnarchyManager anarchyManager;
    [SerializeField] Slider squashbucklerMeter;
    int squashbucklerCharge;
    public int SquashbucklerCharge { get => squashbucklerCharge; private set => squashbucklerCharge = Mathf.Clamp(value, 0, maxCharge) ; }

    private void Start()
    {
        squashbucklerMeter.maxValue = maxCharge;
        anarchyManager.anarchyGained.AddListener(OnAnarchyGained);
        UpdateAnarchyDisplays();
    }

    void OnAnarchyGained(int charges)
    {
        squashbucklerCharge += charges;
        UpdateAnarchyDisplays();
    }

    void UpdateAnarchyDisplays()
    {
        squashbucklerMeter.value = squashbucklerCharge;
    }
}
