using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioDetallePedido
    {
        Task CrearDetalle(DetallePedidos detallePedidos);
        Task<IEnumerable<DetallePedidosCreacionViewModel>> ObtenerPlatos(int id_pedido);
    }
    public class RepositorioDetallePedido : IRepositorioDetallePedido
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
        public async Task<IEnumerable<DetallePedidosCreacionViewModel>> ObtenerPlatos(int id_pedido)
        {
                using var connection = new SqlConnection(connectionString);
                return await connection.QueryAsync<DetallePedidosCreacionViewModel>(@"SELECT dp.id_pedido,c.nombre_plato as NombrePlato, sum(dp.cantidad) as cantidad, sum(dp.cantidad * c.precio_plato) as Subtotal
                                                                            FROM DetallePedido dp
                                                                            JOIN Carta as c
                                                                            ON dp.id_carta = c.id
                                                                            WHERE dp.id_pedido = @id_pedido
                                                                            GROUP BY c.nombre_plato", new { id_pedido });
        }
        

    }
}
