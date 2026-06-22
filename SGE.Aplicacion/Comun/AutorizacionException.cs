namespace SGE.Aplicacion.Comun;

public class AutorizacionException : Exception
{
    public AutorizacionException(string mensaje) : base(mensaje) { }
}