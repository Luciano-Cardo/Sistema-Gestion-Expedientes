using System;
using SGE.Dominio.Tramites;

namespace SGE.Infraestructura;

public class TramiteTxtRepository
{
    private readonly string _trammiteTxt = "Datos de los tramites";

    public void escribir (Tramite tramite, Boolean ok)
    {
        using var sw = new StreamWriter(_trammiteTxt, ok);
        sw.WriteLine(tramite.Id);
        sw.WriteLine(tramite.ExpedienteId);
        sw.WriteLine(tramite.Etiqueta);
        sw.WriteLine(tramite.Contenido);
        sw.WriteLine(tramite.FechaCreacion);
        sw.WriteLine(tramite.FechaUltimaModificacion);
        sw.WriteLine(tramite.UsuarioUltimoCambio);
        sw.Close();
    }

    public void agregar(Tramite tramite)
    {
        escribir(tramite, true);
    }

    public void SobreEscribir(List<Tramite> tramites)
    {
        try
        {
            foreach(Tramite tram in tramites)
            {
                escribir(tram, false);
            }
        }
        catch(Exception e)
        {
            throw new Exception("Error a sobreescribir el archivo");
        }
    }

    public void eliminar(Guid id)
    {
        List<Tramite> tramites = (List<Tramite>)ObtenerTodos();
        Tramite? tramiteABorrar = null;

        foreach(Tramite t in tramites)
        {
            if (t.Id == id)
            {
                tramiteABorrar = t;
                break;
            }
        }

        if (tramiteABorrar == null)
        {
            throw new Exception("No se encontro el tramite para eliminar");
        }

        tramites.Remove(tramiteABorrar);
        SobreEscribirArchivo(tramites);
    }

    public void SobreEscribirArchivo(List<Tramite> tramites)
    {
        try
        {
            foreach(Tramite t in tramites)
            {
                escribir(t, false);
            }
        }
        catch(Exception e)
        {
            throw new Exception("Error a sobreescribir el tramite");
        } 
    }

    public void Modificar(Tramite tramite)
    {
        List<Tramite> tramites = (List<Tramite>)ObtenerTodos();
    }

    public IEnumerable<Tramite> ObtenerTodos()
    {
        
    }
}
