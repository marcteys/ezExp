using System;

namespace UnityEzExp
{
    class EzTimer
    {
        public TimeSpan _originalStartTime; // keep track on the same timer
        public double startTime;
        public double endTime;

        public EzTimer(TimeSpan originalStartTime)
        {
            Start();
            _originalStartTime = originalStartTime;
        }

        public void Start()
        {
            startTime = _originalStartTime.TotalMilliseconds; 
        }

        public string Stop()
        {
            return GetTime();
        }

        public float GetTimeSeconds()
        {
            endTime = _originalStartTime.TotalMilliseconds;

            TimeSpan total = TimeSpan.FromMilliseconds(endTime - startTime);

            return (float)total.TotalSeconds;
        }

        public string GetTime(TimeFormat format = TimeFormat.MINUTES)
        {
            endTime = _originalStartTime.TotalMilliseconds;

            TimeSpan total = TimeSpan.FromMilliseconds(endTime - startTime);
            string formatedValue = "";
            switch (format) // TODO  : Do something with format
            {
                case TimeFormat.MILLISECONDS:
                    formatedValue = total.TotalMilliseconds.ToString(); ;
                    break;
                case TimeFormat.SECONDS:
                    if(total.Minutes > 0)
                        formatedValue = total.Minutes + ":" + total.Seconds + "." + total.Milliseconds;
                    else
                        formatedValue = total.Seconds + "." + total.Milliseconds;
                    break;
                case TimeFormat.MINUTES:
                    formatedValue = total.Minutes  +  ":" + total.Seconds + "." + total.Milliseconds;
                    break;
            }
            return formatedValue;
        }

    }
}