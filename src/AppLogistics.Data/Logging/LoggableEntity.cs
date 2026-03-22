using AppLogistics.Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppLogistics.Data.Logging;

public class LoggableEntity
{
    public string Name { get; }
    public string Action { get; }
    public Func<int> Id { get; }
    private static string IdName { get; }
    public IEnumerable<LoggableProperty> Properties { get; }

    static LoggableEntity()
    {
        IdName = typeof(BaseModel).GetProperties().Single(property => property.IsDefined(typeof(KeyAttribute), false)).Name;
    }

    public LoggableEntity(EntityEntry<BaseModel> entry)
    {
        PropertyValues values =
            entry.State == EntityState.Modified || entry.State == EntityState.Deleted
                ? entry.GetDatabaseValues()
                : entry.CurrentValues;

        Properties = values.Properties.Where(property => property.Name != IdName).Select(property => new LoggableProperty(entry.Property(property.Name), values[property]));
        Properties = entry.State == EntityState.Modified ? Properties.Where(property => property.IsModified) : Properties;
        Properties = Properties.ToArray();

        Name = entry.Entity.GetType().Name;
        Action = entry.State.ToString();
        Id = () => entry.Entity.Id;
    }

    public override string ToString()
    {
        StringBuilder changes = new StringBuilder();
        foreach (LoggableProperty property in Properties)
        {
            changes.Append(property + Environment.NewLine);
        }

        return changes.ToString();
    }
}
