using Godot;
using System;

using DTO;
using BLL.Interface;
using CTRL.Interface;

public class Cabecalho : Control, IDisposableCTRL
{
	private Janela Dados { get; set; }
	private IConsultarRegistroBLL BLL { get; set; }
	private Label Nome { get; set; }
	private Label Apelido { get; set; }
	private Label Descricao { get; set; }
	public override void _Ready()
	{
		RealizarInjecaoDeDependencias();
		PopularNodes();
	}
	private void RealizarInjecaoDeDependencias()
	{
		BLL = new BLL.ConsultarRegistroBLL();
		Nome = GetNode<Label>("./Cabecalho/Nome/NomeTexto");
		Apelido = GetNode<Label>("./Cabecalho/Nome/NomeTexto");
		Descricao = GetNode<Label>("./Cabecalho/Nome/NomeTexto");
	}
	private void PopularNodes()
	{
		Dados = GetParent<Janela>();
	}
	private void _on_Janela_DadosCarregados()
	{
		PopularDados();
	}
	private void _on_Janela_Fechar()
	{
		FecharCTRL();
	}
	public void FecharCTRL()
	{
		Dados.RegistroDTO.Dispose();
		BLL.Dispose();
		GetParent().QueueFree();
	}
	private void PopularDados()
	{
		Nome.Text = Dados.RegistroDTO.Nome;
		Apelido.Text = Dados.RegistroDTO.Apelido;
		Descricao.Text = Dados.RegistroDTO.Descricao;
	}
	private void _on_Editar_button_up()
	{
		Main.DispararRegistro(Dados.RegistroDTO);
		FecharCTRL();
	}
	private void _on_Abrir_button_up()
	{
		
	}
	private void _on_Relacoes_button_up()
	{
		
	}
}