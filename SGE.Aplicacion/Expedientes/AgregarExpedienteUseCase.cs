using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Aplicacion.Autorizacion;
namespace SGE.Aplicacion.Expedientes;

public class AgregarExpedienteUseCase
{
    private readonly IExpedienteRepository _repo;
    private readonly IAutorizacionService _autorizacion;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public AgregarExpedienteUseCase(IExpedienteRepository repo, IAutorizacionService autorizacion,IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repo = repo;
        _autorizacion = autorizacion;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request)
    {
        if (!_autorizacion.PoseeElPermiso(request.UsuarioUltimoCambio, Permiso.ExpedienteAlta)) 
            throw new AutorizacionException("El usuario no posee la autorización para crear expedientes");
        
        var caratula = new Caratula(request.Caratula);

        var nuevoExpediente = new Expediente(caratula,request.UsuarioUltimoCambio); 

        _repo.Agregar(nuevoExpediente);
        _unidadDeTrabajo.Guardar();

        return new AgregarExpedienteResponse(nuevoExpediente.Id);
    }
}