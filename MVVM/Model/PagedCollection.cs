using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasteToFile.MVVM.Model;

namespace PasteToFile.MVVM.Model
{
    public class PagedCollection<ClipboardContent> : ObservableCollection<ClipboardContent>
    {
        private List<ClipboardContent> _allItems;
        private int _itemsPerPage;
        private int _currentPageIndex;
        private bool _showImage;
        public bool ShowImage
        {
            get { return _showImage; }
            set
            {
                if (_showImage != value)
                {
                    _showImage = value;
                    LoadPage(0);
                }
            }
        }
        private bool _showText;
        public bool ShowText
        {
            get { return _showText; }
            set
            {
                if (_showText != value)
                {
                    _showText = value;
                    LoadPage(0);
                }
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                }
            }
        }

        public PagedCollection(List<ClipboardContent> items, int itemsPerPage)
        {
            _allItems = items;
            _itemsPerPage = itemsPerPage;
            _currentPageIndex = 0;
            _showImage = true;
            _showText = true;
            _searchText = "";
            LoadPage(0);
        }
        public void AddItem(ClipboardContent item)
        {
            _allItems.Insert(0, item);
            LoadPage(0);
        }

        public void LoadPage(int pageIndex)
        {
            _currentPageIndex = pageIndex;
            ClearItems();
            List<ClipboardContent> filteredItems = new List<ClipboardContent>();
            if (!ShowImage || !ShowText)
            {
                foreach (ClipboardContent item in _allItems)
                {
                    if (ShowImage && item is ImageClipboardContent)
                    {
                        filteredItems.Add(item);
                    }
                    else if (ShowText && item is TextClipboardContent)
                    {
                        filteredItems.Add(item);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(SearchText))
            {
                foreach (ClipboardContent item in _allItems)
                {
                    if (item is TextClipboardContent textItem && textItem.FullText.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        filteredItems.Add(item);
                    }
                }
            }
            else
            {
                filteredItems = _allItems;
            }
            foreach (var item in filteredItems.Skip(pageIndex * _itemsPerPage).Take(_itemsPerPage))
            {
                Add(item);
            }
        }
        public int GetPage()
        {
            return _itemsPerPage;
        }

        public int TotalPages => (int)Math.Ceiling((double)_allItems.Count / _itemsPerPage);

        public int CurrentPageIndex => _currentPageIndex;
    }

}
