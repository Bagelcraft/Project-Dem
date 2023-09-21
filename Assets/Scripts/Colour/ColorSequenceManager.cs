using System.Collections;
using System.Collections.Generic; // Import the namespace for lists.
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ColorSequenceManager : MonoBehaviour
        {
            public GameObject[] colorPanels; // Array of color panels.
            public TextMeshProUGUI messageText; // Reference to a TextMeshPro element to display messages.
            public float displayTime = 2f; // Time in seconds to display each color.
            public GameObject Buttons;

          public ColorSet colorSet;    

            public List<Color> colorSequence; // List to store the random color sequence.
            private int sequenceIndex = 0; // Index to keep track of the current color in the sequence.
            private int level = 1; // Current game level.
            public int colorsInSequence = 1; // Number of colors in the current sequence. Make it public.
            private bool playerInputEnabled = false; // Controls whether player input is enabled.
            public int playerLives = 3; // Initialize with 3 lives.

        public enum CustomColor
        {
            Red,
            Blue,
            Green,
            Yellow,
            White,
            Black,
            // Add more colors as needed
        }

        // Create a dictionary to map custom color names to sound names
        private Dictionary<CustomColor, string> colorSoundMap = new Dictionary<CustomColor, string>
    {
        { CustomColor.Red, "Red Note" },
        { CustomColor.Blue, "Blue Note" },
        { CustomColor.Green, "Green Note" },
        { CustomColor.Yellow, "Yellow Note" },
        { CustomColor.White, "White Note" },
        { CustomColor.Black, "Black Note" },
        // Add more mappings for other colors
    };

    private void Start()
            {
                // Initialize the color sequence (customize this list as needed).
                colorSequence = new List<Color> { Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.black };

                // Start the game with level 1.
                StartLevel(1);

                Buttons.SetActive(false);
            }

    private void StartLevel(int level)
    {
        this.level = level;
        colorsInSequence = level;
        sequenceIndex = 0;
        playerInputEnabled = false;

        // Clear the color sequence for the current level.
        colorSequence.Clear();

        // Generate a new random color sequence.
        for (int i = 0; i < colorsInSequence; i++)
        {
            int randomIndex = Random.Range(0, colorPanels.Length);
            Color randomColor = colorPanels[randomIndex].GetComponent<Image>().color; // Get the color from the panel.
            colorSequence.Add(randomColor);

            // Call ColourChecker to check and play the sound for the current color.
            ColourChecker(randomColor);
        }

        // Start displaying the color sequence for the current level.
        StartCoroutine(DisplayColorSequence());
    }


    // Coroutine to display the color sequence.
    private IEnumerator DisplayColorSequence()
    {
        for (int i = 0; i < colorsInSequence; i++)
        {
            Color currentColor = colorSequence[i];

            // Set the selected color panel to active (true) and others to inactive (false).
            for (int j = 0; j < colorPanels.Length; j++)
            {
                colorPanels[j].SetActive(colorPanels[j].GetComponent<Image>().color == currentColor);
            }

            // Call ColourChecker to check and play the sound for the current color.
            ColourChecker(currentColor);

            // Display the color for a set amount of time.
            yield return new WaitForSeconds(displayTime);

            // Hide all color panels.
            foreach (GameObject panel in colorPanels)
            {
                panel.SetActive(false);
            }

            // Move to the next color in the sequence.
            sequenceIndex++;
        }

        playerInputEnabled = true; // Enable player input.
        Buttons.SetActive(true);
    }

    public void HandleCorrectInput(List<Color> playerInputSequence)
    {
        Debug.Log("Correct Sequence: " + string.Join(", ", colorSequence));
        Debug.Log("Player Input Sequence: " + string.Join(", ", playerInputSequence));

        // Ensure both sequences have the same number of colors.
        if (playerInputSequence.Count != colorSequence.Count)
        {
            Debug.Log("Wrong!");

            // Deduct a life.
            playerLives--;

            // Check if the player has run out of lives.
            if (playerLives <= 0)
            {
                // Game over logic here.
                // You can restart the game, show a game over screen, etc.
                Debug.Log("Game Over");
                return;
            }
            else
            {
                // Restart the current level.
                Debug.Log("Try Again!");
                RestartCurrentLevel();
            }

            return;
        }

        // Compare the colors with a tolerance level.
        float tolerance = 0.2f; // Adjust this tolerance level as needed.

        for (int i = 0; i < colorSequence.Count; i++)
        {
            Color correctColor = colorSequence[i];
            Color playerColor = playerInputSequence[i];

            if (Mathf.Abs(correctColor.r - playerColor.r) > tolerance ||
                Mathf.Abs(correctColor.g - playerColor.g) > tolerance ||
                Mathf.Abs(correctColor.b - playerColor.b) > tolerance ||
                Mathf.Abs(correctColor.a - playerColor.a) > tolerance)
            {
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
                    return;
                }
                else
                {
                    // Restart the current level.
                    Debug.Log("Try Again!");
                    AudioManager.instance.PlaySound("Wrong Answer");
                    playerInputEnabled = false; // Disable input while transitioning to the next level.
                    // Deactivate answer buttons.
                    Buttons.SetActive(false);
                    RestartCurrentLevel();
                }

                return;
            }
        }

        Debug.Log("Correct!");
        AudioManager.instance.PlaySound("Correct Answer");
        // If the player has completed the current level's sequence.
        messageText.text = "Correct! Level " + level + " Complete!";
        playerInputEnabled = false; // Disable input while transitioning to the next level.
        
        // Deactivate answer buttons.
        Buttons.SetActive(false);

        // Start the next level after a delay (customize this delay).
        StartCoroutine(StartNextLevel());

        // Clear the player's input sequence after checking.
        colorSet.ClearList();
    }

    private IEnumerator StartNextLevel()
    {
        yield return new WaitForSeconds(2f); // Adjust the delay as needed.
        messageText.text = " ";
        StartLevel(level + 1); // Start the next level.
    }

    private void RestartCurrentLevel()
    {
        colorSet.ClearList();
        StartLevel(level);
    }

    private IEnumerator PlaySoundWithDelay(Color currentColor)
    {
        float soundDelay = 0.5f; // Adjust the delay as needed.

        yield return new WaitForSeconds(soundDelay);

        if (currentColor == Color.red)
        {
            AudioManager.instance.PlaySound("Red Note");
        }
        if (currentColor == Color.blue)
        {
            AudioManager.instance.PlaySound("Blue Note");
        }
        if (currentColor == Color.green)
        {
            AudioManager.instance.PlaySound("Green Note");
        }
        if (currentColor == Color.yellow)
        {
            AudioManager.instance.PlaySound("Yellow Note");
        }
        if (currentColor == Color.white)
        {
            AudioManager.instance.PlaySound("White Note");
        }
        if (currentColor == Color.black)
        {
            AudioManager.instance.PlaySound("Black Note");
        }
        // Add similar checks for other colors here.
    }

    private void ColourChecker(Color currentColor)
    {
        float tolerance = 0.2f; // Adjust this tolerance level as needed.

        foreach (KeyValuePair<CustomColor, string> kvp in colorSoundMap)
        {
            CustomColor customColor = kvp.Key;
            Color predefinedColor = GetColorFromEnum(customColor);

            if (ColorWithinTolerance(currentColor, predefinedColor, tolerance))
            {
                PlaySoundForColor(customColor);
                break;
            }
        }
    }

    private Color GetColorFromEnum(CustomColor customColor)
    {
        switch (customColor)
        {
            case CustomColor.Red:
                return Color.red;
            case CustomColor.Blue:
                return Color.blue;
            case CustomColor.Green:
                return Color.green;
            case CustomColor.Yellow:
                return Color.yellow;
            case CustomColor.White:
                return Color.white;
            case CustomColor.Black:
                return Color.black;
            default:
                return Color.white; // Default to white for unknown colors
        }
    }

    private void PlaySoundForColor(CustomColor customColor)
    {
        if (colorSoundMap.TryGetValue(customColor, out string soundName))
        {
            AudioManager.instance.PlaySound(soundName);
        }
    }

    private bool ColorWithinTolerance(Color a, Color b, float tolerance)
    {
        float redDiff = Mathf.Abs(a.r - b.r);
        float greenDiff = Mathf.Abs(a.g - b.g);
        float blueDiff = Mathf.Abs(a.b - b.b);
        float alphaDiff = Mathf.Abs(a.a - b.a);

        // Check if all color component differences are within the tolerance.
        return redDiff <= tolerance && greenDiff <= tolerance && blueDiff <= tolerance && alphaDiff <= tolerance;
    }

}
