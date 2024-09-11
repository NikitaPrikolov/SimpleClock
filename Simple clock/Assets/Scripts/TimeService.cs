using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public class TimeService : MonoBehaviour
{
    private const string API_URL_1 = "https://timeapi.io/api/time/current/coordinate?latitude=55.70262751524248&longitude=37.729988980432516";
    private const string API_URL_2 = "https://api.timezonedb.com/v2.1/get-time-zone?key=YOUTJ3MRSK6Z&format=json&by=position&lat=55.70262751524248&lng=37.729988980432516";
    public GameObject startPanel;

    private DateTime currentTime;
    public ClockManager clockManager;

    private void Start()
    {
        StartCoroutine(GetRealTime());
        StartCoroutine(SyncTimeEveryHour());
    }

    public string GetCurrentTime()
    {
        return currentTime.ToString("HH:mm:ss");
    }

    private IEnumerator SyncTimeEveryHour()
    {
        while (true)
        {
            yield return new WaitForSeconds(3600);
            NotifyClockManager();
        }
    }

    private IEnumerator GetRealTime()
    {
        bool isSuccess = false;

        yield return StartCoroutine(GetTimeFromAPI(API_URL_1, result =>
        {
            isSuccess = result;
            if (result) Debug.Log("Время получено из API: " + API_URL_1);
        }));

        if (!isSuccess)
        {
            yield return StartCoroutine(GetTimeFromAPI(API_URL_2, result =>
            {
                isSuccess = result;
                if (result) Debug.Log("Время получено из API: " + API_URL_2);
            }));
        }

        if (isSuccess)
        {
            NotifyClockManager();
            Debug.Log("Время успешно установлено: " + currentTime);
            startPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Не удалось установить время!");
        }
    }

    private IEnumerator GetTimeFromAPI(string apiUrl, Action<bool> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Ошибка получения времени из {apiUrl}: {webRequest.error}");
                callback(false);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                DateTime parsedTime = ParseTime(jsonResponse, apiUrl);

                if (parsedTime != default)
                {
                    currentTime = parsedTime;
                    callback(true);
                }
                else
                {
                    callback(false);
                }
            }
        }
    }

    private DateTime ParseTime(string jsonResponse, string apiUrl)
    {
        if (apiUrl == API_URL_1)
        {
            string dateTimeString = Regex.Match(jsonResponse, @"""dateTime"":\s*""(.*?)""").Groups[1].Value;
            DateTime parsedTime;
            if (DateTime.TryParse(dateTimeString, out parsedTime))
            {
                return parsedTime;
            }
        }
        else if (apiUrl == API_URL_2)
        {
            string formattedTime = Regex.Match(jsonResponse, @"""formatted"": ""(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2})""").Groups[1].Value;
            DateTime parsedTime;
            if (DateTime.TryParseExact(formattedTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out parsedTime))
            {
                return parsedTime;
            }
        }
        return default;
    }

    private void NotifyClockManager()
    {
        if (clockManager != null)
        {
            clockManager.UpdateTime(currentTime.ToString("HH:mm:ss"));
        }
    }
}
