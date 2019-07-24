using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Services.Exceptions;
using RentCar.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentCar.Services
{
    public class Service<TEntity> : Iservice<TEntity> where TEntity : class
    {
        private readonly Context _context;
        public Service(Context context)
        {
            _context = context;
        }

        // Listar todos:
        public async Task<List<TEntity>> FindAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        // Buscar por Id:
        public async Task<TEntity> FindByIdAsync(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        // Inserir elemento:
        public async Task InsertAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // Atualizar elemento:
        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }

        // Remover elemento:
        public async Task RemoveAsync(string id)
        {
            try
            {
                var entity = await FindByIdAsync(id);
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Não pode ser excluído pois existem itens associados a este elemento.");
            }

        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var entity = await FindByIdAsync(id);
                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Não pode ser excluído pois existem itens associados a este elemento.");
            }

        }
    }
}
