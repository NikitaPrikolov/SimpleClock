using UnityEngine;

public class ClockHand : MonoBehaviour
{
    public AlarmManager alarmManager; // Ссылка на AlarmManager
    public bool isHourHand; // Является ли стрелка часовой

    private Camera mainCamera; // Ссылка на основную камеру
    private Vector3 screenPosition; // Положение стрелки в экранных координатах
    private float angleOffset; // Смещение угла при нажатии
    private Collider2D collider; // Коллайдер стрелки

    private bool isDragging; // Указывает, идет ли в данный момент перетаскивание стрелки

    private void Start()
    {
        mainCamera = Camera.main; // Получаем ссылку на основную камеру
        collider = GetComponent<Collider2D>(); // Получаем коллайдер стрелки
    }

    private void Update()
    {
        // Обрабатываем нажатия кнопки
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            Vector3 inputPosition = (Input.touchCount > 0) ? Input.GetTouch(0).position : Input.mousePosition;
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(inputPosition.x, inputPosition.y, 10)); // Z=10 для правильной конвертации

            if (collider == Physics2D.OverlapPoint(worldPosition))
            {
                screenPosition = mainCamera.WorldToScreenPoint(transform.position);
                Vector3 inputVector = inputPosition - screenPosition;
                angleOffset = (Mathf.Atan2(transform.right.y, transform.right.x) - Mathf.Atan2(inputVector.y, inputVector.x)) * Mathf.Rad2Deg;
                isDragging = true; // Устанавливаем флаг перетаскивания
            }
        }

        // Обрабатываем движение мыши или касание
        if (isDragging && (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)))
        {
            Vector3 inputPosition = (Input.touchCount > 0) ? Input.GetTouch(0).position : Input.mousePosition;
            Vector3 inputVector = inputPosition - screenPosition;
            float angle = Mathf.Atan2(inputVector.y, inputVector.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, 0, angle + angleOffset);
            UpdateTime(); // Обновляем время в timeInputField по углу поворота
        }

        // Обработка завершения перетаскивания
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false; // Сбрасываем флаг перетаскивания
        }
    }

    private void UpdateTime()
    {
        float angle = transform.rotation.eulerAngles.z;

        // Сохраняем значение текущих часов и минут, если они были установлены
        if (isHourHand)
        {
            // Обновляем текущее значение часов
            int newHour = (24 - Mathf.FloorToInt(angle / 15)) % 24;
            alarmManager.currentHour = newHour; // Сохраняем в AlarmManager
        }
        else
        {
            // Обновляем текущее значение минут
            int newMinute = (60 - Mathf.FloorToInt(angle / 6)) % 60;
            alarmManager.currentMinute = newMinute; // Сохраняем в AlarmManager
        }

        // Обновляем поле ввода времени
        UpdateTimeInputField();
    }

    private void UpdateTimeInputField()
    {
        // Форматируем время и обновляем текстовое поле
        string timeString = $"{alarmManager.currentHour:00}:{alarmManager.currentMinute:00}";
        alarmManager.timeInputField.text = timeString;
    }
}
