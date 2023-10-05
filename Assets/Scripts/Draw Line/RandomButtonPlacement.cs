using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomButtonPlacement : MonoBehaviour
{
    public Button buttonPrefab;
    public TextMeshProUGUI buttonTextPrefab;
    public int numberOfButtons = 5;
    public float minX = -755f;
    public float maxX = 755f;
    public float minY = -1800f;
    public float maxY = 1800f;
    public Vector3 buttonScale = new Vector3(2.290428f, 10.01263f, 5.8631f);

    private List<Button> buttons = new List<Button>();
    public int currentLevel = 1;
    public List<string> correctButtonSequence = new List<string>();
    public List<string> playerInputSequence = new List<string>();
    private int currentButtonToClick = 0;

    private void Start()
    {
        StartLevel(currentLevel);
    }

    private void StartLevel(int level)
    {
        currentLevel = level;

        // Reset the list at level 11.
        if (level == 11)
        {
            correctButtonSequence.Clear();
        }
        else
        {
            correctButtonSequence = GenerateButtonSequence(level);
        }

        playerInputSequence.Clear();
        currentButtonToClick = 0;

        CreateButtonsForLevel(level);
    }

    private List<string> GenerateButtonSequence(int level)
    {
        List<string> sequence = new List<string>();

        // Levels 1 to 10: Display numbers.
        if (level <= 10)
        {
            for (int i = 1; i <= level; i++)
            {
                sequence.Add(i.ToString());
            }
        }
        else
        {
            // Start the alternation at level 11.
            for (int i = 1; i <= level - 10; i++)
            {
                if (i % 2 == 1)
                {
                    sequence.Add(((i + 1) / 2).ToString());
                }
                else
                {
                    sequence.Add(((char)('A' + (i / 2 - 1))).ToString());
                }
            }
        }

        return sequence;
    }

    private void CreateButtonsForLevel(int level)
    {
        ClearButtons();

        // Check if the level is less than or equal to 10.
        if (level <= 10)
        {
            correctButtonSequence = GenerateButtonSequence(level);

            for (int i = 0; i < level; i++)
            {
                CreateRandomButton(correctButtonSequence[i]);
            }
        }
        else if (level >= 11 && level <= 20)
        {
            // Start the alternation at level 11.
            correctButtonSequence = GenerateButtonSequence(level);

            for (int i = 0; i < level - 10; i++)
            {
                CreateRandomButton(correctButtonSequence[i]);
            }
        }
    }


    private void ClearButtons()
    {
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
    }

    private void CreateRandomButton(string buttonLabel)
    {
        Button newButton = Instantiate(buttonPrefab, transform);

        Vector3 randomPosition;
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            randomPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
            attempts++;
        } while (IsOverlapping(newButton, randomPosition) && attempts < maxAttempts);

        if (attempts < maxAttempts)
        {
            newButton.transform.localPosition = randomPosition;
            newButton.transform.localScale = buttonScale;

            TextMeshProUGUI buttonText = Instantiate(buttonTextPrefab, newButton.transform);
            buttonText.text = buttonLabel;

            newButton.onClick.AddListener(() =>
            {
                HandleButtonClick(buttonLabel);
            });

            buttons.Add(newButton);
        }
        else
        {
            Debug.LogWarning("Button placement failed. Increase spacing or reduce the number of buttons.");
        }
    }

    private bool IsOverlapping(Button newButton, Vector3 position)
    {
        foreach (Button button in buttons)
        {
            float distance = Vector3.Distance(button.transform.position, position);
            if (distance < buttonScale.x)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleButtonClick(string buttonLabel)
    {
        if (buttonLabel == correctButtonSequence[currentButtonToClick])
        {
            playerInputSequence.Add(buttonLabel);
            currentButtonToClick++;

            if (currentButtonToClick >= correctButtonSequence.Count)
            {
                // All buttons in the sequence have been clicked.
                bool isCorrect = true;

                for (int i = 0; i < currentLevel - 10; i++)
                {
                    if (playerInputSequence[i] != correctButtonSequence[i])
                    {
                        isCorrect = false;
                        break;
                    }
                }

                if (isCorrect)
                {
                    Debug.Log("Correct sequence!");

                    // Start the next level.
                    StartLevel(currentLevel + 1);
                }
                else
                {
                    Debug.Log("Incorrect sequence!");

                    // Restart the current level.
                    StartLevel(currentLevel);
                }
            }
        }
    }
}
