using Microsoft.Maui.Devices.Sensors;
using MonkeyFinder.Services;

namespace MonkeyFinder.ViewModel;

public partial class MonkeysViewModel : BaseViewModel
{
    public ObservableCollection<Monkey> Monkeys { get; } = new();
    MonkeyService monkeyService;

    IConnectivity connectivity;
    IGeolocation geolocation;

    public MonkeysViewModel(MonkeyService monkeyService,
        IConnectivity connectivity,
        IGeolocation geolocation)
    {
        Title = "Monkey Finder";
        this.monkeyService = monkeyService;

        this.connectivity = connectivity;
        this.geolocation = geolocation;

    }

    [RelayCommand]
    async Task GetClosestMonkey()
    {
        if (IsBusy || Monkeys.Count == 0)
            return;

        try
        {
            // Get cached location, else get real location.
            var location = await geolocation.GetLastKnownLocationAsync();

            if (location is null)
            {
                var GeoRequest = new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(30)
                };

                location = await geolocation.GetLocationAsync(GeoRequest);
            }

            if (location is null)
                return;

            // Find closest monkey to us
            var first = Monkeys.OrderBy(m => location.CalculateDistance(
                                m.Latitude, 
                                m.Longitude, 
                                DistanceUnits.Miles)).FirstOrDefault();

            if (first is null)
                return;

            await Shell.Current.DisplayAlert(
                "Closest Monkey", 
                $"{first.Name} in {first.Location}", 
                "Ok");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get closest monkey: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
    }

    [RelayCommand]
    async Task GoToDetails(Monkey monkey)
    {
        if (monkey == null)
            return;

        await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
        {
            {"Monkey", monkey }
        });
    }

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        if (IsBusy)
            return;

        try
        {
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert(
                    "Internet issue",
                    $"Check your internet and try again!",
                    "OK");

                return;
            }

            IsBusy = true;
            var monkeys = await monkeyService.GetMonkeys();

            if (Monkeys.Count != 0)
                Monkeys.Clear();

            foreach (var monkey in monkeys)
                Monkeys.Add(monkey);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }
}
