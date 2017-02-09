using System;
using System.Diagnostics;
using System.Threading;

namespace UnityEzExp
{
    class EzTimer
    {
        //TODO : creer un timermanager capable de lancer / arreter plusieurs timers 
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
            return GetTime();
        }

        public string GetTime(string format = null) // ++ format   
        {
            if (format != null)
            {
                // formater avec le nv format
            }
            else
            {
                //formater avec le format defautl
            }

            //TODO : destroy after it sent
            return timer.Elapsed.ToString();
        }

    }
}