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
    private int currentLevel = 1;
    public List<int> correctButtonSequence = new List<int>();
    public List<int> playerInputSequence = new List<int>();
    private int currentButtonToClick = 1;

    private void Start()
    {
        StartLevel(currentLevel);
    }

    private void StartLevel(int level)
    {
        currentLevel = level;
        correctButtonSequence = GenerateAscendingSequence(level);
        playerInputSequence.Clear();
        currentButtonToClick = 1;
        CreateButtonsForLevel(level);
    }

    private List<int> GenerateAscendingSequence(int length)
    {
        List<int> sequence = new List<int>();
        for (int i = 1; i <= length; i++)
        {
            sequence.Add(i);
        }
        return sequence;
    }

    private void CreateButtonsForLevel(int level)
    {
        ClearButtons();
        for (int i = 0; i < level; i++)
        {
            CreateRandomButton(correctButtonSequence[i]);
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

    private void CreateRandomButton(int buttonID)
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
            buttonText.text = buttonID.ToString();

            newButton.onClick.AddListener(() =>
            {
                HandleButtonClick(buttonID);
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

    private void HandleButtonClick(int buttonID)
    {
        if (buttonID == currentButtonToClick)
        {
            playerInputSequence.Add(buttonID);
            currentButtonToClick++;

            if (playerInputSequence.Count == correctButtonSequence.Count)
            {
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
