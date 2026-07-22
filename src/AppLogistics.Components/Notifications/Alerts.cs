using System.Collections.Generic;

namespace AppLogistics.Components.Notifications;

public class Alerts : List<Alert>
{
    public void Merge(Alerts alerts)
    {
        if (alerts == this)
        {
            return;
        }

        AddRange(alerts);
    }

    public void AddInfo(string message, int timeout = 0)
    {
        Add(new Alert { Type = AlertType.Info, Message = message, Timeout = timeout });
    }

    public void AddError(string message, int timeout = 0)
    {
        Add(new Alert { Type = AlertType.Danger, Message = message, Timeout = timeout });
    }

    public void AddSuccess(string message, int timeout = 0)
    {
        Add(new Alert { Type = AlertType.Success, Message = message, Timeout = timeout });
    }

    public void AddWarning(string message, int timeout = 0)
    {
        Add(new Alert { Type = AlertType.Warning, Message = message, Timeout = timeout });
    }
}
