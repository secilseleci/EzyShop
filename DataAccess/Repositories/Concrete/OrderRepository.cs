﻿using DataAccess.Repositories.Abstract;
using Models.Entities.Concrete;

namespace DataAccess.Repositories.Concrete
{
    public class OrderRepository(ApplicationDbContext context) : BaseRepository<Order>(context), IOrderRepository
    {
        
    }
}
