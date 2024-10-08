# [Implementing navigation in .Net MAUI # passing parameters](https://youtu.be/DuNLR_NJv8U?t=8763)

1. The GoToDetailsAsync method, the method implemented in the code below,  is responsible for navigation to the DetailsPage while passing a monkey object as a parameter. Open the **MonkeysViewModel** class and add the code below:
   
```
[RelayCommand]
async Task GoToDetailsAsync(Monkey monkey)
{
    if (monkey is null)
        return;

    var monkeyDictionary = new Dictionary<string, object>
    {
        {"Monkey", monkey }
    };

    await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, monkeyDictionary);
}
```

* The method GoToDetailsAsync navigates to a page named DetailsPage using Shell.Current.GoToAsync.

* The second argument (true) Shell.Current.GoToAsync line indicates that the navigation should be animated (e.g., sliding or fading between pages).

* A new *Dictionary<string, object>* called **monkeyDictionary** is created to pass a monkey object using the key "Monkey". This allows the monkey object to be associated with that specific key during navigation.

2. We need to call this method. Open and edit **MainPage.xaml** as shown in the code below:

```

<ContentPage ... >
    <Grid ...>
        <CollectionView ...>
            <CollectionView.ItemTemplate>
                <DataTemplate ...>
                    <Grid ...>
                        <Frame ...>

                            <Frame.GestureRecognizers>
                                <TapGestureRecognizer 
                                        Command="{Binding Source={RelativeSource AncestorType={x:Type viewmodel:MonkeysViewModel}}, Path=GoToDetailsCommand}"
                                        CommandParameter="{Binding .}"/>
                            </Frame.GestureRecognizers>
        ...                            
    </Grid>
</ContentPage>
```

3. Passing the object to a specific page. Open and edit **DetailsPage.xaml.cs** as shown in the code below:

```
namespace MonkeyFinder;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(MonkeyDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }
}
```

4. Open and edit **MonkeyDetailsViewModel.cs** as shown in the code below:

```
namespace MonkeyFinder.ViewModel;

[QueryProperty("Monkey", "Monkey")]
public partial class MonkeyDetailsViewModel : BaseViewModel
{
    public MonkeyDetailsViewModel()
    {        
    }

    [ObservableProperty]
    Monkey monkey;
}
```

5. Now it's time to register the route in the **AppShell.xaml.cs** file.
   
```
namespace MonkeyFinder;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
    }
}
```

6. Open the **MauiProgram.cs** to add **build.Services** as shown in the code below:
   
```
...
builder.Services.AddSingleton<MonkeyService>();

builder.Services.AddSingleton<MonkeysViewModel>();
builder.Services.AddTransient<MonkeyDetailsViewModel>();

builder.Services.AddSingleton<MainPage>();
builder.Services.AddTransient<DetailsPage>();
...
```
   
# [Create DetailsPage.xaml UI](https://youtu.be/DuNLR_NJv8U?t=9868)

## DetailsPage.xaml
  
1. Defining our DataType by defining the view model namespace and also setting the title:
   
```
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="MonkeyFinder.DetailsPage"
                          
             xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"
             x:DataType="viewmodel:MonkeyDetailsViewModel"            
             Title="{Binding Monkey.Name}">

    <Label Text="{Binding Monkey.Name}"
           FontSize="25" />

</ContentPage>
```

* **xmlns:viewmodel="clr-namespace:MonkeyFinder.ViewModel"** - This statement defines viewmodel as alias for the classes in the **MonkeyFinder.ViewModel** folder. This makes it easier to refer to those classes in the XAML code.

* **x:DataType="viewmodel:MonkeyDetailsViewModel"** - This line tells the XAML page the binding context (source of data to display). The UI elements will use the data and properties from the **MonkeyDetailsViewModel** class.

* These two lines work together like this:

    1. The **viewmodel** alias is created to easier access to the classes  from the **MonkeyFinder.ViewModel** folder

    2. the second line uses the **viewmodel** alias to tell the XAML page that **MonkeyDetailsViewModel** class will be used as the binding context.

2. `ScrollView`, `VerticalStackLayout` and `Grid` to layout
   
```
<ScrollView>
    <VerticalStackLayout>
        <Grid ColumnDefinitions="*,Auto,*"
                RowDefinitions="160, Auto">
                ....
        </Grid>
    </VerticalStackLayout>
</ScrollView>
```

3. Putting the monkey picture in the middle of the screen

```
<ScrollView>
    <VerticalStackLayout>
        <Grid ColumnDefinitions="*,Auto,*"
                RowDefinitions="160, Auto">

            <BoxView
                BackgroundColor="{StaticResource Primary}"
                Grid.ColumnSpan="3"
                HeightRequest="160" 
                HorizontalOptions="FillAndExpand"/>

            <Frame Grid.RowSpan="2"
                    Grid.Column="1"
                    HeightRequest="160"
                    WidthRequest="160"
                    CornerRadius="80"
                    HorizontalOptions="Center"
                    IsClippedToBounds="True"
                    Padding="0"
                    Margin="0,80,0,0">

                <Image Aspect="AspectFill"
                        HeightRequest="160"
                        WidthRequest="160"
                        HorizontalOptions="Center"
                        VerticalOptions="Center"
                        Source="{Binding Monkey.Image}" />
            </Frame>
            
        </Grid>
    </VerticalStackLayout>
</ScrollView>
```

4. Now we're gonna to put some information

```
...
<ScrollView>
    <VerticalStackLayout>
        <Grid ColumnDefinitions="*,Auto,*"
                RowDefinitions="160, Auto">
        ...
        </Grid>   

        <VerticalStackLayout Padding="10" Spacing="10" HorizontalOptions="Center">
            <Label Text="{Binding Monkey.Details}" 
                    Style="{StaticResource MediumLabel}" />
            <Label Text="{Binding Monkey.Location, StringFormat='Location: {0}'}" 
                    Style="{StaticResource SmallLabel}"/>
            <Label Text="{Binding Monkey.Population, StringFormat='Population: {0}'}" 
                    Style="{StaticResource SmallLabel}"/>
        </VerticalStackLayout>

    </VerticalStackLayout>
</ScrollView>
...
```