using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;

    public bool leftButtonPressed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assign Values to Static Values
        Player.player = player; 
        Player.leftButtonPressed = leftButtonPressed;
    }

    public void setLeftButtonPressed(bool leftButtonPressed)
    {
        Debug.Log("Enabled");
        Player.leftButtonPressed = leftButtonPressed;
        Invoke("disableLeftButtonPressed", 0.05f);
    }

    void disableLeftButtonPressed()
    {
        Player.leftButtonPressed = false;
    }
}
