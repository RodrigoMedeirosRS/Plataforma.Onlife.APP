using System;
using Godot;
using BibliotecaViva.DTO;

namespace BibliotecaViva.BLL.Utils
{
    public static class ImportadorDeBinariosUtil
    {
        public static AudioStream GerarAudio(string nomeImagem, string formato, string base64)
        {
            var data = Convert.FromBase64String(base64);
            
            switch(formato)
            {
                case (".wav"):
                    return new AudioStreamSample()
                    {
                        Data = data
                    }; 
                case (".mp3"):
                    return new AudioStreamMP3()
                    {
                        Data = data
                    };
                case (".ogg"):
                    return new AudioStreamOGGVorbis()
                    {
                        Data = data
                    };
                default:
                    throw new Exception("Formato de áudio não suportado, por favor use .wav, .mp3 ou .ogg");
            }
        }
        public static Texture GerarImagem(string nomeImagem, string formato, string base64)
        {
            var caminho = CarregarBinario(nomeImagem, formato, base64);
            return BuscarImagem(nomeImagem, formato, caminho);
        }
        public static Texture BuscarImagem(string nomeImagem, string formato, string caminho)
        {
            var imagem = new Image();
            var texturaDaImagem = new ImageTexture();
            var caminhoComFormato = caminho + nomeImagem + "." + formato;
            var caminhoImport = caminhoComFormato + ".import";

            imagem.Load(caminhoComFormato);
            texturaDaImagem.CreateFromImage(imagem);
            LimparArquivosTemporarios(caminhoComFormato, caminhoImport);
            
            return texturaDaImagem;
        }
        private static string CarregarBinario(string nomeBinario, string formato, string base64)
        {
            var caminho = ObterDiretorioTemporario();
            System.IO.File.WriteAllBytes(caminho + nomeBinario + "." + formato, Convert.FromBase64String(base64));
            return caminho;
        }

        private static string ObterDiretorioTemporario()
        {
            if (!System.IO.Directory.Exists("TEMP"))
                System.IO.Directory.CreateDirectory("TEMP");
            return "./TEMP/";
        }

        private static void LimparArquivosTemporarios(string caminhoComFormato, string caminhoImport)
        {
            if (System.IO.File.Exists(caminhoComFormato))
                System.IO.File.Delete(caminhoComFormato);
            if (System.IO.File.Exists(caminhoImport))
                System.IO.File.Delete(caminhoImport);
        }
        public static string ObterBase64(string nomeImagem, string formato, string caminho)
        {
            return ObterBase64(caminho + nomeImagem, formato);
        }
        public static string ObterBase64(string caminho, string formato)
        {
            return ObterBase64(caminho + formato);
        }
        public static string ObterBase64(string caminho)
        {
            return System.Convert.ToBase64String(System.IO.File.ReadAllBytes(caminho));
        }
        public static void SalvarBase64(string caminho, string base64)
        {
            ObterDiretorioTemporario();
            System.IO.File.WriteAllBytes(caminho, Convert.FromBase64String(base64));
        }
        public static void DeletarBinario(string caminho)
        {
            if (System.IO.File.Exists(caminho))
                System.IO.File.Delete(caminho);
        }
    }
}