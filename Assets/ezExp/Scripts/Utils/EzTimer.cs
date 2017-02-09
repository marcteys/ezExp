using System;

namespace UnityEzExp
{
    class EzTimer
    {
        public DateTime originalStartTime; // keep track on the same timer
        public DateTime startTime;
        public DateTime endTime;

        string outputFormat;

        public EzTimer(DateTime originalStartTime, string format = "s.ffff")
        {
        //    startTime = DateTime.Now.ToString("hh.mm.ss.ffffff");
            outputFormat = format;
        }

        public void Start()
        {
            startTime = DateTime.Now; // it actually reset the time
        }

        public string Stop()
        {
            return GetTime();
        }

        public string GetTime(string format = null) // ++ format   
        {
            DateTime now = DateTime.Now;
            if (format != null)
            {
                // formater avec le nv format
            }
            else
            {
                //formater avec le format defautl
            }

            //TODO : destroy after it sent
            return "lol";
        }

    }
}