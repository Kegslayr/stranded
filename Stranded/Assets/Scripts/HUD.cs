using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI instructionsTxt;
    [SerializeField] private TextMeshProUGUI coinsTxt;
    [SerializeField] private TextMeshProUGUI victoryTxt;

    private void Start(){
        instructionsTxt.text = "Search the island for treasure!";
        coinsTxt.text = "Coins: 0";
        victoryTxt.text = "";
    }

    public void UpdateCoins(string message){
        coinsTxt.text = message;
    }

    public void UpdateInstructions(string message){
        instructionsTxt.text = message;
    }

    public void SetVictory(string message){
        victoryTxt.text = message;
    }
}
