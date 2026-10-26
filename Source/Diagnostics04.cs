using System;
using System.IO;
using System.Linq;
using System.Threading;
static class Diagnostics04 {
 public static void Run(){var drivers=Drivers.Inventory();var deep=DeepClean.Scan(CancellationToken.None,null);var apps=Engine.Apps();var lines=new[]{"Read-only diagnostics; no driver download/install, cleanup or uninstall executed.","Installed entries: "+apps.Count,"Driver records: "+drivers.Count,"DISPLAY driver records: "+drivers.Count(x=>String.Equals(x.Class,"DISPLAY",StringComparison.OrdinalIgnoreCase)),"Deep scan findings: "+deep.Count,"AutoCAD approved product states:"}.Concat(new[]{"{28B89EEF-8155-0409-0100-CF3F3A09B77D}","{28B89EEF-8101-0409-2102-CF3F3A09B77D}","{28B89EEF-8101-0000-3102-CF3F3A09B77D}"}.Select(x=>x+" = "+MsiSupport.MsiQueryProductState(x))).Concat(new[]{"Shared Autodesk MSI state: "+MsiSupport.MsiQueryProductState("{9DA04284-B9CA-344E-8D9E-85DA1467CFD9}")}).Concat(drivers.Where(x=>String.Equals(x.Class,"DISPLAY",StringComparison.OrdinalIgnoreCase)).Select(x=>"GPU: "+x.Device+" | "+x.Version));File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"diagnostics-0.4.txt"),lines);}
}
