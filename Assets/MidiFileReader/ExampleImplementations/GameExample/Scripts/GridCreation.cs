using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreation : MonoBehaviour
{
    public GameObject gridLine;
    [SerializeField] private int numberOfVertLines = 12;
    public Vector3 vertScale;
    [SerializeField] private int numberOfHorizontalLines = 12;
    public Vector3 horizontalScale;

	private void Start()
    {
        FillGrid();
    }

    private void FillGrid()
    {
        Vector3 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));

        for (int i = 0; i < numberOfVertLines; i++)
        {
            float leftPos = -screenDimensions.x;
            float rightPos = screenDimensions.x;
            float distanceToIncrease = (rightPos - leftPos) / numberOfVertLines;
            Vector3 pos = new Vector3(leftPos + (distanceToIncrease * i), 0, 0);
            Quaternion rotation = new Quaternion(0, 0, 0, 0);
            GameObject newLine = Instantiate(gridLine, pos, rotation, this.transform);
            newLine.transform.localScale = vertScale;
        }

        for (int i = 0; i < numberOfVertLines; i++)
        {
            float topPos = screenDimensions.y;
            float bottomPos = -screenDimensions.y;
            float distanceToIncrease = (topPos - bottomPos) / numberOfHorizontalLines;
            Vector3 pos = new Vector3(0, bottomPos + (distanceToIncrease * i), 0);
            Quaternion rotation = new Quaternion(0, 0, 0, 0);
            GameObject newLine = Instantiate(gridLine, pos, rotation, this.transform);
            newLine.transform.localScale = horizontalScale;
        }
    }
}
