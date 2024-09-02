# [CollectionView](https://youtu.be/DuNLR_NJv8U?t=12838)

1. Is a view for presenting lists of data.
2. Use different layout specifications.
3. More flexible.
4. Alternative to ListView.

## [Adding Pull-to-Refresh](https://youtu.be/DuNLR_NJv8U?t=12625)

1. In the MainPage.xaml file, wrap the CollectionView element with the RefreshView element as shown in the code below:

```
<ContentPage  ... >

    <Grid ...>

        <RefreshView Grid.ColumnSpan="2">
            <CollectionView ... >
                ....
            </CollectionView>
        </RefreshView>
       
    </Grid>
</ContentPage>
```

2. GridItemsLayout

```
<ContentPage  ... >

    <Grid ...>

        <RefreshView Grid.ColumnSpan="2">
            <CollectionView ... >
                ...

                <CollectionView.ItemsLayout>
                    <GridItemsLayout Orientation="Vertical"
                                     Span="3">                        
                    </GridItemsLayout>
                </CollectionView.ItemsLayout>

                ...

            </CollectionView>
        </RefreshView>
       
    </Grid>
</ContentPage>
```

3. EmptyView


```
<ContentPage  ... >

    <Grid ...>

        <RefreshView Grid.ColumnSpan="2">
            <CollectionView ... >
                ...
                <CollectionView.EmptyView>
                    <StackLayout VerticalOptions="Center"
                                 HorizontalOptions="Center"
                                 WidthRequest="200"
                                 HeightRequest="200">
                        <Image Source="nodata.png"
                               HorizontalOptions="CenterAndExpand"
                               VerticalOptions="CenterAndExpand"
                               Aspect="AspectFill" />
                    </StackLayout>
                </CollectionView.EmptyView>
                ...

            </CollectionView>
        </RefreshView>
       
    </Grid>
</ContentPage>
```

https://youtu.be/DuNLR_NJv8U?t=13487