using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
	[Area("Admin")]
	[Authorize("Admin")]
	public IActionResult Index()
	{
		return View();
	}
}
