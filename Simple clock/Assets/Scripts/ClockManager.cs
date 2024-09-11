using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : MonoBehaviour
{
    public GameObject alarmPanel;
    public Text timeDisplay; // Ссылка на Text элемент для отображения времени
    private DateTime currentRealTime; // Переменная для хранения подлинного времени
    private bool isSynchronized = false; // Флаг для отслеживания синхронизации времени

    private void Start()
    {
        // Изначальная инициализация не требуется, так как время будет получено из TimeService
    }

    public void UpdateTime(string timeString)
    {
        // Пытаемся распарсить и установить текущее время
        try
        {
            currentRealTime = DateTime.Parse(timeString);
            isSynchronized = true; // Устанавливаем флаг синхронизации
            UpdateTimeDisplay(); // Обновляем отображаемое время сразу после синхронизации
            StartCoroutine(UpdateClockEverySecond()); // Запускаем корутину обновления времени
        }
        catch (FormatException)
        {
            Debug.LogError("Ошибка при парсинге времени.");
        }
    }

    private IEnumerator UpdateClockEverySecond()
    {
        while (isSynchronized) // Обновляем время, пока синхронизировано
        {
            currentRealTime = currentRealTime.AddSeconds(1); // Добавляем секунду
            UpdateTimeDisplay(); // Обновляем отображаемое время
            yield return new WaitForSeconds(1); // Ждем 1 секунду
        }
    }

    public void OpenAlarmPanel()
    {
        alarmPanel.SetActive(true);
    }

    public void SynchronizeTime(string newTimeString)
    {
        // Пытаемся распарсить и синхронизировать с новым временем
        try
        {
            currentRealTime = DateTime.Parse(newTimeString);
            UpdateTimeDisplay(); // Обновляем отображаемое время после успешной синхронизации
        }
        catch (FormatException)
        {
            Debug.LogError("Ошибка при парсинге нового времени, текущее время не обновлено.");
        }
    }

    public string GetCurrentTime()
    {
        return currentRealTime.ToString("HH:mm:ss"); // Возвращаем текущее время в строковом формате
    }

    private void UpdateTimeDisplay()
    {
        // Обновляем текстовое поле с текущим временем
        timeDisplay.text = currentRealTime.ToString("HH:mm:ss"); // Форматируем время
    }
}
