using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public GameObject alarmPanel;
    public Text timeDisplay; // ������ �� Text ������� ��� ����������� �������
    private DateTime currentRealTime; // ���������� ��� �������� ���������� �������
    private bool isSynchronized = false; // ���� ��� ������������ ������������� �������

    private void Start()
    {
        // ����������� ������������� �� ���������, ��� ��� ����� ����� �������� �� TimeService
    }

    public void UpdateTime(string timeString)
    {
        // �������� ���������� � ���������� ������� �����
        try
        {
            currentRealTime = DateTime.Parse(timeString);
            isSynchronized = true; // ������������� ���� �������������
            UpdateTimeDisplay(); // ��������� ������������ ����� ����� ����� �������������
            StartCoroutine(UpdateClockEverySecond()); // ��������� �������� ���������� �������
        }
        catch (FormatException)
        {
            Debug.LogError("������ ��� �������� �������.");
        }
    }

    private IEnumerator UpdateClockEverySecond()
    {
        while (isSynchronized) // ��������� �����, ���� ����������������
        {
            currentRealTime = currentRealTime.AddSeconds(1); // ��������� �������
            UpdateTimeDisplay(); // ��������� ������������ �����
            yield return new WaitForSeconds(1); // ���� 1 �������
        }
    }

    public void OpenAlarmPanel()
    {
        alarmPanel.SetActive(true);
    }

    public void SynchronizeTime(string newTimeString)
    {
        // �������� ���������� � ���������������� � ����� ��������
        try
        {
            currentRealTime = DateTime.Parse(newTimeString);
            UpdateTimeDisplay(); // ��������� ������������ ����� ����� �������� �������������
        }
        catch (FormatException)
        {
            Debug.LogError("������ ��� �������� ������ �������, ������� ����� �� ���������.");
        }
    }

    public string GetCurrentTime()
    {
        return currentRealTime.ToString("HH:mm:ss"); // ���������� ������� ����� � ��������� �������
    }

    private void UpdateTimeDisplay()
    {
        // ��������� ��������� ���� � ������� ��������
        timeDisplay.text = currentRealTime.ToString("HH:mm:ss"); // ����������� �����
    }
}
