using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    Hero Player;

    GameObject Interface;

    TextMeshProUGUI HP;
    TextMeshProUGUI Stamina;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();

        Interface = GameObject.FindGameObjectWithTag("UI");

        HP = Interface.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Stamina = Interface.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }
    private void FixedUpdate()
    {
        HP.text = "" + Player.GetCurrentHp;
        Stamina.text = "" + Player.GetCurrentStamina;
    }
}
