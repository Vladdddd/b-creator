using Newtonsoft.Json;
using BarcodeCreator;
using Spectre.Console;
using System.Runtime.Versioning;

public class LabelData
{
    public string supCode { get; set; } = string.Empty;
    public string prodCode { get; set; } = string.Empty;
    public string labelNumber { get; set; } = string.Empty;
    public string lotNumber { get; set; } = string.Empty;
    public string quantity { get; set; } = string.Empty;
    public string date { get; set; } = string.Empty;
    public string companySenderName { get; set; } = string.Empty;
    public string senderAddress { get; set; } = string.Empty;
    public string reg { get; set; } = string.Empty;
    public string senderStreet { get; set; } = string.Empty;
    public string senderCity { get; set; } = string.Empty;
    public string companyReceiverName { get; set; } = string.Empty;
    public string receiverAddress { get; set; } = string.Empty;
    public string receiverStreet { get; set; } = string.Empty;
    public string receiverCity { get; set; } = string.Empty;
    public string tradeName { get; set; } = string.Empty;   
}

public class Data
{
    public LabelData[] data { get; set; }
}

[SupportedOSPlatform("windows")]
internal class Program
{
    public const string defaultPathIn = "C:\\Users\\User\\source\\repos\\BarcodeCreator\\BarcodeCreator\\data.json";
    public const string defaultPathOut = "C:\\Users\\User\\Desktop\\barcodes\\";

    static void Main(string[] args)
    {
        //Data mockData = new Data();

        AnsiConsole.Write(new FigletText("B-Creator").Color(Color.Blue));
        AnsiConsole.Write(new Markup("This program creates a product label " +
            "with six 1D barcodes and one 2D barcode.\n" +
            "The 2D barcode data consists of six 1D barcode data. \n\n"));

        while(true)
        {
            try
            {
                AnsiConsole.Write(new Markup("\n[bold gold1]START QUESTIONS[/]\n"));
                var answerStartSearching = AnsiConsole.Confirm("Start creating [blue]labels[/]?");

                if (!answerStartSearching)
                {
                    return;
                }

                var useDefaultSettings = AnsiConsole.Confirm("Do you want to use [blue]default settings[/]?");
                AnsiConsole.Write(new Markup("\n"));

                AnsiConsole.Write(new Markup("[bold gold1]SETTINGS[/]\n"));
                AnsiConsole.Write(new Markup(useDefaultSettings ? "Default settings\n" : ""));

                var pathIn = !useDefaultSettings ? AnsiConsole.Prompt(
                    new TextPrompt<string>("Write down the path to the [blue]barcode data file[/]?")
                        .PromptStyle("green")
                ) : defaultPathIn;

                var pathOut = !useDefaultSettings ? AnsiConsole.Prompt(
                    new TextPrompt<string>("Write down the path where you want to save [blue]labels[/]?")
                        .PromptStyle("green")
                ) : defaultPathOut;

                var json = File.ReadAllText(pathIn);
                Data? docs = JsonConvert.DeserializeObject<Data>(json);
                int dataLength = docs.data.Length;

                AnsiConsole.Status()
                    .Start("Loading...", ctx =>
                    {
                        for (int i = 0; i < dataLength; i++)
                        {
                            var data = new LabelImage(
                                pathOut + docs.data[i].tradeName + ".png",
                                docs.data[i].supCode,
                                docs.data[i].prodCode,
                                docs.data[i].labelNumber,
                                docs.data[i].lotNumber,
                                docs.data[i].quantity,
                                docs.data[i].date,
                                docs.data[i].companySenderName,
                                docs.data[i].companyReceiverName,
                                docs.data[i].senderAddress,
                                docs.data[i].reg,
                                docs.data[i].senderStreet,
                                docs.data[i].senderCity,
                                docs.data[i].receiverAddress,
                                docs.data[i].receiverStreet,
                                docs.data[i].receiverCity,
                                docs.data[i].tradeName
                            );

                            data.CreateLabel();
                        }
                    }
                );

                AnsiConsole.Write(new Markup("\n[bold gold1]RESULTS[/]\n"));

                var table = new Table();

                string tableTitle = "Your labels folder path: " + pathOut;
                table.Title = new TableTitle(tableTitle);
                table.Width(tableTitle.Length);

                table.AddColumn("№");
                table.AddColumn("Label Name");

                for (int i = 0; i < dataLength; i++)
                {
                    table.AddRow((i + 1).ToString(), docs.data[i].tradeName + ".png");
                }

                AnsiConsole.Write(table);
            }
            catch (Exception)
            {
                AnsiConsole.Write(new Markup("[bold red]Something went wrong!!![/]\n"));
            }
        }
    }
}