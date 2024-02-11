using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioCarta
    {
        Task Actualizar(Carta carta);
        Task Borrar(int id);
        Task Crear(Carta carta);
        Task<bool> Existe(string nombre_plato, int tipo_plato, int precio_plato, string descripcion);
        Task<IEnumerable<Carta>> Obtener();
        Task<Carta> ObtenerporId(int id);
    }
    public class RepositorioCarta:IRepositorioCarta
    {
        private readonly string connectionString;//Variable que contendrá la conexión a la base de datos
        public RepositorioCarta(IConfiguration configuration)//Metodo que configura la variable connectionString con la informacion que tiene el DefaultConnection
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear (Carta carta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Carta(nombre_plato,tipo_plato,precio_plato,descripcion)
                                                            VALUES(@nombre_plato,@tipo_plato,@precio_plato,@descripcion)
                                                            SELECT SCOPE_IDENTITY();",carta);
            carta.id = id;
        }
        public async Task<bool> Existe(string nombre_plato, int tipo_plato, int precio_plato, string descripcion)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1
                                                                        FROM Carta
                                                                        WHERE nombre_plato = @nombre_plato
                                                                        AND tipo_plato = @tipo_plato
                                                                        AND precio_plato = @precio_plato
                                                                        AND descripcion = @descripcion
                                                                        ",new {nombre_plato,tipo_plato,precio_plato,descripcion});
            return existe == 1;
        }
        public async Task<IEnumerable<Carta>> Obtener()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Carta>(@"SELECT id,nombre_plato,CategoriaPlato.categoria as tipoPlato,precio_plato,descripcion
                                                        FROM Carta
                                                        INNER JOIN CategoriaPlato
                                                        ON CategoriaPlato.id_categoria = Carta.tipo_plato");
        }
        public async Task Actualizar(Carta carta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Carta
                                            SET nombre_plato = @nombre_plato,
	                                            tipo_plato = @tipo_plato,
	                                            precio_plato = @precio_plato,
	                                            descripcion = @descripcion
                                            WHERE id = @id", carta);
        }
        public async Task<Carta>ObtenerporId(int id)
        {
            using var connection = new SqlConnection( connectionString);
            return await connection.QueryFirstOrDefaultAsync<Carta>(@"SELECT  id,
		                                                        nombre_plato,
		                                                        tipo_plato,
		                                                        precio_plato,
		                                                        descripcion
                                                                FROM Carta
                                                                WHERE id = @id",
                                                                new {id});
        }
        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Carta WHERE id = @id", new { id });
        }
    }
}
