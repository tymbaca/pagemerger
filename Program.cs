﻿// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using CommandLine;

class Program
{
    public class Options
    {
        [Value(0, Required = true, HelpText = "Name of the output file.", MetaName = "Output")]
        public string OutputFilename { set; get; }

        [Value(1, Required = true, Min = 2, HelpText = "Names of files that need to be merged.", MetaName = "Filenames")]
        public IEnumerable<string> Filenames { set; get; }

        [Option('b', "set-page-breaks", Required = false, HelpText = "Set page breaks between each file.")]
        public bool SetPageBreaks { set; get; }
    }

    static public void Main(string[] args)
    {
        var parseResult = Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => {
            // Console.Write(String.Join(", ", o.filenames));
            File.Delete(o.OutputFilename);
            var merger = new FileMerger(o.OutputFilename, o.Filenames, o.SetPageBreaks);
            // merger.MergeFiles();
            merger.MergeFiles();
        });
        if (parseResult.Errors.Count() == 0) {
            Environment.Exit(0);
        } else {
            Environment.Exit(1);
        }
    }
}

class FileMerger
{   
    private IEnumerable<string> filenames;
    private string outputFilename;
    private bool setPageBreaks;

    public FileMerger(string outputFilename, IEnumerable<string> filenames, bool setPageBreaks = false)
    {   
        if (filenames.Count() < 2) {
            throw new ArgumentException("Specified less then 2 files. Add 2 or more filenames.");
        }
        this.filenames = filenames;
        this.setPageBreaks = setPageBreaks;
        this.outputFilename = outputFilename;
    }

    public void MergeFiles()
    {
        File.Delete(outputFilename);
        File.Copy(filenames.ElementAt(0), outputFilename);
        for (int i = 1; i < filenames.Count(); i++)
        {
            if (setPageBreaks)
            {
                insertPageBreakToEndOfFile(outputFilename);
            }
            AppendDocToDoc(outputFilename, filenames.ElementAt(i));
        }
    }
    
    public void AppendDocToDoc(string targetFilename, string tailFilename)
    {
        using (WordprocessingDocument myDoc = WordprocessingDocument.Open(targetFilename, true))  
        {  
            string altChunkId = "_" + Guid.NewGuid().ToString("d");  
            MainDocumentPart mainPart = myDoc.MainDocumentPart;  
            // if (setPageBreaks) {
            //     mainPart.Document.Body.InsertAfterSelf(new Paragraph(
            //                                             new Run(
            //                                                 new Break { Type = BreakValues.Page })));
            // }
            AlternativeFormatImportPart chunk =   
                mainPart.AddAlternativeFormatImportPart(  
                AlternativeFormatImportPartType.WordprocessingML, altChunkId);  
            using (FileStream fileStream = File.Open(tailFilename, FileMode.Open))  
                chunk.FeedData(fileStream);  
            AltChunk altChunk = new AltChunk();  
            altChunk.Id = altChunkId;  
            mainPart.Document.Body  
                .InsertAfter(altChunk, mainPart.Document.Body  
                    .Elements().Last());  
            mainPart.Document.Save();  
        }
    }

    private void insertPageBreakToEndOfFile (string filename) 
    {
        WordprocessingDocument doc = WordprocessingDocument.Open(filename, true);
        MainDocumentPart mainPart = doc.MainDocumentPart;
        var pageBreak = new Paragraph(
                            new Run(
                                new Break { Type = BreakValues.Page }));

        // Chose in which file to add pagebreak depending on is there already embeded files or not
        if (mainPart.AlternativeFormatImportParts.Count() == 0)
        {
            mainPart.Document.Body.InsertAfter(pageBreak, mainPart.Document.Body  
                        .Elements<Paragraph>().Last());
        } else {
            WordprocessingDocument externalDoc = WordprocessingDocument.Open(mainPart.AlternativeFormatImportParts.Last().GetStream(), true);
            var externalMainPart = externalDoc.MainDocumentPart;
            externalMainPart.Document.Body.InsertAfter(pageBreak, externalMainPart.Document.Body  
                        .Elements<Paragraph>().Last());
            externalDoc.Close();
        }
        doc.Close();
    }
}
// WordprocessingDocument.Open("template.docx", true);

