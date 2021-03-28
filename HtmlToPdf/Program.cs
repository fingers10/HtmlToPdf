using CliWrap;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;

Console.WriteLine("Approach - 1: HtmlToPdf with Selenium WebDriver.");

// Url can be webpage url for physical html file path.
var url = "https://www.google.com";

var driverOptions = new ChromeOptions();
// In headless mode, PDF writing is enabled by default (tested with driver major version 85)
driverOptions.AddArgument("headless");
using var driver = new ChromeDriver(driverOptions);
driver.Navigate().GoToUrl(url);

// Output a PDF of the first page in A4 size at 90% scale
var printOptions = new Dictionary<string, object>
{
    { "paperWidth", 210 / 25.4 },
    { "paperHeight", 297 / 25.4 },
    { "scale", 0.9 },
    { "pageRanges", "1" }
};
var printOutput = driver.ExecuteChromeCommandWithResult("Page.printToPDF", printOptions) as Dictionary<string, object>;
var pdf = Convert.FromBase64String(printOutput["data"] as string);

var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "HtmlToPdf");
Directory.CreateDirectory(directory);
var filePath = Path.Combine(directory, "SeleniumWebDriverHtmlToPdf.pdf");
await File.WriteAllBytesAsync(filePath, pdf);

//==========================================================================

Console.WriteLine("Approach - 2: HtmlToPdf with Chrome headless");

var chromeCmd = Cli.Wrap("C:/Program Files (x86)/Google/Chrome/Application/chrome.exe")
             .WithArguments(@$"/C --headless --disable-gpu --run-all-compositor-stages-before-draw --print-to-pdf-no-header --print-to-pdf=""C:/Users/Abdul Rahman/Desktop/test.pdf"" ""{url}""");

var chromeResult = await chromeCmd.ExecuteAsync();

//==========================================================================

Console.WriteLine("Approach - 3: HtmlToPdf with wkhtmltopdf");

var wkhtmltopdfCmd = Cli.Wrap("cmd.exe")
             .WithArguments(@$"/C echo | set /p=""<h3>blep</h3>"" | ""C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"" -s A4 - ""C:\Users\Abdul Rahman\Desktop\test.pdf""");
//or
//WithArguments(@$"/C ""C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"" -s A4 - {url} ""C:\Users\Abdul Rahman\Desktop\test.pdf""");

var wkhtmltopdfResult = await wkhtmltopdfCmd.ExecuteAsync();

//==========================================================================

Console.ReadLine();