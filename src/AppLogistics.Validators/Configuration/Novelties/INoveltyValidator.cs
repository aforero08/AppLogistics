using AppLogistics.Objects;

namespace AppLogistics.Validators;

public interface INoveltyValidator : IValidator
{
    bool CanCreate(NoveltyView view);
    
    bool CanEdit(NoveltyView view);
    
    bool CanDelete(int id);
}
