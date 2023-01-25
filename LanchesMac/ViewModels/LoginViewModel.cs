using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace LanchesMac.ViewModels;
public class LoginViewModel
{
	[Required(ErrorMessage = "Informe o usuário")]
	[Display(Name = "Usuário")]
	public string UserName { get; set; }
	[Required(ErrorMessage = "Informe a senha")]
	[DataType(DataType.Password)]
	[Display(Name = "Senha")]
	public string Password { get; set; }
	public string ReturnUrl { get; set; }
}
