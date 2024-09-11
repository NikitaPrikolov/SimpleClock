using System;
using UnityEngine;

public class ClockFace : MonoBehaviour
{
    public Transform hourHand;   // Ссылка на стрелку часов
    public Transform minuteHand; // Ссылка на стрелку минут
    public Transform secondHand; // Ссылка на стрелку секунд

    public ClockManager clockManager; // Ссылка на ClockManager

    private void Update()
    {
        UpdateClockHands(); // Обновляем положение стрелок каждую кадр
    }

    private void UpdateClockHands()
    {
        string currentTimeString = clockManager.GetCurrentTime(); // Получаем текущее время из ClockManager
        if (DateTime.TryParse(currentTimeString, out DateTime currentTime))
        {
            // Вычисляем угол для каждой стрелки
            float hourAngle = (currentTime.Hour % 12 + currentTime.Minute / 60f) * 30f; // 360 / 12 = 30
            float minuteAngle = (currentTime.Minute + currentTime.Second / 60f) * 6f; // 360 / 60 = 6
            float secondAngle = currentTime.Second * 6f; // 360 / 60 = 6

            // Применяем углы к стрелкам
            hourHand.localRotation = Quaternion.Euler(0, 0, -hourAngle);
            minuteHand.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
            secondHand.localRotation = Quaternion.Euler(0, 0, -secondAngle);
        }
    }
}
