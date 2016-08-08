scenarioo-cs
============

C# API for Scenarioo docu content generation

Branch | Status
:---|:---|:---
Master               | [![Build status](https://ci.appveyor.com/api/projects/status/wxm377bueg70428d?branch=release-screen-annotation&svg=true)](https://ci.appveyor.com/project/scenarioo-ci/scenarioo-cs)
Dev                  | [![Build status](https://ci.appveyor.com/api/projects/status/wxm377bueg70428d?branch=release-screen-annotation&svg=true)](https://ci.appveyor.com/project/scenarioo-ci/scenarioo-cs)

NuGet stats: [![NuGet Downloads](http://img.shields.io/nuget/dt/scenarioo-cs.svg)](https://www.nuget.org/packages/scenarioo-cs)
[![NuGet Version](http://img.shields.io/nuget/v/scenarioo-cs.svg)](https://www.nuget.org/packages/scenarioo-cs)

## Development Guide

### Compile
* open sln
* build the solution to get all nuget packages

You are now ready to run the tests and do coding! Have fun!

### Branch Strategy / Version Strategy
gitversion is used for the versioning. The documentation can be found here: http://gitversion.readthedocs.io.

`master` commits will automatically be build and published to the nuget feed.

`dev` commits will automatically be pushed as pre-release nuget packages to the official nuget feed, 
e.g. `scenarioo-cs.2.1.4-pre0056`. Those packages are for the brave developers out there!

`feature/xxx` aka feature branches can be used to implement your feature. The nuget packages will be published
to an private nuget feed: https://ci.appveyor.com/nuget/scenarioo. This may be useful for testing new features
before making them publicly available.

`release/xxx` branches are used to create release candiates which are automatically published to nuget. The
version number gets a `-rc` suffix which nuget will automatically treat as a "pre" release, e.g. 
`scenarioo-cs.2.1.6-rc0012`. NuGet doesn't really differentiate between `rc` and `pre`. We consider `rc` as more
stable, or if we have a new feature and we want to do some final tests with other projects/stakeholders. 
In order to install pre packages with nuget, use the `-pre` flag in the nuget cli.

### CI Environment
Appveyor is used to build scenarioo-cs. There is just one project in appveyor, the different branch configuration
is done within the `appveyor.yml`. Please note that almost all UI settings are ignored if a appveyor.yml is present!

Since the appveyor.yml is public, we have to encrypt all sensitive strings with https://ci.appveyor.com/tools/encrypt.

### Testing
The tests will create an output, which can directly be read by scenarioo. Please check the 
[installation guide](https://github.com/scenarioo/scenarioo/wiki/How%20to%20use%20Scenarioo) to install scenarioo. 
This is how i finally test wheter the scenarioo-cs library works.

### Versioning
The general versioning strategy is described in the Branch Strategy chapter. In general we have the numbers aligned to
the number in [Scenarioo Format](https://github.com/scenarioo/scenarioo-format).

### Feedback & Bugs
All bugs are tracked with github issues. If you have any improvement or wishes, please feel free to add an issue. We'll
be watching the issue list!

### Core Team
 - [tobiaszuercher](https://github.com/tobiaszuercher) (Tobias Zürcher)
 - [mautechr](https://github.com/mautechr) (Christoph Maute)
 - [felixmokross](https://github.com/felixmokross) (Felix Mokross)

### Contributors
 - feel free to send a pull request :)
