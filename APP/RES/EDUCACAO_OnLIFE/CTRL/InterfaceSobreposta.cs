using Godot;
using System;

namespace Onlife.CTRL
{
	public class InterfaceSobreposta : Control
	{
		private AcceptDialog CaixaDeDialogo { get; set; }
		private ConfirmationDialog CaixaDePergunta { get; set; }
		private FileDialog CaixaDeArquivos { get; set; }
		public override void _Ready()
		{
			PopularNodes();
		}
		private void PopularNodes()
		{
			CaixaDeDialogo = GetNode<AcceptDialog>("../Popups/CaixaDeDialog");
			CaixaDePergunta = GetNode<ConfirmationDialog>("../Popups/CaixadeConfirmacao");
			CaixaDeArquivos = GetNode<FileDialog>("./Popups/FileDialog");
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
			// Replace with function body.
		}
		private void _on_NovoRegistro_button_up()
		{
			// Replace with function body.
		}
		private void _on_NovaCidade_button_up()
		{
			DispararDialogo( "Por favor, clique sobre o globo marcando a posição aproximada da cidade que você quer cadastrar.");
		}

		private void DispararDialogo(string mensagem)
		{
			CaixaDeDialogo.DialogText = mensagem;
			CaixaDeDialogo.Popup_();
		}
	}
}
