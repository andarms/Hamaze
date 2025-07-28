namespace Hamaze.Engine.Core;

// <summary>
// Traits are lightweight data containers that define the state or identity of a game object.
// They do not contain logic, they're purely declarative and can be added or removed dynamically at runtime.
// </summary>
public record Trait(string Name) { }
