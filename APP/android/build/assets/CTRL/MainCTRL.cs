using Godot;

using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class MainCTRL : Control, IDisposableCTRL
	{ 
		private static IMainBLL BLL { get ; set; }
		private int Busca { get; set; }
		public override void _Ready()
		{
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			ReposicionarTela();
		}
		private void RealizarInjecaoDeDependencias()
		{
			Busca = 0;
			BLL = new MainBLL(GetNode<TabContainer>("./Container/TabContainer"));
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void ReposicionarTela()
		{
			if (OS.GetName() == "Windows")
			{
				var screen_size = OS.GetScreenSize(0);
				var window_size = OS.WindowSize;
				OS.WindowPosition = screen_size * 0.5f - window_size * 0.5f;
			}
		}
		public static void EditarRegistro(RegistroDTO registroDTO)
		{
			var tab = BLL.IntanciarTab(registroDTO.Nome, "res://RES/CENAS/TabCadastrarRegistro.tscn");
			if (tab != null)
				(tab as TabCadastrarRegistroCTRL).PopularPreenchiento(registroDTO);
		}
		public static void EditarPessoa(PessoaDTO pessoaDTO)
		{
			var tab = BLL.IntanciarTab(pessoaDTO.Nome + " " + pessoaDTO.Sobrenome, "res://RES/CENAS/TabCadastrarPessoa.tscn");
			if (tab != null)
				(tab as TabCadastrarPessoaCTRL).PopularPreenchiento(pessoaDTO);
		}
		private void _on_CadastrarPessoa_button_up()
		{
			BLL.IntanciarTab("Nova Pista Viva", "res://RES/CENAS/TabCadastrarPessoa.tscn");
		}
		private void _on_CadatrarRegistro_button_up()
		{
			BLL.IntanciarTab("Novo Registro", "res://RES/CENAS/TabCadastrarRegistro.tscn");
		}
		private void _on_Buscar_button_up()
		{
			Busca += 1;
			BLL.IntanciarTab("Buscar N°" + Busca, "res://RES/CENAS/TabBuscar.tscn");
		}
		private void _on_Sonar_button_up()
		{
			BLL.IntanciarTab("Sonar", "res://RES/CENAS/TabSonar.tscn");
		}
		private void _on_Rastros_button_up()
		{
			BLL.IntanciarTab("Rastros", "res://RES/CENAS/TabRastros.tscn");
		}
		private void _on_Sobre_button_up()
		{
			BLL.IntanciarTab("Sobre", "res://RES/CENAS/TabSobre.tscn");
		}
		public void FecharCTRL()
		{
			BLL.Dispose();
			QueueFree();
		}
	}
}
