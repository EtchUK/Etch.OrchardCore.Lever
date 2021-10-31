# Etch.OrchardCore.Lever

Lever job API with ability to apply for a job.

## Build Status

[![Build Status](https://secure.travis-ci.org/etchuk/Etch.OrchardCore.Lever.png?branch=master)](http://travis-ci.org/etchuk/Etch.OrchardCore.Lever) [![NuGet](https://img.shields.io/nuget/v/Etch.OrchardCore.Lever.svg)](https://www.nuget.org/packages/Etch.OrchardCore.Lever)

## Orchard Core Reference

This module is referencing a stable build of Orchard Core ([`1.1.0`](https://www.nuget.org/packages/OrchardCore.Module.Targets/1.1.0)).

## Installing

This module is available on [NuGet](https://www.nuget.org/packages/Etch.OrchardCore.Lever). Add a reference to your Orchard Core web project via the NuGet package manager. Search for "Etch.OrchardCore.Lever", ensuring include prereleases is checked.

Alternatively you can [download the source](https://github.com/etchuk/Etch.OrchardCore.Lever/archive/master.zip) or clone the repository to your local machine. Add the project to your solution that contains an Orchard Core project and add a reference to Etch.OrchardCore.Lever.

## Usage

This module is using the [Lever posting API](https://github.com/lever/postings-api) to pull postings and create content items within Orchard Core.

After installation, enable this module from features within the admin area. Next you'll need to go to the configuration section (found in Configuration -> Lever -> Api) and enter a Lever API key, which is required in order to retrieve postings.

### Configuration

**_API Key_**: Create and manage your API keys from the integrations and API page in settings of your Lever account. The key can be found on the API credentials tab, this needs to be entered in to the API key field.

**_Site_**: Unique to each organisation and can be found in Lever admin page.

**_Success URL_**: URL to redirect users after successful application form completion.

**_Form Id_**: Content item id of form to show on each lever posting. The module exposes a new content type and as a default provides a form to customise and use. If left blank, no application form will be shown.

**_Locations_**: Restrict postings by location, leave blank to get all postings.

### Form

An example form has been provided and uses widgets from [OrchardCore.Forms](https://github.com/OrchardCMS/OrchardCore/tree/dev/src/OrchardCore.Modules/OrchardCore.Forms) module. This will need to be updated to match the form fields expected by Lever for the posting.

### Background task

There is a background task which by default will run every hour to get/update/delete the postings.

### Workflows

#### Posting apply event

This event will be fired when an application has been completed. Ideal for wiring up notifications for when an application has failed or successfully been submitted.

#### Getting and updating lever postings from API

Manually trigger update of postings from API. By default there is a background task which runs every hour to get/update/delete the postings.

### Search

By default various fields are being indexed to enable implementation of a comprehensive job search. Title, team, location & commitment are all indexed and an example query has been provided.

### Features

#### Feeds

There is a XML feed feature included in this module which allows users to access XML feed in [MCV jobs](https://mcvuk.careerwebsite.com/r/jobs/post/batch_specs.cfm) format.   
The URL to access the feed is `/postings/feed/mcv`.

## Packaging

When the theme is compiled (using `dotnet build`) it's configured to generate a `.nupkg` file (this can be found in `\bin\Debug\` or `\bin\Release`).

## Notes

This module was created using `v0.4.1` of [Etch.OrchardCore.ModuleBoilerplate](https://github.com/EtchUK/Etch.OrchardCore.ModuleBoilerplate) template.
