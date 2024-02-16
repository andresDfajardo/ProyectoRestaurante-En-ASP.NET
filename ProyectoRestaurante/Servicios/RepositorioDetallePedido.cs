using Dapper;
using Microsoft.Data.SqlClient;
using ProyectoRestaurante.Models;

namespace ProyectoRestaurante.Servicios
{
    public interface IRepositorioDetallePedido
    {
        Task Borrar(int id_detalle);
        Task CrearDetalle(DetallePedidos detallePedidos);
        Task<IEnumerable<DetallePedidosCreacionViewModel>> ObtenerPlatos(int id_pedido);
        Task<DetallePedidos> ObtenerporId(int id_detalle);
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
                return await connection.QueryAsync<DetallePedidosCreacionViewModel>(@"SELECT dp.id_detalle,dp.id_pedido,c.nombre_plato as NombrePlato, sum(dp.cantidad) as cantidad, sum(dp.cantidad * c.precio_plato) as Subtotal
                                                                            FROM DetallePedido dp
                                                                            JOIN Carta as c
                                                                            ON dp.id_carta = c.id
                                                                            WHERE dp.id_pedido = @id_pedido
                                                                            GROUP BY dp.id_pedido,c.nombre_plato,dp.id_detalle", new { id_pedido });
        }

        public async Task Borrar(int id_detalle)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE DetallePedido WHERE id_detalle = @id_detalle", new { id_detalle });
        }
        public async Task<DetallePedidos>ObtenerporId(int id_detalle)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<DetallePedidos>(@"SELECT id_detalle,id_pedido,id_carta,cantidad
                                                                            FROM DetallePedido
                                                                            WHERE id_detalle = @id_detalle", new {id_detalle});
        } 

    }
}
