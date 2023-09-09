using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorSequenceManager : MonoBehaviour
{
    public GameObject[] colorPanels; // Array of color panels.
    public TextMeshProUGUI messageText; // Reference to a TextMeshPro element to display messages.
    public float displayTime = 2f; // Time in seconds to display each color.

    private Color[] colorSequence; // Array to store the random color sequence.
    private int sequenceIndex = 0; // Index to keep track of the current color in the sequence.
    private int level = 1; // Current game level.
    private int colorsInSequence = 1; // Number of colors in the current sequence.

    private void Start()
    {
        // Initialize the color sequence (customize this array as needed).
        colorSequence = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.white, Color.black };

        // Start the game with level 1.
        StartLevel(1);
    }

    private void StartLevel(int level)
    {
        this.level = level;
        colorsInSequence = level;
        sequenceIndex = 0;

        // Start displaying the color sequence for the current level.
        StartCoroutine(DisplayColorSequence());
    }

    // Coroutine to display the color sequence.
    private IEnumerator DisplayColorSequence()
    {
        for (int i = 0; i < colorsInSequence; i++)
        {
            // Choose a random color from the sequence.
            int randomIndex = Random.Range(0, colorPanels.Length);
            Color randomColor = colorSequence[randomIndex];

            // Set the selected color panel to active (true) and others to inactive (false).
            for (int j = 0; j < colorPanels.Length; j++)
            {
                colorPanels[j].SetActive(j == randomIndex);
            }

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

        // Display a message when the sequence is done.
        messageText.text = "Level " + level + " Complete!";

        // Start the next level after a delay (you can customize this delay).
        yield return new WaitForSeconds(1f);
        StartLevel(level + 1);
    }
}
