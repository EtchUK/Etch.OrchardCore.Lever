using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch",
    Category = "Content",
    Description = "Lever job API with ability to apply for a job.",
    Name = "Lever",
    Version = "0.2.8",
    Website = "https://etchuk.com",
    Dependencies = new string[] { "OrchardCore.BackgroundTasks", "OrchardCore.Forms" }
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.Lever.Feeds",
    Name = "Lever posting feeds",
    Description = "Enable RSS feed for lever postings.",
    Category = "Content"
)]