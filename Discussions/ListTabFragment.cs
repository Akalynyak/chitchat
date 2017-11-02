using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Discussions
{
    public class ListTabFragment : Fragment
    {
        DiscussionAdapter adapter;
        List<Discussion> prevDiscussions;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.ListTab, container, false);

            prevDiscussions = JsonConvert.DeserializeObject<List<Discussion>>(Arguments.GetString("prevdis"));

            var list = view.FindViewById<ListView>(Resource.Id.lvMADiscussions);

            adapter = new DiscussionAdapter(Activity, prevDiscussions.ToArray());

            list.Adapter = adapter;

            return view;
        }
    }
}