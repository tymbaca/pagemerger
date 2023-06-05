// See https://aka.ms/new-console-template for more information
using System.Linq;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using CommandLine;

class Program
{
    public class Options
    {
        [Value(0, HelpText = "Name of the output file.", MetaName = "Output")]
        public string OutputFilename { set; get; }

        [Value(1, Min = 2, HelpText = "Names of files that need to be merged.", MetaName = "Filenames")]
        public IEnumerable<string> Filenames { set; get; }

        [Option('b', "set-page-breaks", Required = false, HelpText = "Set page breaks between each file.")]
        public bool SetPageBreaks { set; get; }
    }

    static public void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args).WithParsed<Options>(o => {
            // Console.Write(String.Join(", ", o.filenames));
            File.Delete(o.OutputFilename);
            var merger = new FileMerger(o.Filenames, o.OutputFilename, o.SetPageBreaks);
            merger.MergeFiles();
        });
    }
}

class FileMerger
{   
    private IEnumerable<string> filenames;
    private string outputFilename;
    private bool setPageBreaks;

    public FileMerger(IEnumerable<string> filenames, string outputFilename, bool setPageBreaks = false)
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
        WordprocessingDocument mainDoc = WordprocessingDocument.Open(outputFilename, true);
        for (int i = 1; i < filenames.Count(); i++)
        {
            // var altChunkId = $"altChunk{i}";
            WordprocessingDocument tailDoc = WordprocessingDocument.Open(filenames.ElementAt(i), true);
            mergeTwoDocs(mainDoc, tailDoc);
        }
    }
    
    private void mergeTwoDocs(WordprocessingDocument mainDoc, WordprocessingDocument tailDoc)
    {
        string fileName1 = @"c:\Users\Public\Documents\Destination.docx";  
        string fileName2 = @"c:\Users\Public\Documents\Source.docx";  
        string testFile = @"c:\Users\Public\Documents\Test.docx";  
        File.Delete(outputFilename);  
        File.Copy(testFile, fileName1);  
        using (WordprocessingDocument myDoc =  
            WordprocessingDocument.Open(fileName1, true))  
        {  
            string altChunkId = "AltChunkId1";  
            MainDocumentPart mainPart = myDoc.MainDocumentPart;  
            AlternativeFormatImportPart chunk =   
                mainPart.AddAlternativeFormatImportPart(  
                AlternativeFormatImportPartType.WordprocessingML, altChunkId);  
            using (FileStream fileStream = File.Open(fileName2, FileMode.Open))  
                chunk.FeedData(fileStream);  
            AltChunk altChunk = new AltChunk();  
            altChunk.Id = altChunkId;  
            mainPart.Document  
                .Body  
                .InsertAfter(altChunk, mainPart.Document.Body  
                .Elements<Paragraph>().Last());  
            mainPart.Document.Save();  
        }
    }
}
// WordprocessingDocument.Open("template.docx", true);

