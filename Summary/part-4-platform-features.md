# [Implementing Platform Features](https://youtu.be/DuNLR_NJv8U?t=10737)

In this section, we will see:

1. how to find the closest monkey from us.
2. Open a map with the monkey's location

## [Check for internet](https://youtu.be/DuNLR_NJv8U?t=10999)


We can easily check to see if our user is connected to the internet with the built in **IConnectivity** of .NET MAUI


1. Open the **MonkeysViewModel** class and implement the **IConnectivity** and shown in the code below:

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

## Find Closest Monkey!