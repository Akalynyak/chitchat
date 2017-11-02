using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Discussions
{
    class ResultParticipantAdapter : BaseAdapter<string>
    {
        Participant[] _items;
        Activity _activity;

        public ResultParticipantAdapter(Activity activity, Participant[] items)
        {
            _activity = activity;
            _items = items;
        }

        public void Update(Participant[] items)
        {
            _items = items;
            NotifyDataSetChanged();
        }

        public override string this[int position]
        {
            get
            {
                return _items[position].Name;
            }
        }

        public override int Count
        {
            get
            {
                return _items.Length;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? _activity.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, parent, false);
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = _items[position].Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = "Likes: " + _items[position].GetUpvotes().ToString() + "\t" + "Dislikes: " + _items[position].GetDownvotes().ToString() + "\t" + "Mistakes:" + _items[position].GetMistakes().ToString();
            return view;
        }
    }
}