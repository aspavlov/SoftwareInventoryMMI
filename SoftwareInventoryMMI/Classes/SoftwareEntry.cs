namespace SoftwareInventoryMMI.Classes
{
    internal class SoftwareEntry
    {
        public string ComputerName { get; set; }
        public string DisplayName { get; set; }
        public string DisplayVersion { get; set; }
        public string InstallDate { get; set; }
        public string Publisher { get; set; }

        public void Print ()
        {
            Console.WriteLine($"{ComputerName}\t{DisplayName}\t{DisplayVersion}\t{Publisher}\t{InstallDate}");
        }
    }
}
