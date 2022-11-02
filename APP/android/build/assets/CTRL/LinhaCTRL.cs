using Godot;

namespace BibliotecaViva.CTRL
{
	public class LinhaCTRL : ScrollContainer
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
