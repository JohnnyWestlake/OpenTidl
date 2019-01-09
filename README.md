# OpenTidl for .NET Standard 1.4 & 2.0
**An open source API for TIDAL music streaming service**

This API is written in C# 7.1, with targetting profiles for both .NET Standard 1.4 and 2.0.

*Forked from the original [.NET 4.5 code](https://github.com/jackfagner/OpenTidl) by Jack Fagner. This version is not directly compatible* 

Most (relevant) functions used by TIDAL's web player or Android app are implemented, including:
* Searching (albums, artists, tracks, videos)
* Fetching metadata for albums, artists, tracks (including cover art, artist pictures, etc)
* Logging in using username/password, Facebook, SPID, etc. Only username/password is fully tested.
* Playlist management
* Favorite album/artist/track/playlist management
* Streaming tracks, including lossless, and videos - up to 1080p

Searching and fetching metadata for albums/artists/tracks does not require an active TIDAL subscription.

All API requests require a valid TIDAL Token, which is not provided in this library.

## Installation
This version of OpenTidal is not available on NuGet.

## Disclaimer
This product uses TIDAL but is not endorsed, certified or otherwise approved in any way by TIDAL. TIDAL is the registered trade mark of Aspiro.
