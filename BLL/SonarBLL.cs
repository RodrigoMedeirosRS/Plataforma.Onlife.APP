
using System;
using BibliotecaViva.SAL;
using BibliotecaViva.DTO;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.DTO.Dominio;
using BibliotecaViva.SAL.Interface;
using BibliotecaViva.BLL.Interface;

namespace BibliotecaViva.BLL
{
    public class SonarBLL : ISonarBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLSonar { get; set; }
        public SonarBLL()
        {
            SAL = new Requisicao();
            URLSonar = Apontamentos.URLApi + "/Sonar/Consultar";
        }
        public SonarConsulta ObterDadosSonar(double latitude, double longitude, double alcance)
        {
            return new SonarConsulta()
            {
                Latitude = latitude.ToString().Replace(",", "."),
                Longitude = longitude.ToString().Replace(",", "."),
                Alcance = alcance.ToString().Replace(",", ".")
            };
        }
        public SonarRetorno ExecutarSonar(SonarConsulta sonar)
        {
            return SAL.ExecutarPost<SonarConsulta, SonarRetorno>(URLSonar, sonar);
        }
        public void Dispose()
        {
            URLSonar = null;
            SAL.Dispose();
        }
    }
}