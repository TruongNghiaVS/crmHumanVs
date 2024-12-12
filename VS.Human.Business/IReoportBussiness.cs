using VS.Human.Item;

namespace VS.Human.Business
{
    public interface IReoportBussiness
    {



        Task<BaseList> GetAllImpact(OrderRequest request);
        Task<BaseList> GetAllOrderStatus(OrderRequest request);

    }
}
