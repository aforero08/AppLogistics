using AppLogistics.Objects;
using System;
using System.Collections.Generic;

namespace AppLogistics.Data.Core;

public interface IUnitOfWork : IDisposable
{
    TDestination GetAs<TModel, TDestination>(int? id) where TModel : BaseModel;

    TModel Get<TModel>(int? id) where TModel : BaseModel;

    TModel GetAsNoTracking<TModel>(int? id) where TModel : BaseModel;

    TDestination To<TDestination>(object source);

    TDestination Map<Tsource, TDestination>(Tsource source, TDestination destination);

    IQuery<TModel> Select<TModel>() where TModel : BaseModel;

    void InsertRange<TModel>(IEnumerable<TModel> models) where TModel : BaseModel;

    void Insert<TModel>(TModel model) where TModel : BaseModel;

    void Update<TModel>(TModel model) where TModel : BaseModel;

    void DeleteRange<TModel>(IEnumerable<TModel> models) where TModel : BaseModel;

    void Delete<TModel>(TModel model) where TModel : BaseModel;

    void Delete<TModel>(int id) where TModel : BaseModel;

    void Commit();
}
