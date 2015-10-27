[![Build status](https://ci.appveyor.com/api/projects/status/jqldyvxke1blix8a?svg=true)](https://ci.appveyor.com/project/nied/tellcore)

## What even is this?
TellCore is a managed .NET wrapper around the ``TelldusCore.dll`` that ships with TelldusCenter.

## Why would I use it?
Why use this one when there are already existing wrappers? Well a number of reasons actually:

* TellCore uses common idioms to provide that C#-feel™.
* As .NET developers we like to live in a garbage-collected world and avoid pointers. TellCore handles this for you. For example, all char-pointers returned by ``TelldusCore.dll`` are converted to strings and the pointer is released in the dll by the client as soon as it is converted. Once you dispose of the client, you can feel safe that no memory has been leaked.
* It's on [NuGet](https://www.nuget.org/packages/TellCore/).

## Requirements
All that's required is that [TelldusCenter](http://www.telldus.se/products/nativesoftware) is installed and running on the machine you want to control.

## Installation
TellCore is on [NuGet](https://www.nuget.org/packages/TellCore/).

## Usage
```cs
using (var client = new TellCoreClient())
{
    int noOfDevices = client.GetNumberOfDevices();
}
```
