using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Web;
using LanchesMac.Areas.Admin.FastReportUtils;
using LanchesMac.Areas.Admin.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers;
[Area("Admin")]
public class AdminLanchesReportController : Controller
{
	private readonly IWebHostEnvironment _webHostEnv;
	private readonly RelatorioLanchesService _relatorioLanchesService;

	public AdminLanchesReportController(IWebHostEnvironment webHostEnv, RelatorioLanchesService relatorioLanchesService)
	{
		_webHostEnv = webHostEnv;
		_relatorioLanchesService = relatorioLanchesService;
	}


	[Route("LanchesCategoriaReport")]
	public async Task<IActionResult> LanchesCategoriaReport()
	{
		var webReport = new WebReport();
		var mssqlDataConnection = new MsSqlDataConnection();

		webReport.Report.Dictionary.AddChild(mssqlDataConnection);

		webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports", "LanchesCategoria.frx"));

		var lanches = HelperFastReport.GetTable(await _relatorioLanchesService.GetLanchesReport(), "LanchesReport");

		var categorias = HelperFastReport.GetTable(await _relatorioLanchesService.GetCategoriasReport(), "CategoriasReport");

		webReport.Report.RegisterData(lanches, "LanchesReport");
		webReport.Report.RegisterData(categorias, "CategoriasReport");

		return View(webReport);
	}
	public async Task<IActionResult> LanchesCategoriaPDF()
	{
		var webReport = new WebReport();
		var mssqlDataConnection = new MsSqlDataConnection();

		webReport.Report.Dictionary.AddChild(mssqlDataConnection);

		webReport.Report.Load(Path.Combine(_webHostEnv.ContentRootPath, "wwwroot/reports", "LanchesCategoria.frx"));

		var lanches = HelperFastReport.GetTable(await _relatorioLanchesService.GetLanchesReport(), "LanchesReport");

		var categorias = HelperFastReport.GetTable(await _relatorioLanchesService.GetCategoriasReport(), "CategoriasReport");

		webReport.Report.RegisterData(lanches, "LanchesReport");
		webReport.Report.RegisterData(categorias, "CategoriasReport");

		webReport.Report.Prepare();

		Stream stream = new MemoryStream();

		webReport.Report.Export(new PDFSimpleExport(), stream);
		stream.Position = 0;


		//Gerar Download do PDF direto
		return File(stream, "application/zip", "LancheCategoria.pdf");

		//Abrir o pdf direto no navegador
		/*return new FileStreamResult(stream, "application/pdf")*/
		;
	}
}
