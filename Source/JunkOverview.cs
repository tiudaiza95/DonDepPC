using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
class JunkItem {
 public bool Selected {get;set;}public string Category {get;set;}public string Path {get;set;}public string Reason {get;set;}public string Outcome {get;set;}public long Bytes;
 public bool Recommended;public FileItem File;public AppEntry Owner;public string OwnerRoot;
 public string SizeLabel {get{return Features.Size(Bytes);}}public bool CanClean {get{return File!=null&&Outcome!="Đã xử lý";}}
}
class JunkReport {public List<JunkItem> Items=new List<JunkItem>();public List<string> Notes=new List<string>();public MachineScanResult Machine;public int Roots;}
static class JunkOverview {
 public static JunkReport Scan(bool whole,CancellationToken token,IProgress<string> progress){var report=new JunkReport();if(whole){report.Machine=WholeMachine.Scan(WholeMachine.LocalDrives(),token,progress);token.ThrowIfCancellationRequested();}foreach(var root in AnalysisTools.Roots()){token.ThrowIfCancellationRequested();if(!Directory.Exists(root.Path))continue;report.Roots++;progress.Report("Đang nhận diện rác: "+root.Name+"\n"+root.Path);try{var result=AnalysisTools.Cleanup(root,token,null);report.Notes.Add(root.Name+": "+result.Items.Count+" tệp, bỏ qua "+result.Skipped+(result.Limited?" · kết quả một phần":""));foreach(var f in result.Items)report.Items.Add(new JunkItem{Category=root.Name,Path=f.Path,Bytes=f.Size,File=f,Recommended=true,Selected=true,Reason="Tệp tạm/cache hoặc báo cáo lỗi trong vị trí xác định; tạo và sửa hơn 7 ngày. Không phải tệp lỗi cần sửa chữa."});}catch(OperationCanceledException){throw;}catch(Exception ex){report.Notes.Add(root.Name+": "+ex.Message);}}
  foreach(var app in Engine.RemovalHistory()){token.ThrowIfCancellationRequested();if(Engine.Present(app))continue;progress.Report("Đang đối chiếu phần dư đã gỡ: "+app.Name);try{foreach(var remnant in Engine.Scan(app)){token.ThrowIfCancellationRequested();if(remnant.IsRegistry||remnant.IsShortcut||FileActions.ProtectedPath(remnant.Path)){report.Notes.Add(app.Name+": xem riêng tại Quét chuyên sâu · "+remnant.DisplayPath);continue;}int skipped;bool limited;var files=AnalysisTools.Walk(remnant.Path,token,null,out skipped,out limited);foreach(var f in files){if(FileActions.ProtectedPath(f.Path))continue;report.Items.Add(new JunkItem{Category="Phần dư cần xem · "+app.Name,Path=f.Path,Bytes=f.Size,File=f,Owner=app,OwnerRoot=remnant.Path,Recommended=false,Selected=false,Reason="Ứng dụng không còn đăng ký; khớp thư mục đã ghi nhận. Có thể chứa cấu hình/tài liệu: chỉ chọn khi không còn cần."});}if(skipped>0||limited)report.Notes.Add(app.Name+": phần dư quét chưa đầy đủ");}}catch(OperationCanceledException){throw;}catch(Exception ex){report.Notes.Add(app.Name+": "+ex.Message);}}
  report.Items=report.Items.GroupBy(x=>x.Path,StringComparer.OrdinalIgnoreCase).Select(g=>g.OrderByDescending(x=>x.Recommended).First()).OrderByDescending(x=>x.Bytes).ToList();return report;
 }
 public static void Clean(JunkItem item){if(!item.CanClean)throw new Exception("Mục chỉ xem.");if(item.Owner==null){AnalysisTools.Clean(item.File);return;}if(Engine.Present(item.Owner)||!Engine.SafeFolder(item.OwnerRoot,item.Owner,Engine.Apps())||!Engine.Within(item.Path,item.OwnerRoot))throw new Exception("Phần dư không còn đáp ứng bằng chứng lúc quét; đã giữ lại.");FileActions.Recycle(item.File);}
}
