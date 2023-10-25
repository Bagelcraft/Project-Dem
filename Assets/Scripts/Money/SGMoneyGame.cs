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
    private float initialTime = 180.0f;
    private float remainingTime;

    // Define stage thresholds
    public int[] stageThresholds = { 100, 200, 300, 40, 50 };
    public int currentStage = 1;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        UpdateStageBasedOnScore();
        GenerateNewTargetSum();
        remainingTime = initialTime;
        UpdateUI();
    }

    private void UpdateStageBasedOnScore()
    {
        for (int i = 0; i < stageThresholds.Length; i++)
        {
            if (score < stageThresholds[i])
            {
                currentStage = i + 1;
                return;
            }
        }
        currentStage = stageThresholds.Length;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;

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
            score += 10;
            ResetSelection();
            GenerateNewTargetSum();
        }
        else if (currentSum > targetSum)
        {
            feedbackText.text = "Too much!";
            score = Mathf.Max(score - 5, 0);
            remainingTime = initialTime;
            ResetSelection();
            GenerateNewTargetSum();
        }
    }

    public void GenerateNewTargetSum()
    {
        UpdateStageBasedOnScore();

        switch (currentStage)
        {
            case 1:
                targetSum = Random.Range(1, 10) * 100;
                break;
            case 2:
                targetSum = Random.Range(10, 100) * 100;
                break;
            case 3:
                targetSum = Random.Range(20, 200) * 5;
                break;
            case 4:
                targetSum = Random.Range(200, 2000) * 5;
                break;
            case 5:
                targetSum = Random.Range(10, 100) * 1000;
                break;
            case 6:
                targetSum = Random.Range(2000, 20000) * 5;
                break;
        }

        currentSum = 0;
        UpdateUI();
    }

    public void ResetSelection()
    {
        currentSum = 0;
        feedbackText.text = string.Empty;
        UpdateUI();
    }

    // The rest of your SelectDenomination methods remain unchanged.
    public void Select5Cents() => SelectDenomination(5);
    public void Select10Cents() => SelectDenomination(10);
    public void Select20Cents() => SelectDenomination(20);
    public void Select50Cents() => SelectDenomination(50);
    public void Select1Dollar() => SelectDenomination(100);
    public void Select2Dollar() => SelectDenomination(200);
    public void Select5Dollar() => SelectDenomination(500);
    public void Select10Dollar() => SelectDenomination(1000);
    public void Select50Dollar() => SelectDenomination(5000);
}
