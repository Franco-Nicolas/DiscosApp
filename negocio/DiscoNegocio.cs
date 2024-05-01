using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class DiscoNegocio
    {
        public List<Disco> listar()
        {
            List<Disco> lista = new List<Disco>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;

            try
            {
                //Conexión a la base de datos
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=DISCOS_DB; integrated security= true;";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "Select D.Id, D.Titulo, D.CantidadCanciones, D.FechaLanzamiento, E.Descripcion Estilos, T.Descripcion Edicion, D.UrlImagenTapa, D.IdEstilo, D.IdTipoEdicion From DISCOS D, ESTILOS E, TIPOSEDICION T where D.IdEstilo = E.Id AND D.IdTipoEdicion = T.Id";
                comando.Connection = conexion;
                //lee la tabla
                conexion.Open();
                lector = comando.ExecuteReader();

                //transforma toda la tabla a objetos y devuelve una lista de objetos
                while (lector.Read())
                {
                    Disco aux = new Disco();
                    aux.Id = (int)lector["Id"];
                    aux.Titulo = (string)lector["Titulo"];
                    aux.CantidadCanciones = (int)lector["CantidadCanciones"];
                    aux.FechaLanzamiento = (DateTime)lector["FechaLanzamiento"];
                    //Validacion si es null:
                    if(!(lector["UrlImagenTapa"] is DBNull))
                        aux.UrlImagenTapa = (string)lector["UrlImagenTapa"];
                    aux.Estilo = new Estilo();
                    aux.Estilo.Id = (int)lector["IdEstilo"];
                    aux.Estilo.Descripcion = (string)lector["Estilos"];
                    aux.Edicion = new Edicion();
                    aux.Edicion.Id = (int)lector["IdTipoEdicion"];
                    aux.Edicion.Descripcion = (string)lector["Edicion"];

                    lista.Add(aux);
                }
                conexion.Close();
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void agregar(Disco nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, IdEstilo, IdTipoEdicion) values ('" + nuevo.Titulo + "', @fechaLanzamiento,'" + nuevo.CantidadCanciones + "', @urlImagenTapa ,@idEstilo, @idTipoEdicion)");
                datos.setearParametro("@fechaLanzamiento", nuevo.FechaLanzamiento);
                datos.setearParametro("@urlImagenTapa", nuevo.UrlImagenTapa);
                datos.setearParametro("@idEstilo",nuevo.Estilo.Id);
                datos.setearParametro("@idTipoEdicion", nuevo.Edicion.Id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void modificar(Disco disco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update DISCOS set Titulo = @titulo, FechaLanzamiento = @fechaLanzamiento, CantidadCanciones = @cantidadCanciones, UrlImagenTapa = @urlImagenTapa, IdEstilo = @idEstilo, IdTipoEdicion = @idTipoEdicion where Id = @id");
                datos.setearParametro("@titulo", disco.Titulo);
                datos.setearParametro("@fechaLanzamiento", disco.FechaLanzamiento);
                datos.setearParametro("@cantidadCanciones", disco.CantidadCanciones);
                datos.setearParametro("@urlImagenTapa", disco.UrlImagenTapa);
                datos.setearParametro("@idEstilo", disco.Estilo.Id);
                datos.setearParametro("idTipoEdicion", disco.Edicion.Id);
                datos.setearParametro("@id", disco.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from DISCOS where Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Disco> filtrar(string campo, string criterio, string filtro)
        {
            List<Disco> lista = new List<Disco>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "Select D.Id, D.Titulo, D.CantidadCanciones, D.FechaLanzamiento, E.Descripcion Estilos, T.Descripcion Edicion, D.UrlImagenTapa, D.IdEstilo, D.IdTipoEdicion From DISCOS D, ESTILOS E, TIPOSEDICION T where D.IdEstilo = E.Id AND D.IdTipoEdicion = T.Id and ";

                if (campo == "Cantidad de canciones")
                {
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Cantidad de canciones > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Cantidad de canciones < " + filtro;
                            break;
                        case "Igual a":
                            consulta += "Cantidad de canciones = " + filtro;
                            break;
                    }
                }
                else //if (campo == "Título")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "D.Titulo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "D.Titulo like '%" + filtro + "'";
                            break;
                        case "Contiene":
                            consulta += "D.Titulo like '%" + filtro + "%'";
                            break;
                    }
                }
                
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Disco aux = new Disco();
                    
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Titulo = (string)datos.Lector["Titulo"];
                    aux.CantidadCanciones = (int)datos.Lector["CantidadCanciones"];
                    aux.FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"];
                    
                    //Validacion si es null:
                    if (!(datos.Lector["UrlImagenTapa"] is DBNull))
                        aux.UrlImagenTapa = (string)datos.Lector["UrlImagenTapa"];
                    
                    aux.Estilo = new Estilo();
                    aux.Estilo.Id = (int)datos.Lector["IdEstilo"];
                    aux.Estilo.Descripcion = (string)datos.Lector["Estilos"];
                    aux.Edicion = new Edicion();
                    aux.Edicion.Id = (int)datos.Lector["IdTipoEdicion"];
                    aux.Edicion.Descripcion = (string)datos.Lector["Edicion"];

                    lista.Add(aux);
                }

                return lista;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
