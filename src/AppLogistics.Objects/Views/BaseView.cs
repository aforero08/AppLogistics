using AppLogistics.Components.Extensions.Native;
using AutoMapper;
using System;

namespace AppLogistics.Objects;

public abstract class BaseView
{
    public virtual int Id
    {
        get;
        set;
    }

    public virtual DateTime CreationDate
    {
        get
        {
            if (!IsCreationDateSet)
            {
                CreationDate = DateTime.Now.UtcToDefaultTimeZone();
            }

            return InternalCreationDate;
        }
        protected set
        {
            IsCreationDateSet = true;
            InternalCreationDate = value;
        }
    }

    private bool IsCreationDateSet
    {
        get;
        set;
    }

    private DateTime InternalCreationDate
    {
        get;
        set;
    }
}

public abstract class BaseView<TModel> : BaseView
{
    internal virtual void Map(Profile profile)
    {
        profile.CreateMap(typeof(TModel), GetType()).ReverseMap();
    }
}
