using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch",
    Category = "Content",
    Description = "Lever job API with ability to apply for a job.",
    Name = "Lever",
    Version = "0.2.0",
    Website = "https://etchuk.com",
    Dependencies = new string[] { "OrchardCore.BackgroundTasks", "OrchardCore.Forms" }
)]