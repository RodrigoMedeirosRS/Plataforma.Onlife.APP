using Godot;
using System;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class PesquisaCTRL : Control, IDisposableCTRL
	{
		private IConsultarTipoBLL BLL { get; set; }
		public OptionButton Idioma { get; set; }
		public LineEdit Nome { get; set; }
		public LineEdit Sobrenome { get; set; }
		public LineEdit Apelido { get; set; }
		private OptionButton Tipo { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			PopularNodes();
			PopularDropDowns();
			DesativarFuncoesDeAltoProcessamento();
		}
		private void RealizarInjecaoDeDependencias()
		{
			BLL = new ConsultarTipoBLL();
		}
		private void PopularNodes()
		{
			Nome = GetNode<LineEdit>("./Nome");
			Tipo = GetNode<OptionButton>("./Tipo");
			Apelido = GetNode<LineEdit>("./Apelido");
			Idioma = GetNode<OptionButton>("./Idioma");
			Sobrenome = GetNode<LineEdit>("./Sobrenome");
		}
		private void PopularDropDowns()
		{
			BLL.PopularDropDownIdioma(Idioma);
			PoularTipoDropDown();
		}
		private void PoularTipoDropDown()
		{
			Tipo.AddItem("Pista Viva");
			Tipo.AddItem("Registro");
			Tipo.Select(0);
			AlternarOpcoesDeBusca(0);
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void AlternarOpcoesDeBusca(int index)
		{
			Sobrenome.Visible = Tipo.GetItemText(index) == "Pista Viva";
			Idioma.Visible = Tipo.GetItemText(index) == "Registro";
		}
		private void _on_Tipo_item_selected(int index)
		{
			AlternarOpcoesDeBusca(index);
		}
		public bool ConsultaPessoa()
		{
			return Sobrenome.Visible;
		}
		public void FecharCTRL()
		{
			BLL.Dispose();
			Idioma.QueueFree();
			Apelido.QueueFree();
			Sobrenome.QueueFree();
			Tipo.QueueFree();
			Nome.QueueFree();
			QueueFree();
		}
	}
}
