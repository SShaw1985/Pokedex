# Pokedex
Pokedex app written in Xamarin.Forms

# Implementation
I have implemented this app using an MVVM approach to development.
I have used Prism as the MVVM framework, DryIOC as the DI service, Akavache for the caching of data and a SOLID software development approach.

The app is built for Android only at this time due to hardware constraints on the development side.

# Rationale
Xamarin is built with MVVM in mind and i used Prism as it is a robust and familiar MVVM framework. others exist and are also good options but prism was chose due to familiarity

DryIOC comes bundles with Prism and is packaged together with it via nuget so I opted to use this over the Xamarin DI service as it fit with Prism.

Akavache is an asynchronous, persistent (i.e. writes to disk) key-value store created for writing desktop and mobile applications in C#, based on SQLite3. i have chosen this due to its east of integration and familiarity

All rest calls are made in plain System.net http client calls. when attempting to use ModernHttpClient with then pokeapi it was noticed that the ModernHttpClient has a known issue with the protocol used so i have removed it. this may effect http call performance on ios

# Functionality
## List Screen
The list screen sidplays cards with pokemon names and images.
There is a type filter which will allos you to search All, Favourites, or specific pokemon types
Favourites will pull a list of all favourited pokemon from the cache.
The previous and next buttons are used when filtering by all to paginate through the app. these buttons are diabled for all other filters.
Filtering by type will pull all pokemon for a type from the api and display all records

## View screen
The view screen displays all required information, image, id, height, weight and types.
I have also added in a carousel image gallery and the pokemon description to give the page some more functionality.
All images are swipable.
There is a favourite button which will add a pokemon to a favourite list which can be viewed on the main screen.

# Caching
All data is cached from the api at the point it is requested.
if the data is present in the cache no api calls are made


