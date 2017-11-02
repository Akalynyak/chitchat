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
    public class NewTabFragment : Fragment
    {
        NumberPicker NPMin;
        NumberPicker NPSec;


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

            var view = inflater.Inflate(Resource.Layout.NewTab, container, false);

            NPMin = view.FindViewById<NumberPicker>(Resource.Id.NPMinutes);
            NPSec = view.FindViewById<NumberPicker>(Resource.Id.NPSeconds);
            NPSec.MaxValue = 59;
            NPMin.MaxValue = 59;
            NPSec.MinValue = 0;
            NPMin.MinValue = 0;
            NPMin.Value = 1;
            NPSec.Value = 0;

            var createDiscussionButton = view.FindViewById<Button>(Resource.Id.bNDCreate);
            var discussionName = view.FindViewById<EditText>(Resource.Id.etNDName);
            var speechTimeMin = view.FindViewById<NumberPicker>(Resource.Id.NPMinutes);
            var speechTimeSec = view.FindViewById<NumberPicker>(Resource.Id.NPSeconds);
            TimeSpan speechTime = new TimeSpan(0, speechTimeMin.Value, speechTimeSec.Value);
            createDiscussionButton.Click += (sender, e) =>
            {
                Discussion d = new Discussion(discussionName.Text, speechTime);
                var intent = new Intent(Activity, typeof(ParticipantListActivity));
                intent.PutExtra("dis", JsonConvert.SerializeObject(d));
                StartActivity(intent);
            };

            return view;
        }
    }
}