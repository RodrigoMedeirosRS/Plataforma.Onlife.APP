using Godot;
using System.Linq;
using System.Collections.Generic;

namespace Onlife.CTRL
{
	public class BarraDeBusca : Control
	{
		private List<string> Opcoes { get; set; }
		private LineEdit Termo { get; set; }
		private OptionButton Dropdown { get; set; }
		private AnimationPlayer Player { get; set; }
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
			PopularAtributos();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void PopularAtributos()
		{
			Dropdown = GetNode<OptionButton>("./Panel/OptionButton");
			Player = GetNode<AnimationPlayer>("./AnimationPlayer");
			Termo = GetNode<LineEdit>("./LineEdit");
			Opcoes = new List<string>();
			Opcoes.Add("Pista Viva");
			Opcoes.Add("Registro");
			Dropdown.AddItem("Pista Viva");
			Dropdown.AddItem("Registro");
		}
		public string ObterSelecao()
		{
			var selecao = Dropdown.GetSelectedId();
			return Opcoes[selecao];
		}
		public List<string> ObterTermoDeBusca()
		{
			var termos = Termo.Text.Split(" ");
			var retorno = new List<string>();
			retorno.Add(termos[0]);
			var termo2 = string.Empty;
			for (int i = 1; i < termos.Count(); i++)
				termo2 += " " + termos[i];
			retorno.Add(termo2);
			return retorno;
		}
		public void Exibir(bool exibir)
		{
			if (exibir)
				Player.Play("Exibir");
			else
				Player.Play("Ocultar");
		}
	}
}
