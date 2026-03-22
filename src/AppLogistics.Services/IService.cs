using System;

namespace AppLogistics.Services;

public interface IService : IDisposable
{
    int CurrentAccountId { get; set; }
}
