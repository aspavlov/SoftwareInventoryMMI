using SoftwareInventoryMMI.Classes;

namespace SoftwareInventoryMMI
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var Scaner = new SoftwareScaner(null);
            var InstalledSoftware = Scaner.GetInstalledSoftware();

            foreach (SoftwareEntry Item in InstalledSoftware)
            {
                Item.Print();
            }
        }
    }
}