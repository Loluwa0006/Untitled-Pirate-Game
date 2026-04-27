using UnityEngine;
using UnityEngine.UI;

public class SquashbucklerManager : MonoBehaviour
{

    [SerializeField] int maxCharge = 10;
    [SerializeField] AnarchyManager anarchyManager;
    [SerializeField] Slider squashbucklerMeter;
    int squashbucklerCharge;
    public int SquashbucklerCharge { get => squashbucklerCharge;
        set
        {

            squashbucklerCharge = Mathf.Clamp(value, 0, maxCharge);
            UpdateSquashbucklerDisplays();
        }
    }

    public int MaxCharge { get => maxCharge; }


    private void Start()
    {
        squashbucklerMeter.maxValue = maxCharge;
        anarchyManager.anarchyGainedThroughScaledMethod.AddListener((method, charges) => OnAnarchyGained(charges));
        anarchyManager.anarchyGainedThroughUnscaledMethod.AddListener((method, charges) => OnAnarchyGained(charges));
        UpdateSquashbucklerDisplays();
    }

    void OnAnarchyGained(int charges)
    {
        squashbucklerCharge += charges;
        UpdateSquashbucklerDisplays();
    }

    void UpdateSquashbucklerDisplays()
    {
        squashbucklerMeter.value = squashbucklerCharge;
    }
}
