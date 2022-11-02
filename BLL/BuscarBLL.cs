using Godot;
using System;

using BibliotecaViva.BLL.Utils;
using BibliotecaViva.BLL.Interface;

namespace BibliotecaViva.BLL
{
    public class BuscarBLL : IBuscarBLL, IDisposable
    {
        private HBoxContainer Container { get; set; }
        public BuscarBLL(HBoxContainer container)
        {
            Container = container;
        }
        public void InstanciarColuna()
        {
            var cena = InstanciadorUtil.CarregarCena("res://RES/CENAS/Linha.tscn");
            var coluna = InstanciadorUtil.InstanciarObjeto(Container, cena, null);
            coluna._Ready();
        }
        public void RemoverColuna(Node linha)
        {
            if (Container.GetChildCount() > 0 && linha.GetChild(0).GetChildCount() == 0)
                linha.QueueFree();
        }
        public bool ValidarColuna(int coluna)
        {
            return Container.GetChildCount() <= coluna; 
        }
        
        public void Dispose()
        {
            Container.QueueFree();
        }
    }
}