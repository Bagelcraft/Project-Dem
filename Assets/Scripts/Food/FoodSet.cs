using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FoodSet : MonoBehaviour
{
    public List<Sprite> foodSprites; // Store the food sprites here.
    public List<Sprite> playerInputSequence; // Store the player's input sequence here.

    public FoodSequence foodSequenceManager;

    private int inputIndex = 0; // Keep track of the index of the player's input.

    private void Start()
    {
        playerInputSequence = new List<Sprite>();
        foodSequenceManager = FindObjectOfType<FoodSequence>();
    }

    public void Food1Clicked()
    {
        HandleFoodInput(foodSprites[0]); // Assuming foodSprites[0] corresponds to the first food item.
        AudioManager.instance.PlaySound("Red Note");
    }

    public void Food2Clicked()
    {
        HandleFoodInput(foodSprites[1]); // Assuming foodSprites[1] corresponds to the second food item.
        AudioManager.instance.PlaySound("Blue Note");

    }
    public void Food3Clicked()
    {
        HandleFoodInput(foodSprites[2]); // Assuming foodSprites[1] corresponds to the second food item.
        AudioManager.instance.PlaySound("Green Note");

    }
    public void Food4Clicked()
    {
        HandleFoodInput(foodSprites[3]); // Assuming foodSprites[1] corresponds to the second food item.
        AudioManager.instance.PlaySound("Yellow Note");
    }
    public void Food5Clicked()
    {
        HandleFoodInput(foodSprites[4]); // Assuming foodSprites[1] corresponds to the second food item.
        AudioManager.instance.PlaySound("White Note");
    }
    public void Food6Clicked()
    {
        HandleFoodInput(foodSprites[5]); // Assuming foodSprites[1] corresponds to the second food item.
        AudioManager.instance.PlaySound("Black Note");
    }
    // Repeat similar methods for Food3, Food4, Food5, and Food6.

    private void HandleFoodInput(Sprite foodSprite)
    {
        // Add the clicked food sprite to the player's input sequence.
        playerInputSequence.Add(foodSprite);
        inputIndex++;

        Debug.Log("Button pressed with food: " + foodSprite.name);

        // Check the player's input when they have entered as many foods as there are in the sequence.
        if (inputIndex >= foodSequenceManager.foodsInSequence)
        {
            CheckPlayerInput();
        }
    }

    public void CheckPlayerInput()
    {
        // Implement your logic to check the player's input sequence.
        // You can compare playerInputSequence with the correct food sequence here.
        // If the sequence is correct, you can proceed with the game logic.

        foodSequenceManager.HandleCorrectInput(playerInputSequence);
    }

    public void ClearPlayerInputSequence()
    {
        playerInputSequence.Clear();
        inputIndex = 0;
    }


}

