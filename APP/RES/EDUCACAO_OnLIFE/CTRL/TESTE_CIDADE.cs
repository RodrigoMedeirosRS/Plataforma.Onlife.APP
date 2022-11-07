using Godot;
using System;

public class TESTE_CIDADE : Control
{
	private Janela JenalaRegistro { get; set; }
	public override void _Ready()
	{
		JenalaRegistro = GetNode<Janela>("./Janela");
		JenalaRegistro.PopularDados(new DTO.RegistroDTO(){
			Nome = "Registro Teste",
			Apelido = "Apelido do Registro",
			Idioma = "Português",
			Tipo = "Texto",
			Conteudo = "Lula livre!",
			Descricao = "Descrição",
			DataInsercao = DateTime.Now
		});
	}
}
