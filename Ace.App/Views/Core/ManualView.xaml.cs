using System;
using System.IO;
using System.Windows.Controls;
using Ace.App.Utilities;

namespace Ace.App.Views.Core
{
    public partial class ManualView : UserControl
    {
        public ManualView()
        {
            InitializeComponent();
            LoadManual();
        }

        private void LoadManual()
        {
            var manualPath = PathUtilities.GetManualPath();

            if (File.Exists(manualPath))
            {
                var markdown = File.ReadAllText(manualPath);
                MarkdownViewer.Markdown = markdown;
            }
            else
            {
                MarkdownViewer.Markdown = "# Benutzerhandbuch\n\nDas Handbuch konnte nicht gefunden werden.\n\nErwarteter Pfad: " + manualPath;
            }
        }
    }
}
