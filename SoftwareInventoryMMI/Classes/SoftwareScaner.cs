using Microsoft.Management.Infrastructure;
using System.Reflection;

namespace SoftwareInventoryMMI.Classes
{
    internal class SoftwareScaner
    {
        string ComputerName { get; }

        public SoftwareScaner (string ComputerName)
        {
            this.ComputerName = ComputerName;
        }

        public List<SoftwareEntry> GetInstalledSoftware()
        {
            string[] SoftwareEntryNames = { "DisplayName", "DisplayVersion", "Publisher", "InstallDate" };
            List<string> SoftwareRegKeys = new List<string>();
            List<SoftwareEntry> InstalledSoftware = new List<SoftwareEntry>();
            Type softwareEntryType = typeof(SoftwareEntry);

            CimRegistry RemoteRegistry = new CimRegistry(ComputerName);
            RemoteRegistry.Connect();

            string[] InstalledSoftwareRootKey =
            {
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall",
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
            };

            foreach (string RootKey in InstalledSoftwareRootKey)
            {
                Array.ForEach(RemoteRegistry.EnumRegistryKey(RootKey), 
                    i => SoftwareRegKeys.Add(RootKey + "\\" + i));
            }


            foreach (var RegKey in SoftwareRegKeys)
            {
                SoftwareEntry softwareEntryInstance = new SoftwareEntry();

                foreach (string ValueName in SoftwareEntryNames)
                {
                    string Value = RemoteRegistry.GetStringValue(RegKey,ValueName);

                    PropertyInfo seProperty = softwareEntryType.GetProperty(ValueName);
                    seProperty.SetValue(softwareEntryInstance, Value);

                }

                InstalledSoftware.Add(softwareEntryInstance);

            }

            RemoteRegistry.Disconnect();

            return InstalledSoftware.Distinct().Where(s => s.DisplayName != "").ToList();

        }
    }
}
