using System.Collections.Generic;

namespace Checkout.Infra.Interfaces
{
    public interface ICRUD<Entity> where Entity : class
    {
        IEnumerable<Entity> GetAll();
        Entity GetById(int Id);
        void Add(Entity e);
        void Update(Entity e);
        void Delete(Entity e);
    }
}
