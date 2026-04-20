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


    private void Start()
    {
        squashbucklerMeter.maxValue = maxCharge;
        anarchyManager.anarchyGained.AddListener(OnAnarchyGained);
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
