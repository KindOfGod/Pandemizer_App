using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Pandemizer.ViewModels;

namespace Pandemizer
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object? data)
        {
            if (data == null) 
                throw new ArgumentNullException(nameof(data));
            
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control) Activator.CreateInstance(type)!;
            }

            return new TextBlock {Text = "Not Found: " + name};
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
    }
}