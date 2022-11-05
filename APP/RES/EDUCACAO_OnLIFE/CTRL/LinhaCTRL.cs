using Godot;
using System;
using CTRL.Interface;

namespace Onlife.CTRL
{
	public class LinhaCTRL : ScrollContainer, IDisposableCTRL
	{
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
		public override void _Process(float delta)
		{
			Visible = GetChild(0).GetChildCount() > 0;
		}
		public void FecharCTRL()
		{
			QueueFree();
		}
	}
}
