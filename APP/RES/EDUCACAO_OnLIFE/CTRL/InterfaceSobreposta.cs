using Godot;
using System;

	public class InterfaceSobreposta : Control
	{
		public const string MensagemRegistro = "Por favor, clique com o botão direito do mouse sobre o mapa marcando a localização aproximada do registro que você quer cadastrar.";
		public const string MensagemCidade = "Por favor, clique com o botão direito do mouse sobre o globo marcando a posição aproximada da cidade que você quer cadastrar.";
		private AnimationPlayer Anim { get; set; }
		public override void _Ready()
		{
			PopularNodes();
		}
		public override void _Process(float delta)
		{
			if(Input.IsActionPressed("sair"))
				ExpandirRetrair(false);
		}
		private void PopularNodes()
		{
			Anim = GetNode<AnimationPlayer>("Anim");
		}
		public void _on_ExpandirButton_pressed()
		{
			ExpandirRetrair(true);
		}
		public void _on_CloseButton_pressed()
		{
			ExpandirRetrair(false);
		}
		private void ExpandirRetrair(bool expandir)
		{
			var acao = expandir ? "Expandir" : "Retrair";
			Anim.Play(acao);
		}
		private void _on_NovaPistaViva_button_up()
		{
			Main.DispararPistaViva();
		}
		private void _on_NovoRegistro_button_up()
		{
			if (Main.LocalidadeMode())
				Main.DispararDialogo(MensagemRegistro);
			else
				Main.DispararRegistro();
		}
		private void _on_NovaCidade_button_up()
		{
			Main.DispararDialogo(MensagemCidade);
		}
		private void _on_Buscar_button_up()
		{
			// Replace with function body.
		}
	}