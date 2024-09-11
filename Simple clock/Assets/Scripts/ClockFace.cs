using System;
using UnityEngine;

public class ClockFace : MonoBehaviour
{
    public Transform hourHand;   // ������ �� ������� �����
    public Transform minuteHand; // ������ �� ������� �����
    public Transform secondHand; // ������ �� ������� ������

    public ClockManager clockManager; // ������ �� ClockManager

    private void Update()
    {
        UpdateClockHands(); // ��������� ��������� ������� ������ ����
    }

    private void UpdateClockHands()
    {
        string currentTimeString = clockManager.GetCurrentTime(); // �������� ������� ����� �� ClockManager
        if (DateTime.TryParse(currentTimeString, out DateTime currentTime))
        {
            // ��������� ���� ��� ������ �������
            float hourAngle = (currentTime.Hour % 12 + currentTime.Minute / 60f) * 30f; // 360 / 12 = 30
            float minuteAngle = (currentTime.Minute + currentTime.Second / 60f) * 6f; // 360 / 60 = 6
            float secondAngle = currentTime.Second * 6f; // 360 / 60 = 6

            // ��������� ���� � ��������
            hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
            secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
        }
    }
}
