using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;
using System.ComponentModel;
using System.Drawing;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

Document.Create(container =>
{
    container.Page(page =>
    {
        /*page.Header().Height(100).Background(Colors.Blue.Medium);
        page.Content().Background(Colors.Grey.Medium);
        page.Footer().Height(100).Background(Colors.LightBlue.Medium);*/

        page.Header().Row(row =>
        {
            row.RelativeItem().Border(1).Background(Colors.Blue.Medium).Height(100);
            row.RelativeItem().Border(1).Background(Colors.Green.Medium).Height(100);
            row.RelativeItem().Border(1).Background(Colors.Brown.Medium).Height(100);
            row.RelativeItem().Border(1).Background(Colors.Brown.Medium).Height(100);
        }); //to make rows

        page.Content().PaddingVertical(10).Column(col =>
        {
            col.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });//columns headers

                table.Header(header =>
                {
                    header.Cell().Background("#257272").AlignCenter().Text("UserName").FontColor("#fff");
                    header.Cell().Background("#257272").AlignCenter().Text("Name").FontColor("#fff");
                    header.Cell().Background("#257272").AlignCenter().Text("Email").FontColor("#fff");
                    header.Cell().Background("#257272").AlignCenter().Text("Role").FontColor("#fff");
                });
            });
        });
    });
}).ShowInPreviewer();