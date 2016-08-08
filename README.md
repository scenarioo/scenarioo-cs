scenarioo-cs
============

C# API for Scenarioo docu content generation

Branch | Status
:---|:---|:---
Master               | [![Build status](https://ci.appveyor.com/api/projects/status/wxm377bueg70428d?branch=release-screen-annotation&svg=true)](https://ci.appveyor.com/project/scenarioo-ci/scenarioo-cs)
Dev                  | [![Build status](https://ci.appveyor.com/api/projects/status/wxm377bueg70428d?branch=release-screen-annotation&svg=true)](https://ci.appveyor.com/project/scenarioo-ci/scenarioo-cs)
Screen Annotation RC | [![Build status](https://ci.appveyor.com/api/projects/status/wxm377bueg70428d?branch=release-screen-annotation&svg=true)](https://ci.appveyor.com/project/scenarioo-ci/scenarioo-cs)

NuGet stats: [![NuGet Downloads](http://img.shields.io/nuget/dt/scenarioo-cs.svg)](https://www.nuget.org/packages/scenarioo-cs)
[![NuGet Version](http://img.shields.io/nuget/v/scenarioo-cs.svg)](https://www.nuget.org/packages/scenarioo-cs)

## Development Guide

### Compile
* open sln
* build the solution to get all nuget packages

You are now ready to run the tests and do coding! Have fun!

### Branch Strategy

`master` commits will automatically be build and published to the nuget feed.

`dev` branch is our current "working" branch. if needed, feature branches are configured on the build system,
please use as branch name `feature-xxx` where xxx is the feature name (for example feature-screen-annotation).

`release-xxx` branches are used to create release candiates which are automatically published to nuget. The
version number gets a `-rc` suffix which nuget will automatically treat as a "pre" release. In order to install
pre packages with nuget, use the `-pre` flag.

### CI Environment
`appveyor.com` is used as our CI environment. There is just one project in appveyor, the different branch configuration
is done within the `appveyor.yml`. Please note that almost all UI settings are ignored if a appveyor.yml is present!

Since the appveyor.yml is public, we have to encrypt all sensitive strings with https://ci.appveyor.com/tools/encrypt.

### Testing
The tests will create an output, which can directly be read by scenarioo. Please check the 
[installation guide](https://github.com/scenarioo/scenarioo/wiki/How%20to%20use%20Scenarioo) to install scenarioo. 
This is how i finally test wheter the scenarioo-cs library works.

### Versioning
The following pattern is used for versioning: `Major.Minor.Builld`. `Major` and `Minor` are given by the scenarioo
format to indicate the alignment of the features. In the future, there will be a repository 
[Scenarioo Format](https://github.com/scenarioo/scenarioo-format).

### How To: Versioning
`appveyor.yml` is used to do the versioning. It should be self explaining.

### Feedback & Bugs
All bugs are tracked with github issues. If you have any improvement or wishes, please feel free to add an issue. We'll
be watching the issue list!

### Core Team
 - [tobiaszuercher](https://github.com/tobiaszuercher) (Tobias Zürcher)
 - [mautechr](https://github.com/mautechr) (Christoph Maute)
 - [felixmokross](https://github.com/felixmokross) (Felix Mokross)

### Contributors
 - feel free to send a pull request :)
