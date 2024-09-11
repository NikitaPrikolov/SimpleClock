using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    
    public GameObject[] centerObjects; // ���������� ����� ���������� ������� � ������ portrait
    public GameObject timeTextbox;
    public GameObject inputField;
    public GameObject toggle;
    // ��������� ������� ��� landscape
    public Vector3 centerLandscapePosition;
    public Vector3 timeLandscapePosition;
    public Vector3 inputLandscapePosition;
    public Vector3 toggleLandscapePosition;

    // �������� ������� ��� portrait
    private Vector3 centerPortraitPosition;
    private Vector3 timePortraitPosition;
    private Vector3 fieldPortraitPosition;
    private Vector3 togglePortraitPosition;

    private void Start()
    {
        // ��������� �������� ������� ��� ������
        centerPortraitPosition = centerObjects[0].transform.position;

        timePortraitPosition = timeTextbox.transform.position;
        fieldPortraitPosition = inputField.transform.position;
        togglePortraitPosition = toggle.transform.position;
    }

    private void Update()
    {
        // �������� �� ��������� ����������
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
        {
            Invoke("SetLandscapePositions", 0.5f);
        }
        else
        {
            Invoke("SetPortraitPositions", 0.5f);
        }
    }

    private void SetLandscapePositions()
    {
        // ������������� ������� ��� ��������� � ������ landscape
        for (int i = 0; i < centerObjects.Length; i++)
        {
            centerObjects[i].transform.position = centerLandscapePosition;
        }

        timeTextbox.transform.position = timeLandscapePosition;
        inputField.transform.position = inputLandscapePosition;
        toggle.transform.position = toggleLandscapePosition;
    }

    private void SetPortraitPositions()
    {
        // ������������� �������� ������� ��� ��������� � ������ portrait
        for (int i = 0; i < centerObjects.Length; i++)
        {
            centerObjects[i].transform.position = centerPortraitPosition;
        }

        timeTextbox.transform.position = timePortraitPosition;
        inputField.transform.position = fieldPortraitPosition;
        toggle.transform.position = togglePortraitPosition;
    }
}
