using System;using System.Diagnostics;using System.Threading;using System.Threading.Tasks;
// Legacy CLI is disabled. Retain only the process-wait test helper; no network/update calls.
static class DriverSearchJob {
 public static int Worker(string folder){return 2;}
 public static async Task Wait(Process proc,CancellationToken token,IProgress<string> progress,TimeSpan timeout){var watch=Stopwatch.StartNew();try{while(!proc.HasExited){token.ThrowIfCancellationRequested();if(watch.Elapsed>timeout)throw new TimeoutException();await Task.Delay(250,token);}}finally{if(!proc.HasExited){proc.Kill();proc.WaitForExit(2000);}}}
}
