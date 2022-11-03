using Godot;
using System.Threading.Tasks;

namespace Onlife.CTRL
{
	public class RegistroGeolocalizado : TextureButton
	{
		[Export]
		public string NomeRegistro { get; set; }
		[Export]
		public string ApelidoRegistro { get; set; }
		[Export]
		public DadosCTRL Dados { get; set; }
		public override void _Ready()
		{
			DesativarFuncoesDeAltoProcessamento();
			Dados = FindParent("Main").GetNode<DadosCTRL>("./Interface/InterfaceSobreposta/Dados");
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		public void PopularDados(string nome, string apelido, DadosCTRL dados)
		{
			NomeRegistro = nome;
			ApelidoRegistro = apelido;
			Dados = dados;
		}
		private void _on_RegistroGeolocalizado_button_up()
		{
			if (Dados != null)
				if (!string.IsNullOrEmpty(NomeRegistro) || !string.IsNullOrEmpty(ApelidoRegistro))
					Task.Run(async () => await Dados.RealizarConsultaRegistro(NomeRegistro, ApelidoRegistro));
		}
	}
}
