# Etch.OrchardCore.Lever

Lever job API with ability to apply for a job.

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.Lever.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.Lever) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.Lever.svg)](https://www.nuget.org/packages/Etch.OrchardCore.Lever)

## Orchard Core Reference

This module is referencing the RC1 build of Orchard Core ([`1.0.0-rc2-13450`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.0.0-rc2-13450)).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.Lever). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.Lever", ensuring include prereleases is checked.

Alternatively you can [download the source](https://github.com/etchuk/Etch.OrchardCore.Lever/archive/master.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.Lever.

## Usage

This module is using the [Lever posting API](https://github.com/lever/postings-api). 

After installation, the Lever module can be enabled from Features within the OrchardCore admin area. This then enables the Configuration for Lever API which needs to be completed before being able to use the module. (This can be found in Configuration -> Lever -> Api).

### Configuration
***API Key*** Create and manage your API keys from the Integrations and API page in Settings of your Lever account, on the API Credentials tab then enter the API key in this field.

***Site** The site name is unique to each organisation and can be found in Lever admin page.

***Success URL*** The URL to redirect the users after successful application form completion.

***Form Id*** The id of the form to show on each lever posting. (The module exposes a new content type and as a default provides a form to customise and use).

***Locations*** Control which locations should be pulled from API. (Leave blank to get all postings).

### Form
An example form has been provided and uses OrchardCore form fields, it needs to be changed to excatly match the exising form on the Lever.

### Background task
There is a background task which runs every hour to get/update/delete the postings.

### Workflow 
This module provides one workflow Event (Upon successful and failure application completion, this can be used to collect the data internally if needed.)
In addition, there is a workflow Task which can be used to manually trigger to update the postings. (By default there is a background task which runs every hour to get/update/delete the postings)


### Search
This module is using Lucene search and indexes the main fields such as Posting Title, Team, Location, Commitment. An example of the query has been provided and can be customised to your need.

## Packaging

When the theme is compiled (using `dotnet build`) it's configured to generate a `.nupkg` file (this can be found in `\bin\Debug\` or `\bin\Release`).

## Notes

This theme was created using `v0.4.1` of [Etch.OrchardCore.Lever](https://github.com/EtchUK/Etch.OrchardCore.Lever) template.