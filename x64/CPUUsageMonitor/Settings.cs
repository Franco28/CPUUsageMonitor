
using System;

namespace CPUUsageMonitor
{
    [Serializable()]
    public class Settings
    {

        private System.String m_oToolTheme;
      
        public System.String ToolTheme
        {
            get { return m_oToolTheme; }
            set { m_oToolTheme = value; }
        } 
    }
}
