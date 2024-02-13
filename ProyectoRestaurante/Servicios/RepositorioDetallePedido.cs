using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioDetallePedido
    {
        Task CrearDetalle(DetallePedidos detallePedidos);
        Task<DetallePedidos> ObtenerPlatos(int id_pedido);
    }
    public class RepositorioDetallePedido:IRepositorioDetallePedido
    {
        private readonly string connectionString;
        public RepositorioDetallePedido(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CrearDetalle(DetallePedidos detallePedidos)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"Insertar_Detalle", new {detallePedidos.id_carta,detallePedidos.cantidad},
                commandType: System.Data.CommandType.StoredProcedure);
            detallePedidos.id_detalle = id;
        }
        public async Task<DetallePedidos> ObtenerPlatos(int id_pedido)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<DetallePedidos>(@"SELECT c.nombre_plato as NombrePlato, cantidad
                                                                            FROM DetallePedido
                                                                            JOIN Carta as c
                                                                            ON DetallePedido.id_carta = c.id
                                                                            WHERE id_pedido = @id_pedido", new {id_pedido});
        }
        

    }
}
