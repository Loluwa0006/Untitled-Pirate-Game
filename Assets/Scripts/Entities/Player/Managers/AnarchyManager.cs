using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AnarchyManager : MonoBehaviour
{
    const int MAX_ANARCHY = 99;
    public enum AnarchyGenerationMethod
    {
        Swing,
        Dash,
        Parry,
        RailParry,
        Dragonstep,
        WormThrow,
    }

   [SerializeField] PlayerController player;
   [SerializeField] TMP_Text anarchyDisplay;
   [SerializeField] TMP_Text anarchyProgressDisplay;

   [SerializeField] int numberOfOptionsToUseToReduceScaling = 2;
   [SerializeField, Range(0, 1)] float scalingGenerationReductionAmount = 0.1f;
   [SerializeField] float generationPerOption = 30.0f;

    [SerializeField] int baseDecayRate = 150;
    [SerializeField] int minDecayRate = 30;

    public int DecayRate { get => Mathf.RoundToInt(Mathf.Lerp(baseDecayRate, minDecayRate, CurrentAnarchy / MAX_ANARCHY)); }

    int decayTracker = 0;
    int currentAnarchy;
   public int CurrentAnarchy { set => currentAnarchy = Mathf.RoundToInt(Mathf.Clamp(value, 0, MAX_ANARCHY)); get => currentAnarchy; }

    float progressToAnarchy;
    public float ProgressToAnarchy { set => progressToAnarchy = value; get => progressToAnarchy; }



    Dictionary<AnarchyGenerationMethod, float> generationScaling = new();

    private void Start()
    {
        generationScaling[AnarchyGenerationMethod.Swing] = 0;
        generationScaling[AnarchyGenerationMethod.Dash] = 0;
        generationScaling[AnarchyGenerationMethod.Parry] = 0;
        generationScaling[AnarchyGenerationMethod.RailParry] = 0;
        generationScaling[AnarchyGenerationMethod.Dragonstep] = 0;
        generationScaling[AnarchyGenerationMethod.WormThrow] = 0;

        UpdateAnarchyDisplays();
    }

    public void GenerateAnarchy(AnarchyGenerationMethod method)
    {
        foreach (var kvp in generationScaling.ToList())
        {
            if (kvp.Key == method) continue;
            generationScaling[kvp.Key]--;
        }
        progressToAnarchy += generationPerOption * (1 - generationScaling[method]);
        generationScaling[method] = scalingGenerationReductionAmount * numberOfOptionsToUseToReduceScaling;

        var increasesToAnarchy = Mathf.FloorToInt(progressToAnarchy / 100);
        currentAnarchy += increasesToAnarchy;
        player.WormManager.WormsRemaining += increasesToAnarchy;
        ProgressToAnarchy -= increasesToAnarchy * 100;
        decayTracker = DecayRate;
        UpdateAnarchyDisplays();
    }

    void UpdateAnarchyDisplays()
    {
       if (anarchyDisplay != null) anarchyDisplay.text = "Anarchy: " + currentAnarchy.ToString();
       if (anarchyProgressDisplay != null) anarchyProgressDisplay.text = "Anarchy Progress: " + progressToAnarchy.ToString();
    }

    void ResetAnarchy()
    {
        CurrentAnarchy = 0;
        decayTracker = DecayRate;
        foreach (var kvp in generationScaling.ToList())
        {
            generationScaling[kvp.Key] = 0;
        }
        UpdateAnarchyDisplays();
    }

    private void FixedUpdate()
    {
        if (decayTracker > 0)
        {
            decayTracker--;
            if (decayTracker == 0)
            {
                ResetAnarchy();
            }
        }
    }


}
