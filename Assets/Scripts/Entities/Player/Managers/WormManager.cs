using System.Collections.Generic;
using UnityEngine;

public class WormManager : MonoBehaviour
{
    [SerializeField] WormEntity wormPrefab;
    [SerializeField] PlayerController player;


    Queue<WormEntity> wormPool = new();

    private void Start()
    {
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
