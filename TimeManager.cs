using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Open();
public delegate void Close();
public class TimeManager : MonoBehaviour
{
    struct Date
    {
        public int year;
        public int month;
        public int day;
        public Date(int y, int m, int d)
        {
            year = y;
            month = m;
            day = d;
        }
    }
    public static TimeManager manager;
    private void Awake()
    {
        manager = this;
    }

    public event Open OnOpen;
    public event Close OnClose;

    [Header("����Ӫҵʱ��")]
    public int openTime ;
    [Header("����Ъҵʱ��")]
    public int closeTime ;

    public float timeMultiple;
    Date date = new(2022,1,1);//��ʼ����
    int hour=9;
    int mins=1;
    float s=0;

    private void FixedUpdate()
    {
        s += Time.fixedDeltaTime;
        if (s > 60f / timeMultiple)
        {
            mins++;
            SetTime();
            s = 0;
        }
        if (mins > 59)
        {
            hour++;
            mins = 1;
            if (hour == openTime)
            {
                //����Ӫҵ
                OnOpen.Invoke();
            }
            if (hour == closeTime)
            {
                //�°�
                OnClose.Invoke();
            }
        }
        if (hour > 23)
        {
            date.day++;
            hour = 0;
        }
        if (date.day > 30)
        {
            date.month++;
            date.day = 1;
        }
        if (date.month > 12)
        {
            date.year++;
            date.month= 1;
        }
    }

    public void SetTime()
    {
        UIManager.manager.dateData.text = date.year + "." + date.month + "." + date.day + " " + hour + ":" + mins;
    }
}
