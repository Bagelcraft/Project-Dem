using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FoodSequence : MonoBehaviour
{
    public GameObject[] foodPanels; // Array of food panels.
    public TextMeshProUGUI messageText; // Reference to a TextMeshPro element to display messages.
    public float displayTime = 2f; // Time in seconds to display each food.
    public GameObject Buttons;
    public GameObject Correct;
    public GameObject Wrong;

    public List<Sprite> foodSequence; // List to store the random food sequence.
    private int sequenceIndex = 0; // Index to keep track of the current food in the sequence.
    public int level = 1; // Current game level.
    public int foodsInSequence = 1; // Number of foods in the current sequence. Make it public.
    private bool playerInputEnabled = false; // Controls whether player input is enabled.
    public int playerLives = 3; // Initialize with 3 lives.

    public GameObject StartButton;
    public GameObject RestartButton;
    public GameObject MainMenu;

    private void Start()
    {
        // Initialize the food sequence by extracting sprites from food panels.
        foodSequence = new List<Sprite>();

        foreach (GameObject foodPanel in foodPanels)
        {
            Image image = foodPanel.GetComponent<Image>();
            if (image != null)
            {
                foodSequence.Add(image.sprite);
            }
            else
            {
                Debug.LogError("Missing Image component on food panel: " + foodPanel.name);
            }
        }

        //// Start the game with level 1.
        //StartLevel(1);

        Buttons.SetActive(false);
        Correct.SetActive(false);
        Wrong.SetActive(false);
        StartButton.SetActive(true);
        RestartButton.SetActive(false);
        MainMenu.SetActive(false);
    }


    private void StartLevel(int level)
    {
        this.level = level;
        playerInputEnabled = false;
        StartButton.SetActive(false);
        RestartButton.SetActive(false);
        MainMenu.SetActive(false);

        if (level <= 5)
        {
            foodsInSequence = level;
            displayTime = 3f;
        }
        else if (level <= 10)
        {
            foodsInSequence = level - 5; // Reset foodsInSequence and start from 1 to 5 again.
            displayTime = 2.5f;
        }
        else if (level <= 15)
        {
            foodsInSequence = level - 10; // Reset foodsInSequence and start from 1 to 5 again.
            displayTime = 2.0f;
        }
        else if (level <= 20)
        {
            foodsInSequence = level - 15; // Reset foodsInSequence and start from 1 to 5 again.
            displayTime = 1.5f;
        }
        else if (level <= 25)
        {
            foodsInSequence = level - 20; // Reset foodsInSequence and start from 1 to 5 again.
            displayTime = 1.0f;
        }
        else
        {
            // Beyond level 25, add one more item to the sequence and keep a display time of 1.0.
            foodsInSequence = level - 25 + 5; // Start from 6 items and add one more for each level.
            displayTime = 1.0f;
        }

        sequenceIndex = 0;

        // Clear the food sequence for the current level.
        foodSequence.Clear();

        // Generate a new random food sequence.
        for (int i = 0; i < foodsInSequence; i++)
        {
            int randomIndex = Random.Range(0, foodPanels.Length);
            Sprite randomFoodSprite = foodPanels[randomIndex].GetComponent<Image>().sprite; // Get the food sprite from the panel.
            foodSequence.Add(randomFoodSprite);

            // Call FoodChecker to check and play the sound for the current food.
            FoodChecker(randomFoodSprite);
        }

        // Start displaying the food sequence for the current level.
        StartCoroutine(DisplayFoodSequence());
    }



    // Coroutine to display the food sequence.
    private IEnumerator DisplayFoodSequence()
    {
        for (int i = 0; i < foodsInSequence; i++)
        {
            Sprite currentFood = foodSequence[i];

            // Set the selected food panel to active (true) and others to inactive (false).
            for (int j = 0; j < foodPanels.Length; j++)
            {
                foodPanels[j].SetActive(foodPanels[j].GetComponent<Image>().sprite == currentFood);
            }

            // Call FoodChecker to check and play the sound for the current food.
            FoodChecker(currentFood);

            // Display the food for a set amount of time.
            yield return new WaitForSeconds(displayTime);

            // Hide all food panels.
            foreach (GameObject panel in foodPanels)
            {
                panel.SetActive(false);
            }

            // Move to the next food in the sequence.
            sequenceIndex++;
        }

        playerInputEnabled = true; // Enable player input.
        Buttons.SetActive(true);
    }

    // Implement FoodChecker, HandleCorrectInput, and other methods similarly to how they were done with colors but for food images.
    private void FoodChecker(Sprite currentFood)
    {
        // Implement your logic to check the current food against the correct food.
        // You can compare the currentFood sprite with the correct food sprite.
        // Play a sound or take other actions based on the result.

        if (currentFood.name == "Bah Kut Teh")
        {
            AudioManager.instance.PlaySound("Red Note");
        }
        if (currentFood.name == "Chicken Rice")
        {
            AudioManager.instance.PlaySound("Blue Note");
        }
        if (currentFood.name == "Chilli Crab")
        {
            AudioManager.instance.PlaySound("Green Note");
        }
        if (currentFood.name == "Laksa")
        {
            AudioManager.instance.PlaySound("Yellow Note");
        }
        if (currentFood.name == "Rojak")
        {
            AudioManager.instance.PlaySound("White Note");
        }
        if (currentFood.name == "Roti Prata")
        {
            AudioManager.instance.PlaySound("Black Note");
        }
    }

    // Handle the correct input from the player.
    // Handle the correct input from the player.
    public void HandleCorrectInput(List<Sprite> playerInputSequence)
    {
        Debug.Log("Correct Sequence: " + string.Join(", ", foodSequence));
        Debug.Log("Player Input Sequence: " + string.Join(", ", playerInputSequence));

        // Ensure both sequences have the same number of foods.
        if (playerInputSequence.Count != foodSequence.Count)
        {
            StartCoroutine(IncorrectInputDelay());
            return;
        }

        // Compare the food sprites in playerInputSequence with foodSequence.
        for (int i = 0; i < foodSequence.Count; i++)
        {
            Sprite correctFood = foodSequence[i];
            Sprite playerFood = playerInputSequence[i];

            // Compare playerFood with correctFood (customize this logic based on your needs).
            if (playerFood != correctFood)
            {
                StartCoroutine(IncorrectInputDelay());
                return;
            }
        }

        Debug.Log("Correct!");
        AudioManager.instance.PlaySound("Correct Answer");
        // If the player has completed the current level's sequence.
        messageText.text = "Correct! Level " + level + " Complete!";
        playerInputEnabled = false; // Disable input while transitioning to the next level.

        // Clear the player input list here, after completing the level and before starting the next one.
        FindObjectOfType<FoodSet>().ClearPlayerInputSequence();

        // Deactivate answer buttons.
        Buttons.SetActive(false);
        Correct.SetActive(true);
        // Start the next level after a delay (customize this delay).
        StartCoroutine(StartNextLevel());
    }

    private IEnumerator IncorrectInputDelay()
    {

        // Handle actions after the delay here.
        Debug.Log("Wrong!");

        // Deduct a life.
        playerLives--;

        // Check if the player has run out of lives.
        if (playerLives <= 0)
        {
            // Game over logic here.
            // You can restart the game, show a game over screen, etc.
            Debug.Log("Game Over");
            AudioManager.instance.PlaySound("Game Over");
            Buttons.SetActive(false);
            RestartButton.SetActive(true);
            MainMenu.SetActive(true);
            messageText.text = "Game Over! Score :" + level;
        }
        else
        {
            // Restart the current level.
            Debug.Log("Try Again!");
            AudioManager.instance.PlaySound("Wrong Answer");
            // Clear the player input list here, after handling the incorrect input.
            FindObjectOfType<FoodSet>().ClearPlayerInputSequence();
            Buttons.SetActive(false);
            Wrong.SetActive(true);
            yield return new WaitForSeconds(2f); // Delay for 2 seconds.
            Wrong.SetActive(false);
            RestartCurrentLevel();
        }
    }

    private IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay as needed.
        messageText.text = " ";
        Correct.SetActive(false);
        StartLevel(level + 1); // Start the next level.
    }


    private void RestartCurrentLevel()
    {
        StartLevel(level);
    }

    public void StartGame()
    {
        // Add logic to initialize or start the game from the beginning.
        // For example, reset the level, lives, and start displaying the food sequence.
        StartLevel(1); // Start from level 1.
        playerLives = 3;
        messageText.text = " ";
    }

    public void LoadMainMenu()
    {
        // Load the game scene (replace "GameSceneName" with the actual scene name)
        SceneManager.LoadScene("Main Menu");
    }

}
