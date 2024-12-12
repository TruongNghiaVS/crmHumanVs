﻿using VS.Human.Item;
using VS.Human.Rep.Model;

namespace VS.Human.Rep
{
    public interface IMasterDataRep
    {
        Task<bool> AddOrUpdate(MasterData item);

        Task<bool> Delete(int id);

        Task<MasterData> GetById(int id);



        Task<BaseList> GetAll(CommonRequest request);



    }
}
