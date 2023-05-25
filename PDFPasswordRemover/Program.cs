using iText.Kernel.Pdf;
using System.Text;

if (args.Length == 0)
{
	Console.Error.WriteLine("No password provided");
	return;
}

var password = args[0];

Directory.CreateDirectory("output");
var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.pdf").Select(fullPath => Path.GetFileName(fullPath));

foreach (var file in files)
{
	RemovePasswordAndSave(file, password);
}

static void RemovePasswordAndSave(string filename, string password)
{
	var trimmedFilename = filename["Loonstrook ".Length..filename.IndexOf('.')];
	var seperatorIndex = trimmedFilename.IndexOf("-");
	var month = trimmedFilename[0..seperatorIndex];
	var year = trimmedFilename[(seperatorIndex + 1)..];
	var newFilename = $"{year}-{month}.pdf";
	var properties = new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(password));
	using var passwordProtectedPdf = new PdfDocument(new PdfReader(filename, properties));
	using var newPdf = new PdfDocument(new PdfWriter($"output/{newFilename}"));

	passwordProtectedPdf.CopyPagesTo(1, passwordProtectedPdf.GetNumberOfPages(), newPdf);
}