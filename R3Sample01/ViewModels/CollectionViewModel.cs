using R3;
using R3Sample01.Common;
using System.Collections.ObjectModel;

namespace R3Sample01.ViewModels
{
    public class CollectionViewModel : DisposableBase
    {
        public ObservableCollection<Person> List { get; } = new();
        public BindableReactiveProperty<Person?> SelectedItem { get; } = new();
        public ReactiveCommand AddCommand { get; }

        public CollectionViewModel()
        {
            this.AddCommand = new ReactiveCommand(_ =>
            {
                this.List.Add(new Person());
                this.SelectedItem.Value = this.List[^1];
            });
        }
    }
    public class Person
    {
        public BindableReactiveProperty<string> Name { get; } = new();
        public IReadOnlyBindableReactiveProperty<bool> HasValue { get; }

        public Person()
        {
            this.HasValue = this.Name
                .Select(x => !string.IsNullOrWhiteSpace(x))
                .ToReadOnlyBindableReactiveProperty();
        }
    }
}
