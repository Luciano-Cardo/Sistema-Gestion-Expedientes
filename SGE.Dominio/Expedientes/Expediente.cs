using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoExpediente Estado { get; private set; }

    //Constructor
    public Expediente(Caratula caratula, Guid usuarioUltimoCambio)
    {
        Id = Guid.NewGuid();
        Caratula = caratula;
        UsuarioUltimoCambio = usuarioUltimoCambio;
        FechaCreacion = DateTime.Now;
        FechaUltimaModificacion = DateTime.Now;
        Estado = EstadoExpediente.RecienIniciado;
    }

    // Factory Method
    public static Expediente Reconstruir(Guid id, Caratula caratula, DateTime fechaCreacion, DateTime fechaUltimaModificacion,
                                         Guid usuarioUltimoCambio, EstadoExpediente estado)
    {
        if (fechaUltimaModificacion < fechaCreacion)
            throw new DominioException("La fecha de modificacion no puede ser menor que la fecha de creacion.");
        return new Expediente(caratula, usuarioUltimoCambio)
        {
            Id = id, 
            FechaCreacion = fechaCreacion, 
            FechaUltimaModificacion = fechaUltimaModificacion, 
            Estado = estado
        };
    }

    //Modificar caratula
    public void ModificarCaratula(Caratula nuevaCaratula, Guid idUsuario)
    {
        Caratula = nuevaCaratula;
        UsuarioUltimoCambio = idUsuario;    
        FechaUltimaModificacion = DateTime.Now;
    }

    //Cambio manual
    public void CambiarEstado(EstadoExpediente nuevoEstado, Guid idUsuario)
    {
        Estado = nuevoEstado;
        UsuarioUltimoCambio = idUsuario;
        FechaUltimaModificacion = DateTime.Now;
    }

    //Cambia automatico
    /*
        Que pasaria en la situacion en la cual recibas un estado de tramite X y el estado del expediente sea FINALIZADO.
    */
    public bool ActualizarEstado(EtiquetaTramite? ultimaEtiqueta, Guid idUsuario)
    {
        bool ok = true;
        switch (ultimaEtiqueta)
        {
            
            case EtiquetaTramite.Resolucion :
                CambiarEstado(EstadoExpediente.ConResolucion, idUsuario);
                break;
            case EtiquetaTramite.PaseAEstudio:
                CambiarEstado(EstadoExpediente.ParaResolver, idUsuario);
                break;
            case EtiquetaTramite.PaseAlArchivo:
                CambiarEstado(EstadoExpediente.Finalizado, idUsuario);
                break;
            case null: 
                CambiarEstado(EstadoExpediente.RecienIniciado, idUsuario);
                break;
            default:
                ok = false;
                break;
        }
        return ok;
        
    }
}
