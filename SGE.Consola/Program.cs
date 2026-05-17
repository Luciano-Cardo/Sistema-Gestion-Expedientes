using SGE.Aplicacion;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Aplicacion.Servicios;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Comun;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using SGE.Infraestructura;
 
//COMPOSITION ROOT
IExpedienteRepository expedienteRepo= new ExpedienteTxtRepository();
ITramiteRepository tramiteRepo= new TramiteTxtRepository();
IAutorizacionService autorizacion= new AutorizacionProvisionalService();
 
var servicioEstado= new ActualizacionEstadoExpedienteService(expedienteRepo, tramiteRepo);
var agregarExpediente= new AgregarExpedienteUseCase(expedienteRepo, autorizacion);
var eliminarExpediente= new EliminarExpedienteUseCase(expedienteRepo, tramiteRepo, autorizacion);
var modificarCaratula= new ModificarCaratulaExpedienteUseCase(expedienteRepo, autorizacion);
var cambiarEstado= new CambiarEstadoExpedienteUseCase(expedienteRepo, autorizacion);
var listarExpedientes= new ListarExpedientesUseCase(expedienteRepo);
var agregarTramite= new AgregarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacion, servicioEstado);
var eliminarTramite= new EliminarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacion, servicioEstado);
var modificarTramite= new ModificarTramiteUseCase(tramiteRepo, expedienteRepo, autorizacion, servicioEstado);
var listarTramites= new ListarTramitesPorExpedienteUseCase(tramiteRepo);
 
Guid usuarioId = Guid.NewGuid();
 
//CAMINO FELIZ :)
Console.WriteLine("Sistema De Gestion De Expedientes");
Console.WriteLine("--Alta de expedientes--");

Guid expId1 = Guid.Empty;
Guid expId2 = Guid.Empty;
 
try
{
    var r1= agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente sobre construcción ilegal", usuarioId));
    expId1= r1.id;
    Console.WriteLine($"Expediente 1 creado. ID: {expId1}");
 
    var r2 = agregarExpediente.Ejecutar(new AgregarExpedienteRequest("Expediente de habilitación comercial", usuarioId));
    expId2 = r2.id;
    Console.WriteLine($"Expediente 2 creado. ID: {expId2}");
}
catch (DominioException ex)      
    { Console.WriteLine($"[DominioException] {ex.Message}"); }
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
Console.WriteLine("--Listado de expedientes--");
try
{
    foreach (var exp in listarExpedientes.Ejecutar(new ListarExpedientesRequest()))
        Console.WriteLine($"[{exp.Estado}] {exp.Caratula.Valor}");
}
catch (Exception ex) 
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
Console.WriteLine("--Alta de tramites--");
 
Guid tramiteId1 = Guid.Empty;
Guid tramiteId2 = Guid.Empty;
 
try
{
    var rt1 = agregarTramite.Ejecutar(new AgregarTramiteRequest
        (usuarioId, expId1, EtiquetaTramite.EscritoPresentado,new ContenidoTramite("Se presenta el escrito inicial")));
    tramiteId1 = rt1.Id;
    Console.WriteLine($"Tramite 'EscritoPresentado' agregado. ID: {tramiteId1}");
 
    var rt2 = agregarTramite.Ejecutar(new AgregarTramiteRequest
        (usuarioId, expId1, EtiquetaTramite.PaseAEstudio,new ContenidoTramite("Pase a estudio del departamento técnico")));
    tramiteId2 = rt2.Id;
    Console.WriteLine($"Tramite 'PaseAEstudio' agregado. El expediente debería pasar a 'ParaResolver'");
}
catch (DominioException ex)      
    { Console.WriteLine($"[DominioException] {ex.Message}"); }
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
//Verificar cambio de estado automatico
Console.WriteLine("--Estado de expedientes tras agregar trámites--");
try
{
    foreach (var exp in listarExpedientes.Ejecutar(new ListarExpedientesRequest()))
        Console.WriteLine($"[{exp.Estado}] {exp.Caratula.Valor}");
}
catch (Exception ex) 
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Listar tramites del expediente 1
Console.WriteLine("--Trámites del expediente 1");
try
{
    foreach (var t in listarTramites.Ejecutar(new ListarTramitesPorExpedienteRequest(expId1)))
        Console.WriteLine($"[{t.Etiqueta}] {t.Contenido.Valor}");
}
catch (Exception ex) 
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Agregar tramite Resolucion(pasa a con resolucion)
Console.WriteLine("--Agregar tramite 'Resolucion'--");
try
{
    agregarTramite.Ejecutar
        (new AgregarTramiteRequest(usuarioId, expId1, EtiquetaTramite.Resolucion,new ContenidoTramite("Se emite resolución favorable")));
    Console.WriteLine("Tramite 'Resolución' agregado. El expediente debería pasar a 'ConResolucion'");
}
catch (Exception ex) { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Cambio de estado manual
Console.WriteLine("--Cambio de estado manual a 'EnNotificacion'--");
try
{
    cambiarEstado.Ejecutar(new CambiarEstadoExpedienteRequest(expId1, EstadoExpediente.EnNotificacion, usuarioId));
    Console.WriteLine("Estado cambiado manualmente a 'EnNotificacion'");
}
catch (EntidadNoEncontradaException ex) 
    { Console.WriteLine($"[EntidadNoEncontradaException] {ex.Message}"); }
catch (AutorizacionException ex)        
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)                    
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Modificar caratula
Console.WriteLine("--Modificar caratula del expediente 1--");
try
{
    modificarCaratula.Ejecutar(new ModificarCaratulaExpedienteRequest(expId1, "Expediente sobre construcción ilegal - CORREGIDO", usuarioId));
    Console.WriteLine("Caratula modificada correctamente");
}
catch (DominioException ex)      
    { Console.WriteLine($"[DominioException] {ex.Message}"); }
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Modificar contenido de un tramite
Console.WriteLine("--Modificar contenido de un tramite--");
try
{
    modificarTramite.Ejecutar
        (new ModificarTramiteRequest(tramiteId1, new ContenidoTramite("Escrito inicial corregido con documentación adicional"), usuarioId));
    Console.WriteLine("Tramite modificado correctamente");
}
catch (DominioException ex)      
    { Console.WriteLine($"[DominioException] {ex.Message}"); }
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Eliminar un tramite
Console.WriteLine("--Eliminar tramite--");
try
{
    eliminarTramite.Ejecutar(new EliminarTramiteRequest(usuarioId, tramiteId1));
    Console.WriteLine($"Trámite {tramiteId1} eliminado");
}
catch (EntidadNoEncontradaException ex) 
    { Console.WriteLine($"[EntidadNoEncontradaException] {ex.Message}"); }
catch (AutorizacionException ex)        
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)                    
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Eliminar expediente en cascada
Console.WriteLine("--Eliminar expediente 2 (con sus tramites)--");
try
{
    eliminarExpediente.Ejecutar(new EliminarExpedienteRequest(expId2, usuarioId));
    Console.WriteLine("Expediente 2 eliminado junto con todos sus tramites");
}
catch (EntidadNoEncontradaException ex) 
    { Console.WriteLine($"[EntidadNoEncontradaException] {ex.Message}"); }
catch (AutorizacionException ex)        
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)                    
    { Console.WriteLine($"[Error] {ex.Message}"); }
 
 
//Listado final
Console.WriteLine("--Listado final de expedientes--");
try
{
    foreach (var exp in listarExpedientes.Ejecutar(new ListarExpedientesRequest()))
        Console.WriteLine($"[{exp.Estado}] {exp.Caratula.Valor}");
}
catch (Exception ex) { Console.WriteLine($"[Error] {ex.Message}"); }

//Caminos de error :( 
Console.WriteLine("--Caminos de error--");
 
Console.WriteLine("--Error 1: caratula vacia--");
try
{
    agregarExpediente.Ejecutar(new AgregarExpedienteRequest("", usuarioId));
}
catch (DominioException ex)      
    { Console.WriteLine($"[DominioException] {ex.Message}"); }
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error inesperado] {ex.Message}"); }
 
Console.WriteLine("--Error 2: contenido de tramite vacio--");
try
{
    agregarTramite.Ejecutar(new AgregarTramiteRequest(usuarioId, expId1, EtiquetaTramite.Despacho, new ContenidoTramite("")));
}
catch (DominioException ex)     
    { Console.WriteLine($"[DominioException] {ex.Message}"); }
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error inesperado] {ex.Message}"); }
 
Console.WriteLine("--Error 3: sin autorizacion--");
IAutorizacionService sinPermisos  = new AutorizacionSiempreDenegadaService();
var agregarSinPermiso = new AgregarExpedienteUseCase(expedienteRepo, sinPermisos);
try
{
    agregarSinPermiso.Ejecutar(new AgregarExpedienteRequest("No deberia crearse", usuarioId));
}
catch (AutorizacionException ex) 
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)             
    { Console.WriteLine($"[Error inesperado] {ex.Message}"); }
 
Console.WriteLine("--Error 4: expediente que no existe--");
try
{
    cambiarEstado.Ejecutar(new CambiarEstadoExpedienteRequest(Guid.NewGuid(), EstadoExpediente.Finalizado, usuarioId));
}
catch (EntidadNoEncontradaException ex) 
    { Console.WriteLine($"[EntidadNoEncontradaException] {ex.Message}"); }
catch (AutorizacionException ex)        
    { Console.WriteLine($"[AutorizacionException] {ex.Message}"); }
catch (Exception ex)                    
    { Console.WriteLine($"[Error inesperado] {ex.Message}"); }
 
//Clase auxiliar solo para probar el camino de error 3
public class AutorizacionSiempreDenegadaService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso) => false;
}