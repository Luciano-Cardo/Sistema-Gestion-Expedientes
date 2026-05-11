using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public record class Caratula
{
    public string Valor { get; }
    public Caratula(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DominioException("La caratula no puede estar vacia.");
        }
        Valor = valor;
    }
    public override string ToString()
    {
        return Valor;
    }
}
