using Microsoft.Management.Infrastructure;

namespace SoftwareInventoryMMI.Classes
{
    internal class CimRegistry
    {
        const uint HKEY_CLASSES_ROOT = 2147483648;
        const uint HKEY_CURRENT_USERR = 2147483649;
        const uint HKEY_LOCAL_MACHINE = 2147483650;
        const uint HKEY_USERS = 2147483651;
        const uint HKEY_CURRENT_CONFIG = 2147483653;
        const string REGCLASS = "StdRegProv";
        const string REGNAMESPACE = @"root\default";
        private CimInstance RegClassInstance = new CimInstance(REGCLASS, REGNAMESPACE);
        private CimSession _session;
        private string _computerName;

        public CimRegistry(string ComputerName)
        {
            _computerName = ComputerName;
        }

        public void Connect()
        {
            _session = CimSession.Create(null);
        }

        public void Disconnect()
        {
            _session.Close();
        }

        public string[] EnumRegistryKey(string ParentKey)
        {

            CimMethodParametersCollection CimParams = new CimMethodParametersCollection();
            CimParams.Add(CimMethodParameter.Create("hDefKey", HKEY_LOCAL_MACHINE, CimFlags.In));
            CimParams.Add(CimMethodParameter.Create("sSubKeyName", ParentKey, CimFlags.In));

            CimMethodResult Result = _session.InvokeMethod(RegClassInstance, "EnumKey", CimParams);

            CimParams.Dispose();

            return (string[])Result.OutParameters["sNames"].Value;
        }

        public string GetStringValue(string ParentKey, string ValueName)
        {
            CimMethodParametersCollection CimParams = new CimMethodParametersCollection();
            CimParams.Add(CimMethodParameter.Create("hDefKey", HKEY_LOCAL_MACHINE, CimFlags.In));
            CimParams.Add(CimMethodParameter.Create("sSubKeyName", ParentKey, CimFlags.In));
            CimParams.Add(CimMethodParameter.Create("sValueName", ValueName, CimFlags.In));

            CimMethodResult Result = _session.InvokeMethod(RegClassInstance, "GetStringValue", CimParams);
            object ValueObject = Result.OutParameters["sValue"].Value;

            CimParams.Dispose();

            return ValueObject == null ? "" : ValueObject.ToString();
        }
    }
}
