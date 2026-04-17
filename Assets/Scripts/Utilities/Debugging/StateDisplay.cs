using TMPro;
using UnityEngine;

public class StateDisplay : MonoBehaviour
{
    [SerializeField] EntityStateMachine stateMachine;
    [SerializeField] TMP_Text stateDisplay;
    private void Update()
    {
        stateDisplay.text = "State: " + stateMachine.GetCurrentState().name;   
    }
}
