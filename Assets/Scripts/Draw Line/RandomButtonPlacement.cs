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
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;
    public Vector3 buttonScale = new Vector3(0.5f, 0.5f, 1f);

    private int currentButtonID = 1;
    private int[] correctButtonSequence;
    private List<int> playerInputSequence = new List<int>();

    private void Start()
    {
        correctButtonSequence = new int[] { 1, 2, 3, 4, 5 };

        for (int i = 0; i < numberOfButtons; i++)
        {
            CreateRandomButton();
        }
    }

    private void CreateRandomButton()
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

            int buttonID = currentButtonID;
            currentButtonID++;

            TextMeshProUGUI buttonText = Instantiate(buttonTextPrefab, newButton.transform);
            buttonText.text = buttonID.ToString();

            newButton.onClick.AddListener(() =>
            {
                HandleButtonClick(buttonID);
            });
        }
        else
        {
            Debug.LogWarning("Button placement failed. Increase spacing or reduce the number of buttons.");
        }
    }

    private bool IsOverlapping(Button newButton, Vector3 position)
    {
        foreach (Transform child in transform)
        {
            if (child == newButton.transform)
            {
                continue;
            }

            float distance = Vector3.Distance(child.transform.position, position);
            if (distance < buttonScale.x)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleButtonClick(int buttonID)
    {
        playerInputSequence.Add(buttonID);

        bool isCorrect = CheckPlayerInput();

        if (isCorrect)
        {
            Debug.Log("Correct sequence!");
            playerInputSequence.Clear();
        }
        else
        {
            Debug.Log("Incorrect sequence!");
            playerInputSequence.Clear();
        }
    }

    private bool CheckPlayerInput()
    {
        if (playerInputSequence.Count != correctButtonSequence.Length)
        {
            return false;
        }

        for (int i = 0; i < playerInputSequence.Count; i++)
        {
            if (playerInputSequence[i] != correctButtonSequence[i])
            {
                return false;
            }
        }

        return true;
    }
}
