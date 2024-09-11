using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class AlarmManager : MonoBehaviour
{
    public GameObject alarmPanel;
    public GameObject backButton;
    public GameObject inputField;
    public GameObject alarmCircle;
    public InputField timeInputField; // Поле ввода времени
    public Button setAlarmButton; // Кнопка для установки будильника
    public AudioClip alarmSound; // Звук будильника
    public Text alarmTimeText; // Текст для отображения установленного времени
    private AudioSource audioSource; // Компонент для проигрывания звука
    public ClockManager clockManager; // Ссылка на ClockManager
    private DateTime alarmTime; // Время будильника
    private bool alarmSet = false; // Флаг, установлено ли время будильника
    public Animation buttonAnimation;

    public int currentHour = 0; // Текущее значение часов
    public int currentMinute = 0; // Текущее значение минут
    public int currentSecond = 0;

    private void Start()
    {
        timeInputField.text = "00:00";
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = alarmSound;
        audioSource.loop = true;

        // Подписка на событие нажатия кнопки
        setAlarmButton.onClick.AddListener(SetAlarm);
        setAlarmButton.GetComponentInChildren<Text>().text = "ON";
    }

    private void Update()
    {
        // Проверяем, совпадает ли текущее время с временем будильника
        if (alarmSet && clockManager != null && !audioSource.isPlaying)
        {
            DateTime currentTime = DateTime.Parse(clockManager.GetCurrentTime());
            if (currentTime.Hour == alarmTime.Hour && currentTime.Minute == alarmTime.Minute && currentTime.Second == 00)
            {
                RingAlarm();
            }
        }

        // Проверяем корректность ввода времени
        ValidateTimeInput();
    }

    // Метод для валидации ввода времени
    private void ValidateTimeInput()
    {
        string input = timeInputField.text;

        // Проверка на превышение длины
        if (input.Length > 5)
        {
            timeInputField.text = "00:00"; // Сбрасываем на корректное значение
            return;
        }

        // Проверка на разрешенные символы (цифры и знак ":")
        if (!Regex.IsMatch(input, @"^[0-9:]*$"))
        {
            timeInputField.text = "00:00"; // Сбрасываем на корректное значение
            return;
        }

        // Проверка длины для дальнейших проверок
        if (input.Length == 5)
        {
            // Проверка на наличие двоеточия на третьей позиции
            if (input[2] != ':')
            {
                timeInputField.text = "00:00"; // Сбрасываем на корректное значение
                return;
            }

            // Проверка корректности значений
            if (input[0] > '2' || (input[0] == '2' && input[1] > '3') ||
                input[3] > '5' || input[4] > '9')
            {
                timeInputField.text = "00:00"; // Сбрасываем на корректное значение
                return;
            }

            // Если все проверки пройдены, сохраняем значения текущего часа и минуты
            string[] timeParts = input.Split(':');
            if (timeParts.Length == 2 &&
                int.TryParse(timeParts[0], out int hour) &&
                int.TryParse(timeParts[1], out int minute))
            {
                currentHour = hour;
                currentMinute = minute;
            }
        }
    }



    public void SetAlarm()
    {
        if (alarmSet)
        {
            buttonAnimation.Play("ToggleOFF");
            // Если будильник уже установлен, отменяем его
            audioSource.Stop();
            alarmSet = false;
            backButton.SetActive(true);
            inputField.SetActive(true);
            alarmCircle.SetActive(false);
            setAlarmButton.GetComponentInChildren<Text>().text = "ON";
            Debug.Log("Будильник отменен.");
            alarmTimeText.text = ""; // Очищаем текст
        }
        else
        {
            buttonAnimation.Play("ToggleON");
            // Пытаемся распарсить время из поля ввода
            if (DateTime.TryParse($"{currentHour:D2}:{currentMinute:D2}", out alarmTime))
            {
                alarmSet = true; // Устанавливаем флаг будильника
                setAlarmButton.GetComponentInChildren<Text>().text = "OFF";
                alarmTimeText.text = alarmTime.ToString("HH") + "\n" + alarmTime.ToString("mm");

                Debug.Log("Будильник установлен на: " + alarmTime.ToString("HH:mm"));
            }
            else
            {
                Debug.LogError("Ошибка при установке будильника. Пожалуйста, введите корректное время в формате HH:mm.");
            }
        }
    }

    private void RingAlarm()
    {
        audioSource.Play(); // Проигрываем звук будильника
        ShowAlarmPanel(); // Показываем панель с кнопкой отключения
    }

    private void ShowAlarmPanel()
    {
        if (alarmPanel != null && !alarmPanel.activeSelf)
        {
            alarmPanel.SetActive(true); // Активируем alarmPanel
        }
        alarmCircle.SetActive(true);
        backButton.SetActive(false);
        inputField.SetActive(false);
        Debug.Log("Будильник прозвенел! Нажмите 'Отключить'.");
    }
    public void HideAlarmPanel()
    {
        alarmPanel.SetActive(false);
    }

    // Метод для обновления текущего времени, который можно вызывать из ClockHand
    public void UpdateCurrentTime(int hour, int minute)
    {
        currentHour = hour;
        currentMinute = minute;
        timeInputField.text = $"{currentHour:D2}:{currentMinute:D2}"; // Обновляем текстовое поле
    }
}
