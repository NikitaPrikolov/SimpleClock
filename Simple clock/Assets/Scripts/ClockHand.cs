using UnityEngine;

public class ClockHand : MonoBehaviour
{
    public AlarmManager alarmManager; // ������ �� AlarmManager
    public bool isHourHand; // �������� �� ������� �������

    private Camera mainCamera; // ������ �� �������� ������
    private Vector3 screenPosition; // ��������� ������� � �������� �����������
    private float angleOffset; // �������� ���� ��� �������
    private Collider2D collider; // ��������� �������

    private bool isDragging; // ���������, ���� �� � ������ ������ �������������� �������

    private void Start()
    {
        mainCamera = Camera.main; // �������� ������ �� �������� ������
        collider = GetComponent<Collider2D>(); // �������� ��������� �������
    }

    private void Update()
    {
        // ������������ ������� ������
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 inputPosition = (Input.touchCount > 0) ? Input.GetTouch(0).position : Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10)); // Z=10 ��� ���������� �����������

            if (collider == Physics2D.OverlapPoint(worldPosition))
            {
                screenPosition = mainCamera.WorldToScreenPoint(transform.position);
                Vector3 inputVector = inputPosition - screenPosition;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(inputVector.y, inputVector.x)) * Mathf.Rad2Deg;
                isDragging = true; // ������������� ���� ��������������
            }
        }

        // ������������ �������� ���� ��� �������
        if (isDragging && (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)))
        {
            Vector3 inputPosition = (Input.touchCount > 0) ? Input.GetTouch(0).position : Input.mousePosition;
            Vector3 inputVector = inputPosition - screenPosition;
            float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);
            UpdateTime(); // ��������� ����� � timeInputField �� ���� ��������
        }

        // ��������� ���������� ��������������
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false; // ���������� ���� ��������������
        }
    }

    private void UpdateTime()
    {
        float angle = transform.rotation.eulerAngles.z;

        // ��������� �������� ������� ����� � �����, ���� ��� ���� �����������
        if (isHourHand)
        {
            // ��������� ������� �������� �����
            int newHour = (24 - Mathf.FloorToInt(angle / 15)) % 24;
            alarmManager.currentHour = newHour; // ��������� � AlarmManager
        }
        else
        {
            // ��������� ������� �������� �����
            int newMinute = (60 - Mathf.FloorToInt(angle / 6)) % 60;
            alarmManager.currentMinute = newMinute; // ��������� � AlarmManager
        }

        // ��������� ���� ����� �������
        UpdateTimeInputField();
    }

    private void UpdateTimeInputField()
    {
        // ����������� ����� � ��������� ��������� ����
        string timeString = $"{alarmManager.currentHour:00}:{alarmManager.currentMinute:00}";
        alarmManager.timeInputField.text = timeString;
    }
}
