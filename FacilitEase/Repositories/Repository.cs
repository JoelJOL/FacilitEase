using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext Context;

    public Repository(AppDbContext context)
    {
        Context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public T GetById(int id)
    {
        return Context.Set<T>().Find(id);
    }

    public void Add(T entity)
    {
        Context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }
}
