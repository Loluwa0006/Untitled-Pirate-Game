using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WormManager : MonoBehaviour
{
    [SerializeField] WormEntity wormPrefab;
    [SerializeField] PlayerController player;
    [SerializeField] TMP_Text wormDisplay;

    float wormsRemaining;
    public float WormsRemaining
    {
        get => wormsRemaining ;
        set
        {
            wormsRemaining =
            Mathf.Clamp(value, 0, player.PlayerStats.MaxWorms);
            if (wormDisplay != null) wormDisplay.text = "Worms: " + wormsRemaining;
        }
    }
    Queue<WormEntity> wormPool = new();

    private void Start()
    {
        wormsRemaining = player.PlayerStats.MaxWorms;

        for (int i = 0; i < player.PlayerStats.MaxWorms; i++)
        {
            WormEntity newWorm = Instantiate(wormPrefab);
            newWorm.Deactivate();
            wormPool.Enqueue(newWorm);
        }
    }

    public WormEntity GetNewWorm()
    {
        var newWorm = wormPool.Dequeue();
        wormPool.Enqueue(newWorm);
        return newWorm;
    }

}
