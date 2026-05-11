using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    //Constructor
    public Tramite(Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, Guid usuarioUltimoCambio)
    {
        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = DateTime.Now;
        UsuarioUltimoCambio = usuarioUltimoCambio;
    }

    // Factory Method
    public static Tramite Reconstruir(Guid id, Guid expedienteId, EtiquetaTramite etiqueta, ContenidoTramite contenido, DateTime fechaCreacion,
                                      DateTime fechaUltimaModificacion, Guid usuarioUltimoCambio)
    {
        if (fechaUltimaModificacion < fechaCreacion)
            throw new DominioException("La fecha de modificacion no puede ser menor a la de creacion.");
        return new Tramite(expedienteId, etiqueta, contenido, usuarioUltimoCambio)
        {
            Id = id,
            FechaCreacion = fechaCreacion,
            FechaUltimaModificacion = fechaUltimaModificacion
        };
    }

    //Modificar contenido
    public void ModificarContenido(ContenidoTramite nuevoContenido, Guid idUsuario)
    {
        Contenido = nuevoContenido;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }
}
