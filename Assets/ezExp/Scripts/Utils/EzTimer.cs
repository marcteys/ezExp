using System;
using System.Diagnostics;
using System.Threading;

class EzTimer
{

    Stopwatch timer = null;
    string outputFormat = "";

    EzTimer(bool autoStart = true, string format = "")
    {
        timer = new Stopwatch();

        outputFormat = format;
        if (autoStart) Start();
    }

    
    public void Start()
    {
        timer.Start();
    }

    public string Stop()
    {
        timer.Stop();
        return timer.ToString();
    }

    public string GetTime(string format) // ++ format   
    {

        return "";
    }


}
