using Godot;
using DTO;

public class Cidade : StaticBody
{
	private LocalidadeDTO DadosLocalidade { get; set; }
	public override void _Ready()
	{
		DadosLocalidade = new LocalidadeDTO();
	}
	public void DefinirDadosLocalidade(LocalidadeDTO localidadeDTO)
	{
		DadosLocalidade = localidadeDTO;
	}
	public LocalidadeDTO ObterDadosLocalidade()
	{
		return DadosLocalidade;
	}
}
