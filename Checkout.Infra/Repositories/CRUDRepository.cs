using System;
using System.Collections.Generic;
using Checkout.Infra.Interfaces;
using Checkout.Infra.Contexts;
using System.Data.Entity;

namespace Checkout.Infra.Repositories
{
    public class CRUDRepository<Entity> : ICRUD<Entity> where Entity : class
    {
        public CheckoutContext _context;
        public CRUDRepository()
        {
            _context = new CheckoutContext();
        }

        public void Add(Entity e)
        {
            try
            {
                _context.Set<Entity>().Add(e);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void Delete(Entity e)
        {
            try
            {
                _context.Set<Entity>().Remove(e);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public IEnumerable<Entity> GetAll()
        {
            try
            {
                return _context.Set<Entity>();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public Entity GetById(int Id)
        {
            try
            {
                return _context.Set<Entity>().Find(Id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void Update(Entity e)
        {
            try
            {
                _context.Entry(e).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
