using System;

public class Item
{
    public enum TipoItem { Combustible, CrecimientoEstela, Bomba }
    public TipoItem Tipo { get; private set; }
    public int Valor { get; private set; }

    public Item(TipoItem tipo, int valor)
    {
        Tipo = tipo;
        Valor = valor;
    }
}
