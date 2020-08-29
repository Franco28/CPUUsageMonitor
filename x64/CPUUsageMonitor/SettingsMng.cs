
using System;
using System.IO;
using System.Windows.Forms;

namespace CPUUsageMonitor
{
    public class SettingsMng
    {
        public string m_sConfigFileName = Path.GetFileNameWithoutExtension(System.Windows.Forms.Application.ExecutablePath) + ".xml";
        public Settings m_oConfig = new Settings();

        public Settings Config
        {
            get { return m_oConfig; }
            set { m_oConfig = value; }
        }

        public void LoadConfig()
        {
            try
            {
                var settingspath = "C:\\MotoBootLogoMaker\\";
                if (File.Exists(settingspath + m_sConfigFileName))
                {
                    StreamReader srReader = File.OpenText(settingspath + m_sConfigFileName);
                    Type tType = m_oConfig.GetType();
                    System.Xml.Serialization.XmlSerializer xsSerializer = new System.Xml.Serialization.XmlSerializer(tType);
                    object oData = xsSerializer.Deserialize(srReader);
                    m_oConfig = (Settings)oData;
                    srReader.Close();
                }

            }
            catch (Exception er)
            {
                MessageBox.Show("" + er, "Error Loading Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
