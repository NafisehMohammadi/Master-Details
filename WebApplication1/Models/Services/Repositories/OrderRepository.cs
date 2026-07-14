using MasterDetail.Repositories.Contract;
using MasterDetail.Repositories.Mappers;
using MasterDetails.Models;
using MasterDetails.Models.DomainModels.OrderHeaderAggregates;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MasterDetail.Repositories.Repository;


public class OrderRepository : IOrderRepository
{

    #region [- Private Fields() -]
    private readonly ProjectDbContext _projectDbContext;
    #endregion

    #region [- Ctor() -]
    public OrderRepository(ProjectDbContext context)
    {
        _projectDbContext = context;
    }
    #endregion

    #region [- CreateAsync() -] 
    public async Task CreateAsync(OrderHeader order)
    {
        await using var connection =
            _projectDbContext.Database.GetDbConnection();


        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();



        await using var command =
            connection.CreateCommand();



        command.CommandText =
            "dbo.sp_Order_Create";


        command.CommandType =
            CommandType.StoredProcedure;



        command.Parameters.Add(
            new SqlParameter(
                "@GuidKey",
                order.GuidKey));



        command.Parameters.Add(
            new SqlParameter(
                "@OrderNumber",
                order.OrderNumber));



        command.Parameters.Add(
            new SqlParameter(
                "@CustomerId",
                order.CustomerId));



        command.Parameters.Add(
            new SqlParameter(
                "@OrderDate",
                order.OrderDate));



        command.Parameters.Add(
            new SqlParameter(
                "@Description",
                (object?)order.Description
                ?? DBNull.Value));



        command.Parameters.Add(
            new SqlParameter(
                "@TotalAmount",
                order.TotalAmount));



        // =============================
        // TVP Details
        // =============================

        var table = new DataTable();


        table.Columns.Add(
            "GuidKey",
            typeof(Guid));


        table.Columns.Add(
            "ProductId",
            typeof(Guid));


        table.Columns.Add(
            "Quantity",
            typeof(decimal));


        table.Columns.Add(
            "UnitPrice",
            typeof(decimal));


        table.Columns.Add(
            "LineTotal",
            typeof(decimal));



        foreach (var detail in order.Details)
        {
            table.Rows.Add(

                detail.GuidKey,

                detail.ProductId,

                detail.Quantity,

                detail.UnitPrice,

                detail.LineTotal
            );
        }



        var detailsParameter =
            new SqlParameter("@Details", table)
            {
                SqlDbType =
                    SqlDbType.Structured,

                TypeName = "dbo.OrderDetailCreateType"
            };


        command.Parameters.Add(detailsParameter);



        await command.ExecuteNonQueryAsync();

    }

    #endregion

    #region [- UpdateAsync() -]  
    public async Task UpdateAsync(OrderHeader order)
    {
        await using var connection =
            _projectDbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();
        await using var command =
            connection.CreateCommand();

        command.CommandText = "dbo.sp_Order_Update";
        command.CommandType =
            CommandType.StoredProcedure;

       command.Parameters.Add(
            new SqlParameter("@Id", order.Id));

        command.Parameters.Add(
          new SqlParameter( "@GuidKey",order.GuidKey));

        command.Parameters.Add(
            new SqlParameter("@CustomerId", order.CustomerId));

        command.Parameters.Add(
            new SqlParameter("@OrderDate", order.OrderDate));

        command.Parameters.Add(
            new SqlParameter( "@Description",
                (object?)order.Description ?? DBNull.Value));

        command.Parameters.Add(  
            new SqlParameter("@TotalAmount", order.TotalAmount));
        // =========================
        // TVP Details
        // =========================

        var table = new DataTable();


        table.Columns.Add("Id",typeof(Guid));

        table.Columns.Add("GuidKey", typeof(Guid));

        table.Columns.Add("ProductId", typeof(Guid));


        table.Columns.Add("Quantity", typeof(decimal));


        table.Columns.Add("UnitPrice",typeof(decimal));


        table.Columns.Add("LineTotal", typeof(decimal));

        foreach (var detail in order.Details)
        {
            table.Rows.Add(
                detail.Id == Guid.Empty
                ? DBNull.Value
                : detail.Id,
                detail.GuidKey,
                detail.ProductId,
                detail.Quantity,
                detail.UnitPrice,
                detail.LineTotal
            );
        }

        var detailsParameter =
            new SqlParameter("@Details", table)
            {
                SqlDbType = SqlDbType.Structured,

                TypeName = "dbo.OrderDetailUpdateType"
            };



        command.Parameters.Add(detailsParameter);



        await command.ExecuteNonQueryAsync();
    }

    #endregion

    #region [- DeleteAsync() -]
    public async Task DeleteAsync(OrderHeader order)
    {
        await using var connection =
          _projectDbContext.Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        await using var command = connection.CreateCommand();

        command.CommandText = "dbo.sp_Order_Delete";
        command.CommandType = CommandType.StoredProcedure;

        var orderdelete = new OrderHeader
        {
            Id = order.Id,

        };

        command.Parameters.Add(new SqlParameter("@Id", orderdelete.Id));

        await command.ExecuteNonQueryAsync();
    }

    /*public async Task DeleteAsync(Guid id)
    {
        await using var transaction =
            await _projectDbContext.Database
            .BeginTransactionAsync();

        try
        {

            var order =
                await _projectDbContext.OrderHeaders
                .FirstOrDefaultAsync(x => x.Id == id);



            if (order == null)
                throw new Exception("Order not found");



            // Soft Delete Header

            order.IsDeleted = true;

            order.ModifiedDate = DateTime.Now;



            await _projectDbContext.SaveChangesAsync();



            // Soft Delete Details با SP

            await DeleteDetailsBySp(id);



            await transaction.CommitAsync();

        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private async Task DeleteDetailsBySp(Guid orderId)
    {
        await using var connection =
            _projectDbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();
        await using var command =
            connection.CreateCommand();
        command.CommandText =
            "dbo.sp_OrderDetail_Delete";

        command.CommandType =
            CommandType.StoredProcedure;
        command.Parameters.Add(
            new SqlParameter(
                "@OrderHeaderId",
                orderId));
        await command.ExecuteNonQueryAsync();
    }
    */
    #endregion

    #region [- GetByIdAsync() -]
   public async Task<OrderHeader?> GetByIdAsync(Guid id)
    {
        return await _projectDbContext.OrderHeaders

          .AsNoTracking()

          .Where(x =>
              x.Id == id &&
              !x.IsDeleted)

          .Include(x => x.Customer)

          .Include(x => x.Details
              .Where(d => !d.IsDeleted))

              .ThenInclude(d => d.Product)

          .FirstOrDefaultAsync();
    }
 /*  public async Task<OrderHeader?> GetByIdAsync(Guid orderId)
      {
          await using var connection =
              _projectDbContext.Database.GetDbConnection();

          if (connection.State != ConnectionState.Open)
              await connection.OpenAsync();

          await using var command =
              connection.CreateCommand();

          command.CommandText = "sp_Order_GetById";

          command.CommandType = CommandType.StoredProcedure;

          command.Parameters.Add(
               new SqlParameter("@Id", orderId));

          await using var reader =
              await command.ExecuteReaderAsync();

          return OrderDataReaderMapper.MapOrder(reader);
      }*/
    #endregion
   

    #region [- GetAllAsync() -]
    public async Task<List<OrderHeader>> GetAllAsync()
    {
        return await _projectDbContext.OrderHeaders

                .AsNoTracking()

                .Where(x => !x.IsDeleted)

                .Include(x => x.Customer)

                .OrderByDescending(x => x.OrderDate)

                .ToListAsync();
    }

 

    /*  public async Task<List<OrderHeader>> GetAllAsync()
      {
          await using var connection = _projectDbContext.Database.GetDbConnection();

          if (connection.State != ConnectionState.Open)
              await connection.OpenAsync();

          await using var command = connection.CreateCommand();

          command.CommandText = "sp_Order_GetAll";
          command.CommandType = CommandType.StoredProcedure;

          await using var reader = await command.ExecuteReaderAsync();

          var result = OrderListDataReaderMapper.Map(reader);

          return result ?? new List<OrderHeader>();
      }
    */
    #endregion

}