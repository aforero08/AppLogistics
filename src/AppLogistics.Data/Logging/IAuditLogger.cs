using AppLogistics.Objects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;

namespace AppLogistics.Data.Logging;

public interface IAuditLogger : IDisposable
{
    void Log(IEnumerable<EntityEntry<BaseModel>> entries);

    void Save();
}
