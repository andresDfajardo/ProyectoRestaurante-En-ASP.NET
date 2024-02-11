using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioTiposPlatos //Interfaz de contiene los metodos y se enviaran al controlador
    {
        Task Actualizar(TiposPlatos tiposPlatos);
        Task Borrar(int id_categoria);
        Task Crear(TiposPlatos tiposPlatos);
        Task<bool> Existe(string categoria);
        Task<IEnumerable<TiposPlatos>> Obtener();
        Task<TiposPlatos> ObtenerporId(int id_categoria);
    }
    public class RepositorioTiposPlatos:IRepositorioTiposPlatos //Clase que hereda de la interfaz
    {
        private readonly string connectionString;//Variable que contendrá la conexión a la base de datos
        public RepositorioTiposPlatos(IConfiguration configuration)//Metodo que configura la variable connectionString con la informacion que tiene el DefaultConnection
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Crear(TiposPlatos tiposPlatos)//Metodo para Crear un tipo de plato que recibe como parámetro todo el formulario
        {
            //Variable que crea una instancia de SqlConnection y envía de parámetro la variable de conexion
            var connection = new SqlConnection(connectionString);
            //Variable que es igual al valor que retorna la conexión al ejecutar la sentencia de SQL
            //QuerySingleAsync permite recibir un valor de la sentencia sql en este caso es un entero
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO CategoriaPlato(categoria)
                                                        VALUES (@categoria);
                                                        SELECT SCOPE_IDENTITY();",tiposPlatos);
            //A la propiedad que creamos en el Modelo le asignamos el valor que tenemos en la variable id
            tiposPlatos.id_categoria = id;

        }

        public async Task <bool> Existe (string categoria)//Metodo que verifica si existe o no el registro segun la categoria que se envia del formulario
        {
            //Variable que crea una instancia de SqlConnection y envía de parámetro la variable de conexion
            using var connection = new SqlConnection(connectionString);
            //Variable que es igual al valor que retorna la conexion y el QueryFirstOrDefaultAsync permite recibir el primer valor entero o el valor por defecto que retorne la sentencia SQL
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 
                                                                        FROM CategoriaPlato
                                                                        WHERE categoria = @categoria", new {categoria});
            //Como lo que retorna el metodo es un booleano, retornamos el resultado de comparacion entre la variable existe y el numero 1
            //Si existe es igual a 1, retorna un true
            //Si existe es igual a 0, retorna un false al controlador
            return existe == 1;
        }
        public async Task<IEnumerable<TiposPlatos>> Obtener() // Metodo de tipo IEnumerable que devuelve lo que retorna la sentencia SQL
        {
            //Variable que crea una instancia de SqlConnection y envía de parámetro la variable de conexion
            using var connection = new SqlConnection(connectionString);
            //QueryAsync permite retornar varios datos de una sentencia sql
            return await connection.QueryAsync<TiposPlatos>(@"SELECT id_categoria,categoria
                                                              FROM CategoriaPlato");
        }
        public async Task Actualizar(TiposPlatos tiposPlatos) //Metodo que permite ejecutar una sentencia sin devolver ningun valor
        {
            //Variable que crea una instancia de SqlConnection y envía de parámetro la variable de conexion
            using var connection = new SqlConnection(connectionString);
            //ExecuteAsync ejecuta la ccion pero no retorna ningun valor
            await connection.ExecuteAsync(@"UPDATE CategoriaPlato
                                            SET categoria = @categoria
                                            WHERE id_categoria = @id_categoria", tiposPlatos);
        }
        public async Task<TiposPlatos> ObtenerporId(int id_categoria)
        {
            //Variable que crea una instancia de SqlConnection y envía de parámetro la variable de conexion
            using var connection = new SqlConnection(connectionString);
            //Variable que es igual al valor que retorna la conexion y el QueryFirstOrDefaultAsync permite recibir el primer valor entero o el valor por defecto que retorne la sentencia SQL
            return await connection.QueryFirstOrDefaultAsync<TiposPlatos>(@"SELECT id_categoria,categoria
                                                                    FROM CategoriaPlato
                                                                    WHERE id_categoria=@id_categoria",
                                                                    new { id_categoria });
        }

        public async Task Borrar(int id_categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE CategoriaPlato WHERE id_categoria = @id_categoria", new { id_categoria });
        }
    }
}
