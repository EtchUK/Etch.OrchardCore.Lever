using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch",
    Name = "Lever",
    Version = "1.2.0",
    Website = "https://etchuk.com"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.Lever",
    Name = "Lever posting",
    Description = "Lever job API with ability to apply for a job.",
    Category = "Content",
    Dependencies = new string[] { "OrchardCore.BackgroundTasks", "OrchardCore.Forms", "OrchardCore.Workflows" }
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.Lever.Feeds",
    Name = "Lever posting feeds",
    Description = "Enable RSS feed for lever postings.",
    Category = "Content",
    Dependencies = new string[] { "Etch.OrchardCore.Lever" }
)]