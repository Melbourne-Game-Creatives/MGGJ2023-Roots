using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    private Camera myCam;

    [SerializeField] private RectTransform boxVisual;

    private Rect selectionBox;

    private Vector2 startPosition;
    private Vector2 endPosition;

    private void Start()
    {
        myCam = Camera.main;
        startPosition = Vector2.zero;
        endPosition = Vector2.zero;
        DrawVisual(); // remove initial rendered box from the screen
    }

    private void Update()
    {
        // when clicked
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }

        // when dragging
        if (Input.GetMouseButton(0))
        {
            endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        // when release click
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnits();
            startPosition = Vector2.zero;
            endPosition = Vector2.zero;
            DrawVisual();
        }

    }

    private void DrawVisual()
    {
        Vector2 boxStart = startPosition;
        Vector2 boxEnd = endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2f;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    private void DrawSelection()
    {
        // do X calculations
        if (Input.mousePosition.x < startPosition.x)
        {
            // dragging left
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPosition.x;
        }
        else
        {
            // dragging right
            selectionBox.xMin = startPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }
        // do Y calculations
        if (Input.mousePosition.y < startPosition.y)
        {
            // dragging down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPosition.y;
        }
        else
        {
            // dragging up
            selectionBox.yMin = startPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        foreach (ISelectable unit in UnitSelections.Instance.unitList)
        {

            if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.GetGameObject().transform.position)))
            {
                UnitSelections.Instance.DragSelect(unit);
            }
        }
    }
}
