using Godot;
using System;

using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class TabSobreCTRL : Tabs, IDisposableCTRL
	{
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void _on_RepoAPI_button_up()
		{
			OS.ShellOpen("https://github.com/RodrigoMedeirosRS/BibliotecaViva.API");
		}
		private void _on_RepoAPP_button_up()
		{
			OS.ShellOpen("https://github.com/RodrigoMedeirosRS/BibliotecaViva.APP");
		}
		public void FecharCTRL()
		{
			QueueFree();
		}
	}
}
