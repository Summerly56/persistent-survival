using Robust.Client.UserInterface.Controls;

namespace Content.Client.CrewAssignments.UI
{
    public sealed partial class StationOwnerButton : Button
    {
        public string Owner { get; set; } = "";

        public StationOwnerButton(string owner)
        {
            Owner = owner;
            Text = owner;
        }
    }
}
