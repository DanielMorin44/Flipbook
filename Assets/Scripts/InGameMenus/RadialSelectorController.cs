using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class RadialSelectorController : MonoBehaviour
{
    public GameObject cancelCircle;
    public GameObject pointer;
    public GameObject backCircle;
    public GameObject dividingLinePrefab;
    public LevelManager levelManager;
    public InputController inputController;
    public float maxSize;
    public float minSize;

    private Vector3 openLocation;
    private Vector3 currentLocation;
    private Camera cam;
    private float pointerLength;
    private float pointerAngle;
    private float cancelRadius = 0.5f;
    private GameObject[] dividingLines;
    private int sectors;
    private int currentHover;

    private void Update()
    {
        currentLocation = cam.ScreenToWorldPoint(Input.mousePosition);
        RedrawPointer();
        currentHover = CheckSector();
        levelManager.SetOnlyOpen(currentHover);
    }

    private void OnEnable()
    {
        if(cam == null)
        {
            DoFirstTimeSetup();
        }
        openLocation = cam.ScreenToWorldPoint(Input.mousePosition);
        pointer.transform.localScale = new Vector2(pointer.transform.localScale.x, 0);
        backCircle.transform.position = new Vector3(openLocation.x, openLocation.y, 2);
        cancelCircle.transform.position = new Vector3(openLocation.x, openLocation.y, 3);
        pointer.transform.position = new Vector3(openLocation.x, openLocation.y, 4);
        SetDividerLocation();
    }

    private void OnDisable()
    {
        inputController.CompleteFlip(currentHover);
    }

    private void DoFirstTimeSetup()
    {
        cam = Camera.main;
        openLocation = cam.ScreenToWorldPoint(Input.mousePosition);
        EstablishSectors();
        currentHover = sectors-1;
    }

    private void RedrawPointer()
    {
        float xdiff = currentLocation.x - openLocation.x;
        float ydiff = currentLocation.y - openLocation.y;
        pointerLength = Mathf.Clamp(Mathf.Sqrt(Mathf.Pow(xdiff, 2) + Mathf.Pow(ydiff, 2)), minSize, maxSize);
        pointerAngle = (Mathf.Atan2(xdiff, ydiff) * Mathf.Rad2Deg);
        if (float.IsNaN(pointerAngle)) pointerAngle = 0;
        float xsize = pointerLength * Mathf.Sin(pointerAngle * Mathf.Deg2Rad) / 2;
        float ysize = pointerLength * Mathf.Cos(pointerAngle * Mathf.Deg2Rad) / 2;

        // Set Size
        pointer.transform.localScale = new Vector2(pointer.transform.localScale.x, pointerLength);
        // Set Rotation
        pointer.transform.rotation = Quaternion.Euler(Vector3.forward * -pointerAngle);
        // Set Position
        pointer.transform.position = new Vector2(openLocation.x + xsize, openLocation.y + ysize);
    }

    private void SetDividerLocation()
    {
        for (int i = 0; i < dividingLines.Length; i++)
        {
            float curDegree = i * 360/dividingLines.Length;
            float xsize = backCircle.transform.localScale.x * Mathf.Sin(curDegree * Mathf.Deg2Rad) / 2;
            float ysize = backCircle.transform.localScale.y * Mathf.Cos(curDegree * Mathf.Deg2Rad) / 2;
            dividingLines[i].transform.position = new Vector3(openLocation.x + xsize/2, openLocation.y + ysize/2, 3);
        }
    }

    private void EstablishSectors()
    {
        sectors = levelManager.GetNumberPagesAvailable();
        dividingLines = new GameObject[sectors];
        float degPerSector = 360 / sectors;
        for (int i = 0; i < sectors; i++)
        {
            float curDegree = i * degPerSector;
            float xsize = backCircle.transform.localScale.x * Mathf.Sin(curDegree * Mathf.Deg2Rad) / 2;
            float ysize = backCircle.transform.localScale.y * Mathf.Cos(curDegree * Mathf.Deg2Rad) / 2;

            dividingLines[i] = Instantiate(dividingLinePrefab, openLocation, Quaternion.identity);
            // Set Size
            dividingLines[i].transform.localScale = new Vector2(pointer.transform.localScale.x, backCircle.transform.localScale.y/2);
            // Set Rotation
            dividingLines[i].transform.rotation = Quaternion.Euler(Vector3.forward * -curDegree);
            // Set Position
            dividingLines[i].transform.position = new Vector3(openLocation.x + xsize/2, openLocation.y + ysize/2, 3);

            dividingLines[i].transform.parent = gameObject.transform;
        }
    }

    private int CheckSector()
    {
        if (pointerLength <= cancelRadius)
        {
            return levelManager.GetCurrentPage();
        }
        else
        {
            return (int) ((pointerAngle + 360) % 360) / (360 / sectors);
        }
    }
}
