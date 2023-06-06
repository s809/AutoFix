using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;

namespace AutoFix
{
    public static class DbUtils
    {
        public static (ObservableCollection<T>, int) WithSelectedIndex<T>(this ObservableCollection<T> collection, int? id, int addToIndex = 0) where T : Entity
        {
            var foundIndex = collection.IndexOf(collection.FirstOrDefault(e => e.Id == id)!);
            return (collection, foundIndex == -1 ? 0 : foundIndex + addToIndex);
        }
    }
}
