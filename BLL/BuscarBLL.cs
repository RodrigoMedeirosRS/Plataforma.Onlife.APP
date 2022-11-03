using Godot;
using System;

using BLL.Utils;
using BLL.Interface;

namespace BLL
{
    public class BuscarBLL : IBuscarBLL, IDisposable
    {
        private HBoxContainer Container { get; set; }
        public BuscarBLL(HBoxContainer container)
        {
            Container = container;
        }
        public void InstanciarColuna(string caminho)
        {
            var cena = InstanciadorUtil.CarregarCena(caminho);
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