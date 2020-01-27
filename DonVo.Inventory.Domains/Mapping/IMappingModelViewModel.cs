namespace DonVo.Inventory.Domains.Mapping
{
    public interface IMappingModelViewModel<TModel, TViewModel>
    {
        TViewModel MapToViewModel(TModel model);
        TModel MapToModel(TViewModel viewModel);
    }
}
