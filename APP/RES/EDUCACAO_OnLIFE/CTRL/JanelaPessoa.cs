using Godot;
using System;

using DTO;
using DTO.Utils;
using BLL.Utils;
using CTRL.Interface;

public class JanelaPessoa : Control, IDisposableCTRL
{
	private PessoaDTO PistaViva { get; set; }
	private bool Edicao { get; set; }
	private bool MousePosicionado { get; set; }
	private TextureRect Foto { get; set;}
	private Label Nome { get; set; }
	private Label Apelido { get; set; }
	private TextureButton Lattes { get; set; }
	private TextureButton LinkdIn { get; set; }
	private TextureButton ResearchGate { get; set; }
	private Texture ImagemOriginal { get; set; }
	private ConfirmationDialog Alerta { get; set; }
	public override void _Ready()
	{
		PopularNodes();
	}
	public override void _Process(float delta)
	{
		if(MousePosicionado && Input.IsActionPressed("selecao"))
			this.RectGlobalPosition = GetGlobalMousePosition();
	}
	public void PopularNodes()
	{
		Edicao = false;
		MousePosicionado = false;
		Alerta = GetNode<ConfirmationDialog>("./Conteudo/Alterta");
		Foto = GetNode<TextureRect>("./Cabecalho/Foto");
		Nome = GetNode<Label>("./Cabecalho/Nome/NomeTexto");
		Apelido = GetNode<Label>("./Cabecalho/Apelido/ApelidoTexto");
		Lattes = GetNode<TextureButton>("./Cabecalho/Lattes");
		LinkdIn = GetNode<TextureButton>("./Cabecalho/LinkedIn");
		ResearchGate = GetNode<TextureButton>("./Cabecalho/ResearchGate");
		ImagemOriginal = Foto.Texture;
	}
	public void DefinirDados (PessoaDTO pistaviva)
	{
		PistaViva = pistaviva;
		PopularDados();
	}
	private void PopularDados()
	{
		Nome.Text = PistaViva.Nome;
		Apelido.Text = PistaViva.Apelido;
		Lattes.Visible = !string.IsNullOrEmpty(PistaViva.Lattes);
		LinkdIn.Visible = !string.IsNullOrEmpty(PistaViva.LinkedIn);
		ResearchGate.Visible = !string.IsNullOrEmpty(PistaViva.ResearchGate);
		Foto.Texture = ImportadorDeBinariosUtil.GerarImagem("temp", ".jpg", PistaViva.Foto);
	}
	public void FecharCTRL()
	{
		Main.FecharArvore();
		PistaViva.Dispose();
	}
	private void _on_BarraSuperior_mouse_exited()
	{
		MousePosicionado = false;
	}
	private void _on_BarraSuperior_mouse_entered()
	{
		MousePosicionado = true;
	}
	private void _on_Editar_button_up()
	{
		Edicao = true;
		Alerta.Popup_();
	}
	private void AbrirLinkExterno(string url)
	{
		if(!string.IsNullOrEmpty(url))
			OS.ShellOpen(url);
	}
	private void _on_Relacoes_button_up()
	{
		
	}
	private void _on_Alterta_confirmed()
	{
		if (Edicao)
			Main.DispararPistaViva(PistaViva);
		FecharCTRL();
	}
	private void _on_Lattes_button_up()
	{
		AbrirLinkExterno(PistaViva.Lattes);
	}
	private void _on_ResearchGate_button_up()
	{
		AbrirLinkExterno(PistaViva.ResearchGate);
	}
	private void _on_LinkedIn_button_up()
	{
		AbrirLinkExterno(PistaViva.LinkedIn);
	}
	private void _on_Fechar_button_up()
	{
		Edicao = false;
		Alerta.Popup_();
	}
}