# [Implementing Platform Features](https://youtu.be/DuNLR_NJv8U?t=10737)

In this section, we will see:

1. how to find the closest monkey from us.
2. Open a map with the monkey's location

## [Check for internet](https://youtu.be/DuNLR_NJv8U?t=10999)

1. Open the MonkeysViewModel class and implement the **IConnectivity** and shown in the code below:

```
...

public partial class MonkeysViewModel : BaseViewModel
{
    ...

    public MonkeysViewModel(..., IConnectivity connectivity)
    {
        ...
        this.connectivity = connectivity;
    }
   
   ...
}
```

2. AAAA

## Find Closest Monkey!