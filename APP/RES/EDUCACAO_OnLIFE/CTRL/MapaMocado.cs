using Godot;
using System;

namespace Onlife.CTRL
{
	public class MapaMocado : Control
	{
		private Control Offset { get; set; }
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
			PopularNode();
		}
		private void PopularNode()
		{
			Offset = (GetParent() as Control);
		}
		public override void _Process(float delta)
		{
			var scala = Offset.RectScale.x * 10.0f;
			if (scala > 1)
				scala = 1;
			this.Modulate = new Color(1, 1, 1, scala);
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
		}
	}
}
