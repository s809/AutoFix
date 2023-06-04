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
        public static (ObservableCollection<T>, int) WithSelectedIndex<T>(this ObservableCollection<T> collection, int id) where T : Entity
            => (collection, Math.Max(0, collection.IndexOf(collection.FirstOrDefault(e => e.Id == id)!)));
    }
}
