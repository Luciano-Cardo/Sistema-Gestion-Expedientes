using System;
using System.IO;
using System.Security.AccessControl;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Interfaces;
using SGE.Dominio.Expedientes;
namespace SGE.Infraestructura;

public class ExpedienteTxtRepository : IExpedienteRepository
{
    private readonly string _expedienteTxt = "Datos de los expedientes";

    private void escribir(Expediente exp, Boolean ok)
    {
        using var sw = new StreamWriter(_expedienteTxt,ok);
        sw.WriteLine(exp.Id);
        sw.WriteLine(exp.Caratula.Valor);
        sw.WriteLine(exp.FechaCreacion);
        sw.WriteLine(exp.FechaUltimaModificacion);
        sw.WriteLine(exp.UsuarioUltimoCambio);
        sw.WriteLine(exp.Estado);
        sw.Close();
    }

    public void Agregar(Expediente expediente)
    {
        escribir(expediente, true);
    }


    public void SobreEscribir(List<Expediente> expedientes)
    {
        try
        {            
            foreach(Expediente exp in expedientes)
            {
                escribir (exp, false);   
            }
        }
        catch(Exception e)
        {
            throw new Exception("Error a sobreescribir el archivo");
        }
    }
    public void Eliminar(Guid id)
    {
        List<Expediente> expedientes = (List<Expediente>)ObtenerTodos();

        Expediente? expedienteABorrar = null;

        foreach(Expediente e in expedientes)
        {
            if(e.Id == id)
            {
                expedienteABorrar = e;
                break;
            }
        }

        if(expedienteABorrar == null)
        {
            throw new Exception("No se encontro el expediente para eliminar");
        }

        expedientes.Remove(expedienteABorrar);
        SobreEscribir(expedientes);   
    }
    
    public void Modificar(Expediente expediente)
    {
        List<Expediente> expedientes = (List<Expediente>)ObtenerTodos();

        Expediente? expedienteModificar = null;

        foreach (Expediente e in expedientes)
        {
            if(e.Id == expediente.Id)
            {
                expedienteModificar = e;
            }

        }

        if(expedienteModificar == null)
        {
            throw new Exception("No se encontro el expediente para modificar");
        }

        expediente.ModificarCaratula(expedienteModificar.Caratula,expedienteModificar.Id);
        SobreEscribir(expedientes);
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        List<Expediente> expedientes = (List<Expediente>)ObtenerTodos();
        Expediente? obtenerExpediente = null;
        foreach (Expediente e in expedientes)
        {
            if(e.Id == id)
            {
                obtenerExpediente = e;
            }
        }
        if(obtenerExpediente == null)
        {
            throw new Exception("No se encontro un ID con ese expediente");
        }
        return obtenerExpediente;
    }

        public IEnumerable<Expediente> ObtenerTodos()
    {
        List<Expediente> resultado = new List<Expediente>();

        if (!File.Exists(_expedienteTxt))
        {
            return resultado;
        }

        try 
        {
            using StreamReader sr = new StreamReader(_expedienteTxt);

            while (!sr.EndOfStream)
            {
                string idStr = sr.ReadLine() ?? "";               
                string caratulaStr = sr.ReadLine() ?? "";          
                string fechaCreacionStr = sr.ReadLine() ?? "";
                string fechaModifStr = sr.ReadLine() ?? "";
                string usuarioStr = sr.ReadLine() ?? "";
                string estadoStr = sr.ReadLine() ?? "";

                Guid id = Guid.Parse(idStr);
                Caratula caratula = new Caratula(caratulaStr); // Usamos tu Value Object
                DateTime fechaCreacion = DateTime.Parse(fechaCreacionStr);
                DateTime fechaModif = DateTime.Parse(fechaModifStr);
                Guid usuario = Guid.Parse(usuarioStr);
                EstadoExpediente estado = Enum.Parse<EstadoExpediente>(estadoStr);

                Expediente expediente = Expediente.Reconstruir(id, caratula, fechaCreacion, fechaModif, usuario, estado);
                
                resultado.Add(expediente);
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Error al intentar leer los expedientes: {e.Message}");
        }

        return resultado;
    }
}
