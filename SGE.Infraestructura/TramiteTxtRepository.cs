using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Tramites;
 
namespace SGE.Infraestructura;
 
public class TramiteTxtRepository : ITramiteRepository
{
    private readonly string _tramiteTxt = "Datos de los tramites";
 
    private void Escribir(Tramite tramite, bool append)
    {
        using var sw = new StreamWriter(_tramiteTxt, append);
        sw.WriteLine(tramite.Id);
        sw.WriteLine(tramite.ExpedienteId);
        sw.WriteLine(tramite.Etiqueta);
        sw.WriteLine(tramite.Contenido.Valor);
        sw.WriteLine(tramite.FechaCreacion);
        sw.WriteLine(tramite.FechaUltimaModificacion);
        sw.WriteLine(tramite.UsuarioUltimoCambio);
    }
 
    private void SobreEscribir(List<Tramite> tramites)
    {
        File.WriteAllText(_tramiteTxt, string.Empty);
        foreach (var t in tramites)
            Escribir(t, true);
    }
 
    public void Agregar(Tramite tramite)
    {
        Escribir(tramite, true);
    }
 
    public void Eliminar(Guid id)
    {
        var tramites = ObtenerTodos().ToList();
        var aEliminar = tramites.FirstOrDefault(t => t.Id == id);
 
        if (aEliminar == null)
            throw new RepositorioException($"No se encontró el trámite con ID {id} para eliminar.");
 
        tramites.Remove(aEliminar);
        SobreEscribir(tramites);
    }
 
    public void Modificar(Tramite tramite)
    {
        var tramites = ObtenerTodos().ToList();
        int index = tramites.FindIndex(t => t.Id == tramite.Id);
 
        if (index == -1)
            throw new RepositorioException($"No se encontró el trámite con ID {tramite.Id} para modificar.");
 
        tramites[index] = tramite;
        SobreEscribir(tramites);
    }
 
    public Tramite? ObtenerPorId(Guid id)
    {
        return ObtenerTodos().FirstOrDefault(t => t.Id == id);
    }
 
    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return ObtenerTodos().Where(t => t.ExpedienteId == expedienteId);
    }
 
    public IEnumerable<Tramite> ObtenerTodos()
    {
        var resultado = new List<Tramite>();
 
        if (!File.Exists(_tramiteTxt))
            return resultado;
 
        try
        {
            using var sr = new StreamReader(_tramiteTxt);
            while (!sr.EndOfStream)
            {
                var id           = Guid.Parse(sr.ReadLine() ?? "");
                var expedienteId = Guid.Parse(sr.ReadLine() ?? "");
                var etiqueta     = Enum.Parse<EtiquetaTramite>(sr.ReadLine() ?? "");
                var contenidoStr = sr.ReadLine() ?? "";
                var fechaCreacion = DateTime.Parse(sr.ReadLine() ?? "");
                var fechaModif   = DateTime.Parse(sr.ReadLine() ?? "");
                var usuario      = Guid.Parse(sr.ReadLine() ?? "");
 
                var contenido = new ContenidoTramite(contenidoStr);
                resultado.Add(Tramite.Reconstruir(id, expedienteId, etiqueta, contenido, fechaCreacion, fechaModif, usuario));
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error al leer los trámites: {e.Message}");
        }
 
        return resultado;
    }
}