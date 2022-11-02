using Godot;
using System;
using System.Threading.Tasks;
using BibliotecaViva.DTO;
using BibliotecaViva.BLL;
using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.BLL.Interface;
using BibliotecaViva.CTRL.Interface;

namespace BibliotecaViva.CTRL
{
	public class TabSonarCTRL : Tabs, IDisposableCTRL
	{
		private ISonarBLL SonarBLL { get; set; }
		private IBuscarBLL BuscarBLL { get; set; }
		private IConsultarRegistroBLL RegistroBLL { get; set; }
		private IConsultarPessoaBLL PessoaBLL { get; set; }
		private PessoaBoxCTRL PessoaBox { get; set; }
		private RegistroBoxCTRL RegistroBox { get; set; }
		private AcceptDialog PopErro { get; set; }
		private HBoxContainer Coluna { get; set; }
		private double Alcance { get; set; }
		public override void _Ready()
		{	
			PopularNodes();
			RealizarInjecaoDeDependencias();
			DesativarFuncoesDeAltoProcessamento();
			InstanciarColuna();
		}
		private void RealizarInjecaoDeDependencias()
		{
			
			SonarBLL = new SonarBLL();
			BuscarBLL = new BuscarBLL(Coluna);
			RegistroBLL = new ConsultarRegistroBLL();
			PessoaBLL = new ConsultarPessoaBLL();
		}
		private void DesativarFuncoesDeAltoProcessamento()
		{
			SetPhysicsProcess(false);
			SetProcess(false);
		}
		private void PopularNodes()
		{
			Alcance = 0.0001;
			PopErro = GetNode<AcceptDialog>("./PopErro");
			Coluna = GetNode<HBoxContainer>("./Dados/ScrollContainer/Colunas");
			PessoaBox = GetNode<PessoaBoxCTRL>("./Dados/Boxes/PessoaBox");
			RegistroBox = GetNode<RegistroBoxCTRL>("./Dados/Boxes/RegistroBox");
		}
		private void _on_HSlider_value_changed(float value)
		{
			AjustarAlcance(value);
		}
		private void AjustarAlcance(float value)
		{
			Alcance = (double)value * 0.0001;
		}
		private void _on_Timer_timeout()
		{
			Task.Run(async () => await ExecutarSonar());
		}
		private async Task ExecutarSonar()
		{
			var sonar = SonarBLL.ObterDadosSonar(-29.9902427008141, -50.126489165424786, Alcance);
			var resultado = SonarBLL.ExecutarSonar(sonar);
			if (resultado != null)
			{
				foreach (var relacao in resultado.Registros)
				{
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(0), 0);
				}
			}
		}
		private void InstanciarColuna()
		{
			BuscarBLL.InstanciarColuna();
		}
		private void ExibirErro(string mensagem)
		{
			PopErro.DialogText = mensagem;
			PopErro.Popup_();
		}
		private void InstanciarPessoaBox(PessoaObject pessoaObjct, VBoxContainer container, int coluna)
		{
			if (ValidarPessoaJaInstanciadaNaColuna(pessoaObjct.Pessoa, coluna))
				return;
			var pessoaBox = PessoaBox.Duplicate();
			container.AddChild(pessoaBox);
			pessoaBox._Ready();
			(pessoaBox as PessoaBoxCTRL).Preencher(pessoaObjct.Pessoa, new Vector2(0, 0));
			(pessoaBox as PessoaBoxCTRL).TabSonar = this;
			(pessoaBox as PessoaBoxCTRL).Coluna = coluna;
		}
		private void InstanciarRegistroBox(RegistroObject registroObjct, VBoxContainer container, int coluna)
		{
			if (ValidarRegistroJaInstanciadoNaColuna(registroObjct.Registro, coluna))
				return;
			var registroBox = RegistroBox.Duplicate();
			container.AddChild(registroBox);
			registroBox._Ready();
			(registroBox as RegistroBoxCTRL).Preencher(registroObjct.Registro, new Vector2(0, 0));
			(registroBox as RegistroBoxCTRL).TabSonar = this;
			(registroBox as RegistroBoxCTRL).Coluna = coluna;
		}
		private bool ValidarPessoaJaInstanciadaNaColuna(PessoaDTO pessoa, int coluna)
		{
			var pessoas = ObterColuna(coluna).GetChildren();
			foreach (var pessoaBox in pessoas)
			{
				if ((pessoaBox as PessoaBoxCTRL)?.Pessoa.Codigo == pessoa.Codigo)
					return true;
			}
			return false;
		}
		private bool ValidarRegistroJaInstanciadoNaColuna(RegistroDTO registro, int coluna)
		{
			var registros = ObterColuna(coluna).GetChildren();
			foreach (var registroBox in registros)
			{
				if ((registroBox as RegistroBoxCTRL)?.Registro.Codigo == registro.Codigo)
					return true;
			}
			return false;
		}
		private VBoxContainer ObterColuna(int coluna)
		{
			if (BuscarBLL.ValidarColuna(coluna))
			{
				BuscarBLL.InstanciarColuna();
				System.Threading.Thread.Sleep(100);
			}
			return Coluna.GetChild(coluna).GetChild<VBoxContainer>(0);
		}
		public async Task BuscarRelacoes(PessoaDTO pessoa, int coluna, Node box)
		{
			try
			{
				var resultado = PessoaBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(pessoa.Codigo));
				var novaColuna = coluna + 1;			
				foreach (var relacao in resultado)
				{
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna);
				}
			}
			catch(Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
		public async Task BuscarRelacoes(RegistroDTO registro, int coluna, Node box)
		{
			try
			{
				var resultado = RegistroBLL.RealizarConsultaDeRegistrosRelacionados(new RelacaoConsulta(registro.Codigo));
				var novaColuna = coluna + 1;			
				foreach (var relacao in resultado.Registros)
					CallDeferred("InstanciarRegistroBox", new RegistroObject(relacao, null), ObterColuna(novaColuna), novaColuna);
				foreach (var pessoa in resultado.Pessoas)
					CallDeferred("InstanciarPessoaBox", new PessoaObject(pessoa), ObterColuna(novaColuna), novaColuna);
			}
			catch(Exception ex)
			{
				CallDeferred("ExibirErro", ex.Message);
			}
		}
		public void FecharCTRL()
		{
			PopErro.QueueFree();
			SonarBLL.Dispose();
			RegistroBLL.Dispose();
			PessoaBLL.Dispose();
			foreach(var coluna in Coluna.GetChildren())
			{
				foreach(var linha in (coluna as Node).GetChild(0).GetChildren())
					(linha as IDisposableCTRL).FecharCTRL();
				BuscarBLL.RemoverColuna((coluna as Node));
			}
			Coluna.QueueFree();
			RegistroBox.FecharCTRL();
			PessoaBox.FecharCTRL();
			QueueFree();
		}
	}
}
