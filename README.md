# OpenTidl for .NET Standard 1.4 & 2.0
**Free and open source API for TIDAL music streaming service**

This API is written in C# 7.1, with targetting profiles for both .NET Standard 1.4 and 2.0.

*Forked from the original [.NET 4.5 code](https://github.com/jackfagner/OpenTidl) by Jack Fagner* 

Most (relevant) functions used by TIDAL's web player or Android app are implemented, including:
* Searching (albums, artists, tracks, videos)
* Fetching metadata for albums, artists, tracks (including cover art, artist pictures, etc)
* Logging in using username/password, Facebook, SPID, etc. Only username/password is fully tested.
* Playlist management
* Favorite album/artist/track/playlist management
* Streaming tracks, including lossless, and videos - up to 1080p

Searching and fetching metadata for albums/artists/tracks does not require an active TIDAL subscription.

## Installation
This version of OpenTidal is not yet available on NuGet.

The original .NET 4.5 version of OpenTidl is available on [NuGet](https://www.nuget.org/packages/OpenTidl/)
```
PM> Install-Package OpenTidl
```

## Disclaimer
This product uses TIDAL but is not endorsed, certified or otherwise approved in any way by TIDAL. TIDAL is the registered trade mark of Aspiro.
