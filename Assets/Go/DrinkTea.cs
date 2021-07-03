/*⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵
☠ ©2020 Chengdu Mighty Vertex Games. All rights reserved.                                                                        
⚓ Author: SkyAllen                                                                                                                  
⚓ Email: 894982165@qq.com                                                                                  
⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵⛵*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Windows.Forms;
/*NotifyIcon fyIcon = new NotifyIcon();
fyIcon.Icon = new Icon("nihao.ico");
fyIcon.BalloonTipText = "Hello World！";
fyIcon.Visible = true;
fyIcon.ShowBalloonTip(0);*/
using System.Drawing;
using System.Runtime.InteropServices;
using UnityEngine.UI;


public class DrinkTea : MonoBehaviour
{
    public Slider rest;
    public Slider work;

    private GlobalInput GlobalInput;

    private int rest_timer = 0;
    private int restTimeTarget = TimeHelper.Minute * 5;
    private int work_timerTarget = TimeHelper.Minute * 25;
    private bool isTest=false;
    private double LastTime = 0;

    private OtherHelper _otherHelper;
    private void Start()
    {
        GlobalInput = GetComponent<GlobalInput>();
        GlobalInput.Init();

        _otherHelper = GetComponent<OtherHelper>();
        _otherHelper.Init();
   
        
        var restText = rest.transform.Find("Text2").GetComponent<Text>();
        var workText = work.transform.Find("Text2").GetComponent<Text>();
        
        rest.onValueChanged.AddListener(val =>
        {
            var intVal = (int) val;
            restText.text = $"休息时长 ：{intVal}分钟";
            restTimeTarget = TimeHelper.Minute * intVal;
        });
        rest.value = rest.minValue;

        work.onValueChanged.AddListener(val =>
        {
            var intVal = (int) val;
            workText.text = $"工作时长 ：{intVal}分钟";
            work_timerTarget = TimeHelper.Minute * intVal;
        });
        work.value = work.minValue;

        if (isTest)
        {
            restTimeTarget = 5;
            work_timerTarget = 10;
        }

        LastTime = TimeHelper.GetNowGreenwichAlterOfDoubleTimeStamp();
        InvokeRepeating(nameof(Delay), 1, 1);
    }

    private void Delay()
    {
        rest_timer += 1;

        if (rest_timer >= restTimeTarget)
        {
            LastTime = TimeHelper.GetNowGreenwichAlterOfDoubleTimeStamp();
        }

        var now =      TimeHelper.GetNowGreenwichAlterOfDoubleTimeStamp();
        var shengyu = (now - LastTime - work_timerTarget);

        if (shengyu>=0)
        {
            int m = work_timerTarget / TimeHelper.Minute;
            int m2 = restTimeTarget / TimeHelper.Minute;
           // print("show");
            _otherHelper.Show($"提醒 ：您连续操作键盘已有{m}分钟了，起来走动走动，休息{m2}分钟吧！");
            LastTime = TimeHelper.GetNowGreenwichAlterOfDoubleTimeStamp();
        }

       // Debug.Log(rest_timer +":"+(int)shengyu+":"+(int)LastTime);
    }

    private void Update()
    {
        if (IsClick())
        {
            rest_timer = 0;
        }

        /*if (Input.GetKeyDown(KeyCode.A))
        {
            if (isTest)
            {
                restTimeTarget = 10;
                work_timerTarget = 20;
            }
        }*/
    }

    private bool IsClick()
    {
        bool res = false;
        for (int i = 65; i <= 90; i++)
        {
            res = res || GlobalInput.GetKeyDown(GetEnumByInt<GlobalKeyCode>(i));
        }

        return res;
    }

    private static T GetEnumByInt<T>(int val) where T : Enum
    {
        return (T) Enum.Parse(typeof(T), val.ToString());
    }
}

public class Messagebox
{
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, String message, String title, int type);
}


public class TimeHelper
{
    public const int Second = 1;
    public const int Minute = Second * 60;
    public const int Hour = Minute * 60;
    public const int Day = Hour * 24;
    public const int Week = Day * 7;
    public const string DateFormat = "yyyy_MMdd_HHmm";
    public static readonly DateTime Greenwich = CreateDateTime(1970, 1, 1);
    public static readonly DateTime GreenwichAlter = CreateDateTime(2021, 6, 21);

    public static double GetNowGreenwichTimeStamp(TimeStampTypes timeStampTypes = TimeStampTypes.Second,
        DateTime algo = default)
    {
        if (algo == default)
        {
            algo = Greenwich;
        }

        double res = 0f;
        var ts = DateTime.Now - algo;
        switch (timeStampTypes)
        {
            case TimeStampTypes.Second:
                res = ts.TotalSeconds;
                break;
            case TimeStampTypes.Minute:
                res = ts.TotalMinutes;
                break;
            case TimeStampTypes.Hour:
                res = ts.TotalHours;
                break;
            case TimeStampTypes.Day:
                res = ts.TotalDays;
                break;
        }

        return res;
    }

    public static DateTime CreateDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
    {
        return new DateTime(year, month, day, hour, minute, second);
    }

    public static int GetNowGreenwichOfIntTimeStamp()
    {
        return (int) GetNowGreenwichTimeStamp(TimeStampTypes.Second);
    }

    public static double GetNowGreenwichAlterOfDoubleTimeStamp()
    {
        return GetNowGreenwichTimeStamp(algo: GreenwichAlter);
    }

    public static string GetTimeSpanFormat(int f)
    {
        var d = new TimeSpan(0, 0, f);
        return d.ToString();
    }

    public static bool IsBelongTimeQuantumBig(int lMonth, int lDay, int rMontn, int rDay, int addYear = 0)
    {
        var now = DateTime.Now;
        var year = now.Year;
        DateTime l = CreateDateTime(year, lMonth, lDay);
        DateTime r = CreateDateTime(year + addYear, rMontn, rDay);

        return (now > l) && (now < r);
    }

    public static bool IsBelongWeek(DayOfWeek week)
    {
        var now = DateTime.Now;
        return now.DayOfWeek == week;
    }

    public static DateTime CreateDateTimeLite(int hour)
    {
        var now = DateTime.Now;
        var year = now.Year;
        var month = now.Month;
        var day = now.Day;
        return CreateDateTime(year, month, day, hour);
    }

    public static bool IsBelongTimeQuantumSmall(int lHour, int rHour)
    {
        var now =DateTime.Now;
        DateTime l = CreateDateTimeLite(lHour);
        DateTime r = CreateDateTimeLite(rHour);

        return (now > l) && (now < r);
    }
}

public enum TimeStampTypes
{
    Second,
    Minute,
    Hour,
    Day
}