using UnityEngine;
using UnityEngine.UI;

public class LineDrawing : MonoBehaviour
{
    public LineRenderer lineRenderer;
    private int linePointCount = 0;
    private Vector3[] linePoints;

    private Button[] buttons;
    private int currentButtonIndex = 0;

    private void Start()
    {
        linePoints = new Vector3[100]; // Adjust this size as needed.
        buttons = FindObjectsOfType<Button>();
    }

    private void Update()
    {
        // Check if there are any touches on the screen.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch (assuming single touch gameplay).

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Button button = hit.collider.GetComponent<Button>();

                    if (button != null)
                    {
                        if (currentButtonIndex < buttons.Length && button == buttons[currentButtonIndex])
                        {
                            // Correct button touched, draw a line to it.
                            Vector3 buttonPosition = button.transform.position;
                            buttonPosition.z = 0; // Ensure the z-coordinate is appropriate for your 2D setup.
                            AddPointToLine(buttonPosition);
                            currentButtonIndex++;
                        }
                        else
                        {
                            // Incorrect button touched, reset the line.
                            ResetLine();
                        }
                    }
                }
            }
        }
    }

    private void AddPointToLine(Vector3 point)
    {
        if (linePointCount < linePoints.Length)
        {
            linePoints[linePointCount] = point;
            linePointCount++;
            lineRenderer.positionCount = linePointCount;
            lineRenderer.SetPositions(linePoints);
        }
    }

    private void ResetLine()
    {
        linePointCount = 0;
        lineRenderer.positionCount = linePointCount;
        currentButtonIndex = 0;
    }
}
