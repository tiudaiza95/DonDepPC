using System;
using System.Text;
using System.Threading;
class ProcessFixture {
 static int Main(string[] args){
  if(args.Length>0&&args[0]=="--fixture-wait"){Thread.Sleep(30000);return 0;}
  Console.OutputEncoding=Encoding.UTF8;
  Console.WriteLine("fixture-output thành công");
  Console.Error.WriteLine("fixture-error");
  return 7;
 }
}
