using Robust.Client.UserInterface.Controls;

namespace Content.Client.Access.UI
{
    public sealed partial class AssignmentButton : Button
    {
        public int ID { get; set; }

        public AssignmentButton(int id, string name)
        {
            ID = id;
            Text = name;
        }
    }
}
