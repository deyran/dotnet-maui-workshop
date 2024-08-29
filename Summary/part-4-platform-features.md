# [Implementing Platform Features](https://youtu.be/DuNLR_NJv8U?t=10737)

In this section, we will see:

1. how to find the closest monkey from us.
2. Open a map with the monkey's location

## [Check for internet](https://youtu.be/DuNLR_NJv8U?t=10999)


We can easily check to see if our user is connected to the internet with the built in **IConnectivity** of .NET MAUI


1. Open the **MonkeysViewModel** class and inject the **IConnectivity** into the code as shown in the code below:

```
...

public partial class MonkeysViewModel : BaseViewModel
{
    ...

    IConnectivity connectivity;

    public MonkeysViewModel(..., IConnectivity connectivity)
    {
        ...
        this.connectivity = connectivity;
    }
   
   ...
}
```

2. Still in the MonkeysViewModel class, implement the internet connection test as shown in the code below:

```
...

public partial class MonkeysViewModel : BaseViewModel
{
    ...

    [RelayCommand]
    async Task GetMonkeysAsync()
    {
        ... 
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

            ...
        }
        catch (Exception ex)
        {
            ...
        }
        finally
        {
            ...
        }

    }
   
   ...
}
```

3. Now register the service in the MauiProgram file as shown in the code below

```
builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
builder.Services.AddSingleton<IMap>(Map.Default);
```

## [Find Closest Monkey](https://youtu.be/DuNLR_NJv8U?t=11323)

1. Open the **MonkeysViewModel** class and inject the **IGeolocation** into the code as shown in the code below:

```
*** 
public partial class MonkeysViewModel : BaseViewModel
{
    ...
    IConnectivity connectivity; 
    IGeolocation geolocation;

    public MonkeysViewModel(MonkeyService monkeyService, IConnectivity connectivity, IGeolocation geolocation)
    {
        ...
        this.connectivity = connectivity;
        this.geolocation = geolocation;
    }

    ...
}
```

2. Still in the MonkeysViewModel class, implement the [GetClosestMonkey](https://youtu.be/DuNLR_NJv8U?t=11355) method. The purpose of this method is to find the closest Monkey for us:

```
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
```