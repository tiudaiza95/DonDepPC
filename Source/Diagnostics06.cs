using System;
using System.IO;
using System.Linq;
using System.Threading;
class QuietProgress:IProgress<string>{public void Report(string text){}}
static class Diagnostics06 {
 public static void Run(){Engine.Testing=true;var report=JunkOverview.Scan(false,CancellationToken.None,new QuietProgress());var drivers=VendorDrivers.Scan();string text="Read-only audit. No cleanup, download, driver installation or OS update.\nCleanup roots: "+report.Roots+"\nCandidate files: "+report.Items.Count+"\nRecommended files: "+report.Items.Count(x=>x.Recommended)+"\nCandidate bytes: "+report.Items.Sum(x=>x.Bytes)+"\nRemnants requiring manual review: "+report.Items.Count(x=>!x.Recommended)+"\nDriver/device records: "+drivers.Count+"\nBoard: "+VendorDrivers.Board+"\n\n"+String.Join("\n",report.Notes)+"\n\n"+String.Join("\n",drivers.Where(x=>x.Class=="DISPLAY"||(x.Device??"").Contains("Realtek")).Select(x=>x.Device+" | "+x.Version+" | "+x.Status+" | "+x.Source));File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"diagnostics-0.6.txt"),text);}
}
