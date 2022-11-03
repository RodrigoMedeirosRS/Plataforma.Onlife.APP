using Godot;
using System;

namespace Onlife.CTRL
{
	public class InterfaceSobreposta : Control
	{
		public override void _Ready()
		{

		}

		public void _on_ExpandirButton_pressed()
		{
			GetNode<AnimationPlayer>("Anim").Play("Expandir");
		}

		public void _on_CloseButton_pressed()
		{
			GD.Print("Vivo");
			GetNode<AnimationPlayer>("Anim").Play("Retrair");
			GetNode<BarraDeBusca>("./BarraDeBusca").Exibir(false);
		}
	}
}
