using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SGMoneyGame : MonoBehaviour
{
    public TextMeshProUGUI targetSumText;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI scoreText;
    public Slider timerSlider;
    public TextMeshProUGUI timerText;

    public int targetSum;
    public int currentSum;

    public int[] denominations = { 5, 10, 20, 50, 100, 200, 500, 1000, 5000 };

    public int score = 0;
    private float initialTime = 300.0f;
    private float remainingTime;

    // Define stage thresholds
    public int[] stageThresholds = { 100, 200, 300 }; // Example: Progress every 100 points
    private int currentStage = 1;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        targetSum = Random.Range(20, 180) * 5;
        currentSum = 0;
        remainingTime = initialTime;
        UpdateUI();
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;

        // Check if it's time to progress to the next stage
        if (currentStage < stageThresholds.Length && score >= stageThresholds[currentStage - 1])
        {
            currentStage++;
            // Implement changes related to the new stage here (e.g., update targetSum).
            // You can switch cases based on the currentStage to define different behaviors.

            if (currentStage == 2)
            {

            }
        }

        if (remainingTime <= 0)
        {
            InitializeGame();
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        targetSumText.text = "Target: $" + (targetSum / 100.0).ToString("F2");
        scoreText.text = "Score: " + score;
        timerSlider.value = remainingTime / initialTime;

        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void SelectDenomination(int denominationValue)
    {
        currentSum += denominationValue;
        UpdateUI();

        if (currentSum == targetSum)
        {
            feedbackText.text = "Correct!";
            score += 10; // Increment the score for a correct answer (adjust the value as needed).
            ResetSelection(); // Automatically reset the selection for the next round.
            InitializeGame();
        }
        else if (currentSum > targetSum)
        {
            feedbackText.text = "Too much!";
            score = Mathf.Max(score - 5, 0); // Decrement the score for an incorrect answer but not below zero.
            ResetSelection();
            InitializeGame();
        }
    }

    public void Select5Cents()
    {
        SelectDenomination(5); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select10Cents()
    {
        SelectDenomination(10); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select20Cents()
    {
        SelectDenomination(20); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select50Cents()
    {
        SelectDenomination(50); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select1Dollar()
    {
        SelectDenomination(100); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select2Dollar()
    {
        SelectDenomination(200); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select5Dollar()
    {
        SelectDenomination(500); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select10Dollar()
    {
        SelectDenomination(1000); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void Select50Dollar()
    {
        SelectDenomination(5000); // Calls the generic SelectDenomination method with the value for 5 cents.
    }

    public void ResetSelection()
    {
        currentSum = 0;
        feedbackText.text = string.Empty;
        UpdateUI();
    }
}
