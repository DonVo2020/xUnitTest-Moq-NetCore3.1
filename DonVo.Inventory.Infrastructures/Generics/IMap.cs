namespace DonVo.Inventory.Infrastructures
{
    public interface IMap<TModel, TViewModel>
    {
        TViewModel MapToViewModel(TModel model);
        TModel MapToModel(TViewModel viewModel);
    }
}
