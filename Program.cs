using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

// 取得目前目錄位址
var currentDirectory = Directory.GetCurrentDirectory();   

// 設定目標目錄位址
var storesDir = Path.Combine(currentDirectory, "stores");

// 設定新增目錄位址
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
// 新增新目錄
Directory.CreateDirectory(salesTotalDir);            

// 取得目標檔案路徑(array)
var salesFiles = FindFiles(storesDir);

// 讀取檔案並解析
var salesTotal = CalculateSalesTotal(salesFiles);

// 寫入totals.txt檔案
File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal:#0.00}{Environment.NewLine}");


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        // The file name will contain the full path, so only check the end of it
        if (file.EndsWith("sales.json"))
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // READ FILES LOOP
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

record SalesData(double Total);
