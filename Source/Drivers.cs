using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
public class DriverRecord {public string Id {get;set;}public string Status {get;set;}public string Source {get;set;}public string Url {get;set;}public string[] HardwareIds;public string Device {get;set;}public string Class {get;set;}public string Vendor {get;set;}public string Version {get;set;}public string Date {get;set;}public string Inf {get;set;}public string Signed {get;set;}}
static class Drivers {
 public static string ResultLabel(int code){switch(code){case 2:return "Thành công";case 3:return "Hoàn tất một phần, có lỗi";case 4:return "Thất bại";case 5:return "Đã hủy";default:return "Trạng thái chưa hoàn tất: "+code;}}

 public static bool Administrator(){return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);}
 public static bool Allowed(string title){return !String.IsNullOrWhiteSpace(title)&&title.IndexOf("firmware",StringComparison.OrdinalIgnoreCase)<0&&title.IndexOf("BIOS",StringComparison.OrdinalIgnoreCase)<0;}
 public static List<DriverRecord> Inventory(){var result=new List<DriverRecord>();using(var query=new ManagementObjectSearcher("SELECT DeviceID,DeviceName,DeviceClass,Manufacturer,DriverVersion,DriverDate,InfName,IsSigned FROM Win32_PnPSignedDriver"))using(var records=query.Get())foreach(ManagementObject d in records){using(d){string raw=Convert.ToString(d["DriverDate"]),date="Chưa rõ";try{if(!String.IsNullOrEmpty(raw))date=ManagementDateTimeConverter.ToDateTime(raw).ToString("dd/MM/yyyy");}catch{}result.Add(new DriverRecord{Id=Convert.ToString(d["DeviceID"]),Device=String.IsNullOrWhiteSpace(Convert.ToString(d["DeviceName"]))?"Thiết bị "+Convert.ToString(d["DeviceID"]):Convert.ToString(d["DeviceName"]),Class=Convert.ToString(d["DeviceClass"]),Vendor=Convert.ToString(d["Manufacturer"]),Version=Convert.ToString(d["DriverVersion"]),Date=date,Inf=Convert.ToString(d["InfName"]),Signed=Convert.ToString(d["IsSigned"])=="True"?"Có":"Chưa xác nhận"});}}return result.OrderBy(x=>String.Equals(x.Class,"DISPLAY",StringComparison.OrdinalIgnoreCase)?0:1).ThenBy(x=>x.Class).ThenBy(x=>x.Device).ToList();}
}
