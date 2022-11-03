using Godot;
using System;

using CTRL.Interface;

namespace Onlife.CTRL
{
	public class JanelaBase : Control
	{
		private IDisposableCTRL Conteudo { get; set; }
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
		}
		public void PopularConteudo(IDisposableCTRL conteudo)
		{
			Conteudo = conteudo;
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void _on_CloseButton_pressed()
		{
			Conteudo?.FecharCTRL();
			QueueFree();
		}
	}
}
