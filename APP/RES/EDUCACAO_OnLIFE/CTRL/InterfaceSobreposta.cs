using Godot;
using System;

namespace Onlife.CTRL
{
	public class InterfaceSobreposta : Control
	{
		public override void _Ready()
		{
			PopularNodes();
		}
		private void PopularNodes()
		{

		}
		public void _on_ExpandirButton_pressed()
		{
			GetNode<AnimationPlayer>("Anim").Play("Expandir");
		}

		public void _on_CloseButton_pressed()
		{
			GetNode<AnimationPlayer>("Anim").Play("Retrair");
		}
		private void _on_NovaPistaViva_button_up()
		{
			Main.DispararPistaViva();
		}
		private void _on_NovoRegistro_button_up()
		{
			// Replace with function body.
		}
		private void _on_NovaCidade_button_up()
		{
			Main.DispararDialogo("Por favor, clique com o botão direito do mouse sobre o globo marcando a posição aproximada da cidade que você quer cadastrar.");
		}
		private void _on_Buscar_button_up()
		{
			// Replace with function body.
		}
	}
}
