using BibliotecaViva.DTO.Dominio;

namespace BibliotecaViva.BLL.Interface
{
    public interface ISonarBLL
    {
        SonarConsulta ObterDadosSonar(double latitude, double longitude, double alcance);
        SonarRetorno ExecutarSonar(SonarConsulta sonar);

        void Dispose();
    }
}