using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomButtonPlacement : MonoBehaviour
{
    public Button buttonPrefab;
    public Button buttonPrefab2; // Reference to the new button prefab
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
        playerInputSequence.Clear();
        currentButtonToClick = 0;

        // Adjust the max level to 30
        int maxLevel = Mathf.Min(40, level);

        // Check if we need to generate a new correctButtonSequence
        if (correctButtonSequence.Count != maxLevel)
        {
            correctButtonSequence.Clear();
            for (int i = 1; i <= maxLevel; i++)
            {
                correctButtonSequence.Add(i.ToString());
            }
        }

        // Update the currentLevel here.
        currentLevel = level; // This line was missing

        CreateButtonsForLevel(maxLevel);
    }




    private List<string> GenerateButtonSequence(int level)
    {
        List<string> sequence = new List<string>();

        if (level <= 10)
        {
            for (int i = 1; i <= level; i++)
            {
                sequence.Add(i.ToString());
            }
        }
        else if (level >= 11 && level <= 20)
        {
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
        else if (level >= 21 && level <= 30)
        {

            // Generate unique numbers from 1 to level - 20.
            for (int i = 1; i <= level - 20; i++)
            {
                sequence.Add(i.ToString());
            }
        }
        else if (level >= 31 && level <= 40)
        {

            // Generate unique numbers from 1 to level - 20.
            for (int i = 1; i <= level - 30; i++)
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

        if (level <= 10)
        {
            correctButtonSequence = GenerateButtonSequence(level);

            for (int i = 0; i < level; i++)
            {
                CreateRandomButton(correctButtonSequence[i], buttonPrefab);
            }
        }
        else if (level >= 11 && level <= 20)
        {
            correctButtonSequence = GenerateButtonSequence(level);

            for (int i = 0; i < level - 10; i++)
            {
                CreateRandomButton(correctButtonSequence[i], buttonPrefab);
            }
        }
        else if (level >= 21 && level <= 30)
        {
            correctButtonSequence = GenerateButtonSequence(level);

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    CreateRandomButton(correctButtonSequence[i], buttonPrefab);
                }
                else
                {
                    CreateRandomButton(correctButtonSequence[i], buttonPrefab2);
                }
            }
        }
        else if (level >= 31 && level <= 40)
        {
            correctButtonSequence = GenerateButtonSequence(level);

            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    CreateRandomButton(correctButtonSequence[i], buttonPrefab);
                }
                else
                {
                    CreateRandomButton(correctButtonSequence[i], buttonPrefab2);
                }
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

    private void CreateRandomButton(string buttonLabel, Button buttonToUse)
    {
        Button newButton = Instantiate(buttonToUse, transform);

        Vector3 randomPosition;
        int maxAttempts = 1000000;
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
    if (currentButtonToClick < correctButtonSequence.Count && buttonLabel == correctButtonSequence[currentButtonToClick])
    {
        playerInputSequence.Add(buttonLabel);
        currentButtonToClick++;

        if (currentButtonToClick >= correctButtonSequence.Count)
        {
            // All buttons in the sequence have been clicked.
            bool isCorrect = true;

            for (int i = 0; i < correctButtonSequence.Count; i++)
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
