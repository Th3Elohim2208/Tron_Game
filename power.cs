using System;

public class Power
{
    public enum TipoPower { Escudo, HiperVelocidad }
    public TipoPower Tipo { get; private set; }
    public int Duracion { get; private set; }
    public int Valor { get; private set; }

    public Power(TipoPower tipo, int duracion, int valor)
    {
        Tipo = tipo;
        Duracion = duracion;
        Valor = valor;
    }
}
