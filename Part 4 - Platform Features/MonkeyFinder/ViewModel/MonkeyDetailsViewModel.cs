namespace MonkeyFinder.ViewModel;

[QueryProperty(nameof(Monkey), "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    IMap map;
    public MonkeyDetailsViewModel(IMap map)
    {
        this.map = map;
    }

    [ObservableProperty]
    Monkey monkey;

    [RelayCommand]
    async Task OpenMapAsync()
    {
        try
        {
            var mapLaunchOptions = new MapLaunchOptions
            { 
                Name = Monkey.Name,
                NavigationMode = NavigationMode.None
            };

            await map.OpenAsync(Monkey.Latitude, 
                                Monkey.Longitude, 
                                mapLaunchOptions);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable open map: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}
