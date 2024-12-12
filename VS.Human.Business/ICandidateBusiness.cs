using Microsoft.AspNetCore.Http;
using VS.Human.Business.Model;
using VS.Human.Item;
using VS.Human.Rep.Model;

namespace VS.Human.Business
{
    public interface ICandidateBusiness
    {
        Task<bool> AddOrUpdate(CandidateAdd item);
        Task<bool> ImportSource(IFormFile item);

        Task<bool> AddCandidateWidthOrder(CandidateOrderAdd item);
        Task<bool> Delete(int id);
        Task<Candidate> GetById(int id);

        Task<BaseList> GetAll(CandidateRequest request);

    }
}
