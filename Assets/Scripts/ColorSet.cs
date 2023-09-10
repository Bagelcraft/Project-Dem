using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ColorSet : MonoBehaviour
{
    public List<Color> playerInputSequence; // Store the player's input sequence here.

    public ColorSequenceManager colorSequenceManager;

    private int inputIndex = 0; // Keep track of the index of the player's input.

    private void Start()
    {
        playerInputSequence = new List<Color>();
        colorSequenceManager = FindObjectOfType<ColorSequenceManager>();
    }

    public void RedClicked()
    {
        HandleColorInput(Color.red);
        // Play a sound by name
        AudioManager.instance.PlaySound("Red Note");

    }

    public void BlueClicked()
    {
        HandleColorInput(Color.blue);
        AudioManager.instance.PlaySound("Blue Note");
    }

    public void GreenClicked()
    {
        HandleColorInput(Color.green);
        AudioManager.instance.PlaySound("Green Note");
    }

    public void YellowClicked()
    {
        HandleColorInput(Color.yellow);
        AudioManager.instance.PlaySound("Yellow Note");
    }

    public void WhiteClicked()
    {
        HandleColorInput(Color.white);
        AudioManager.instance.PlaySound("White Note");
    }

    public void BlackClicked()
    {
        HandleColorInput(Color.black);
        AudioManager.instance.PlaySound("Black Note");
    }

    private void HandleColorInput(Color color)
    {
        // Add the clicked color to the player's input sequence.
        playerInputSequence.Add(color);
        inputIndex++;

        Debug.Log("Button pressed with color: " + color);

        // Check the player's input when they have entered as many colors as there are in the sequence.
        if (inputIndex >= colorSequenceManager.colorsInSequence)
        {
            CheckPlayerInput();
        }
    }

    public void CheckPlayerInput()
    {
        // Implement your logic to check the player's input sequence.
        // You can compare playerInputSequence with the correct color sequence here.
        // If the sequence is correct, you can proceed with the game logic.

        colorSequenceManager.HandleCorrectInput(playerInputSequence);
    }

    public void ClearList()
    {
        playerInputSequence.Clear();
        inputIndex = 0; // Reset the input index when clearing the list.
    }
}
