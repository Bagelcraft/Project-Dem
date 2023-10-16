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
    //public TextMeshProUGUI circleTextPrefab;
    public int numberOfButtons = 5;
    public float minX = -755f;
    public float maxX = 755f;
    public float minY = -1800f;
    public float maxY = 1800f;
    public Vector3 buttonScale = new Vector3(2.290428f, 10.01263f, 5.8631f);

    private List<Button> buttons = new List<Button>();
    private List<Button> overlappingButtons = new List<Button>();
    private List<Vector3> takenPositions = new List<Vector3>();
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

        int maxLevel = Mathf.Min(40, level);

        if (correctButtonSequence.Count != maxLevel)
        {
            correctButtonSequence.Clear();
            for (int i = 1; i <= maxLevel; i++)
            {
                correctButtonSequence.Add(i.ToString());
            }
        }

        currentLevel = level;

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
            for (int i = 1; i <= level - 20; i++)
            {
                sequence.Add(i.ToString());
            }
        }
        else if (level >= 31 && level <= 40)
        {
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
        overlappingButtons.Clear();

        if (level <= 10)
        {
            correctButtonSequence = GenerateButtonSequence(level);
            int numButtons = correctButtonSequence.Count;

            // Adjust these values to cover the entire screen
            minX = -487f;
            maxX = 487f;
            minY = -1415f;
            maxY = 1415f;

            int rows = Mathf.CeilToInt(Mathf.Sqrt(numButtons));
            int columns = Mathf.CeilToInt(numButtons / (float)rows);
            CreateButtonsGrid(rows, columns, buttonPrefab);
        }
        else if (level >= 11 && level <= 20)
        {
            correctButtonSequence = GenerateButtonSequence(level);
            int numButtons = correctButtonSequence.Count;

            // Adjust these values to cover the entire screen
            minX = -487f;
            maxX = 487f;
            minY = -1415f;
            maxY = 1415f;

            int rows = Mathf.CeilToInt(Mathf.Sqrt(numButtons));
            int columns = Mathf.CeilToInt(numButtons / (float)rows);
            CreateButtonsGrid(rows, columns, buttonPrefab);
        }
        else if (level >= 21 && level <= 40)
        {
            // Generate the correct button sequence
            correctButtonSequence = GenerateButtonSequence(level);

            int numButtons = correctButtonSequence.Count;

            minX = -487f;
            maxX = 487f;
            minY = -1415f;
            maxY = 1415f;

            int rows = Mathf.CeilToInt(Mathf.Sqrt(numButtons));
            int columns = Mathf.CeilToInt(numButtons / (float)rows);

            // Generate grid positions with consistent spacing
            List<Vector3> gridPositions = GenerateGridPositions(rows, columns);

            // Shuffle the grid positions
            gridPositions = ShufflePositions(gridPositions);

            // Create a separate list to store the shuffled button positions
            List<Vector3> buttonPositions = new List<Vector3>(gridPositions);

            for (int i = 0; i < numButtons; i++)
            {
                Button prefabToUse = (i % 2 == 0) ? buttonPrefab : buttonPrefab2;

                // Use shuffled grid positions for randomized placement
                Vector3 position = buttonPositions[i];
                CreateRandomButton(correctButtonSequence[i], prefabToUse, position);
            }
        }



        // Add more level ranges if needed
    }


    private List<string> ShuffleLabels(List<string> labels)
    {
        for (int i = 0; i < labels.Count; i++)
        {
            int randomIndex = Random.Range(i, labels.Count);
            string temp = labels[i];
            labels[i] = labels[randomIndex];
            labels[randomIndex] = temp;
        }
        return labels;
    }

    private List<Vector3> ShufflePositions(List<Vector3> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            int randomIndex = Random.Range(i, positions.Count);
            Vector3 temp = positions[i];
            positions[i] = positions[randomIndex];
            positions[randomIndex] = temp;
        }
        return positions;
    }


    private void CreateButtonsGrid(int rows, int columns, Button buttonToUse)
    {
        ClearButtons();

        float buttonWidth = buttonScale.x * 2;
        float buttonHeight = buttonScale.y * 2;

        // Adjust these spacing values to control the separation between buttons
        float horizontalSpacing = (maxX - minX) / columns * 1.5f;
        float verticalSpacing = (maxY - minY) / rows * 1.1f;

        List<Vector3> gridPositions = new List<Vector3>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = minX + col * horizontalSpacing;
                float y = minY + row * verticalSpacing;
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

        gridPositions = ShuffleList(gridPositions);

        int numButtons = Mathf.Min(correctButtonSequence.Count, gridPositions.Count);

        for (int i = 0; i < numButtons; i++)
        {
            CreateRandomButton(correctButtonSequence[i], buttonToUse, gridPositions[i]);
        }
    }



    private List<Vector3> ShuffleList(List<Vector3> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            Vector3 temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }


    private List<Vector3> GenerateGridPositions(int rows, int columns)
    {
        float buttonWidth = buttonScale.x * 2;
        float buttonHeight = buttonScale.y * 2;
        float horizontalSpacing = (maxX - minX) / columns * 1.5f;
        float verticalSpacing = (maxY - minY) / rows * 1.1f;

        List<Vector3> gridPositions = new List<Vector3>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float x = minX + col * horizontalSpacing;
                float y = minY + row * verticalSpacing;
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }

        return gridPositions;
    }

    private List<Vector3> GenerateRandomPositions(int numPositions)
    {
        List<Vector3> positions = new List<Vector3>();

        for (int i = 0; i < numPositions; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomY = Random.Range(minY, maxY);
            positions.Add(new Vector3(randomX, randomY, 0f));
        }

        return positions;
    }

    private void CreateRandomButton(string buttonLabel, Button buttonToUse, Vector3 position)
    {
        Button newButton = Instantiate(buttonToUse, transform);
        newButton.transform.localPosition = position;
        newButton.transform.localScale = buttonScale;

        TextMeshProUGUI buttonText = Instantiate(buttonTextPrefab, newButton.transform);
        buttonText.text = buttonLabel;

        newButton.onClick.AddListener(() =>
        {
            HandleButtonClick(buttonLabel);
        });

        buttons.Add(newButton);

        if (IsOverlapping(newButton, position))
        {
            overlappingButtons.Add(newButton);
        }
    }

    private bool IsOverlapping(Button newButton, Vector3 position)
    {
        foreach (Button button in buttons)
        {
            if (button == newButton)
                continue;

            float distance = Vector3.Distance(button.transform.position, position);
            if (distance < buttonScale.x)
            {
                return true;
            }
        }

        return false;
    }

    private void ClearButtons()
    {
        foreach (Button button in buttons)
        {
            Destroy(button.gameObject);
        }
        buttons.Clear();
    }

    private void HandleButtonClick(string buttonLabel)
    {
        if (currentButtonToClick < correctButtonSequence.Count && buttonLabel == correctButtonSequence[currentButtonToClick])
        {
            playerInputSequence.Add(buttonLabel);
            currentButtonToClick++;

            if (currentButtonToClick >= correctButtonSequence.Count)
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
                    StartLevel(currentLevel + 1);
                }
                else
                {
                    Debug.Log("Incorrect sequence!");
                    StartLevel(currentLevel);
                }
            }
        }
    }
}
